using B2C_visualizer.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace B2C_visualizer.Comparison
{
    internal class ServicePrincipalComparer
    {
        private readonly string[] environmentIdentifiers = [
            "dev-",
            "test-"
        ];

        public void Compare(IEnumerable<ServicePrincipal> sps)
        {
            //billing
            //var s1 = sps.ToArray()[1];//.GrantedResourceAccesses.ToArray();
            //var s2 = sps.ToArray()[16];//.GrantedResourceAccesses.ToArray();


            var kafkas = sps.Where(s => s.Name.Contains("kafka")).ToArray();

            var s1 = kafkas[0];
            var s2 = kafkas[1];


            var names = sps.Select(s => s.Name).ToList();


            string expected = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "comparison.html");

            var environmentSets = new Dictionary<string, IEnumerable<ServicePrincipal>>();

            for (int i = 0; i < environmentIdentifiers.Length; i++)
            {
                environmentSets[environmentIdentifiers[i]] = sps.Where(sp => sp.Name.Contains($"{environmentIdentifiers[i]}")).ToArray();
            }

            if (environmentSets.Count() != 2)
            {
                throw new NotSupportedException($"You must compare exactly two sets of service principals at a time. Found service principals belong to these environments: {string.Join(',', environmentSets.Keys)}");
            }

            var comparisons = CompareCollection("Everything", environmentSets.First().Value, environmentSets.Last().Value, 0, FindComparisons!, sp => RemoveEnvSpecificToken(sp.Name));


            EvaluateComparisons(comparisons);

            var html = GenerateHtmlReport(comparisons);

            File.WriteAllText(expected, html);

            OpenReport(expected);
        }

        private IEnumerable<Comparison> FindComparisons(ServicePrincipal sp1, ServicePrincipal sp2, int nestingLevel)
        {
            
            List<Comparison> comparisons =
            [
                new Comparison { Property = nameof(sp1.DisplayName), Value1 = sp1?.DisplayName, Value2 = sp2?.DisplayName, NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.EnvSpecific },
                new Comparison { Property = nameof(sp1.Name), Value1 = sp1?.Name, Value2 = sp2?.Name, NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.EnvSpecific},
                new Comparison { Property = nameof(sp1.AppId), Value1 = sp1?.AppId, Value2 = sp2?.AppId, NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.Different },
            ];

            comparisons.AddRange(CompareCollection(nameof(sp1.DefinedAppRoles), sp1?.DefinedAppRoles, sp2?.DefinedAppRoles, nestingLevel, FindComparisons, r => r.Value));
            comparisons.AddRange(CompareCollection(nameof(sp1.DefinedOauth2Permissions), sp1?.DefinedOauth2Permissions, sp2?.DefinedOauth2Permissions, nestingLevel, FindComparisons, p => p.Value));
            comparisons.AddRange(CompareCollection(nameof(sp1.GrantedResourceAccesses), sp1?.GrantedResourceAccesses, sp2?.GrantedResourceAccesses, nestingLevel, FindComparisons, r => r.AppId));
            comparisons.AddRange(CompareCollection(nameof(sp1.Secrets), sp1?.Secrets, sp2?.Secrets, nestingLevel, FindComparisons, s => s.DisplayName));
            comparisons.AddRange(CompareCollection(nameof(sp1.CallbackUrls), sp1?.CallbackUrls, sp2?.CallbackUrls, nestingLevel, FindComparisons, cb => cb.Url));

            comparisons.AddRange(CompareCollection(nameof(sp1.IdentifierUris), sp1?.IdentifierUris, sp2?.IdentifierUris, nestingLevel, FindIdentifierUrisComparisons, u => u));

            return comparisons;
        }

        private IEnumerable<Comparison> FindIdentifierUrisComparisons(string? s1, string? s2, int nestingLevel)
        {
            return
            [
                new Comparison { Property = "Value", Value1 = s1, Value2 = s2, ExpectedSimilarity = Similarity.EnvSpecific, NestingLevel = nestingLevel },
            ];
        }

        private IEnumerable<Comparison> FindComparisons(Role? r1, Role? r2, int nestingLevel)
        {
            return
            [
                new Comparison { Property = nameof(r1.Type), Value1 = r1?.Type.ToString(), Value2 = r2?.Type.ToString(), NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.Same },
                new Comparison { Property = nameof(r1.Id), Value1 = r1?.Id, Value2 = r2?.Id, NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.DontCare },
                new Comparison { Property = nameof(r1.DisplayName), Value1 = r1?.DisplayName, Value2 = r2?.DisplayName, NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.Same },
                new Comparison { Property = nameof(r1.Value), Value1 = r1?.Value, Value2 = r2?.Value, NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.Same },
                new Comparison { Property = nameof(r1.IsEnabled), Value1 = r1?.IsEnabled.ToString(), Value2 = r2?.IsEnabled.ToString(), NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.Same },
                new Comparison { Property = nameof(r1.Description), Value1 = r1?.Description, Value2 = r2?.Description, NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.Same },
            ];
        }

        private IEnumerable<Comparison> FindComparisons(Secret? s1, Secret? s2, int nestingLevel)
        {
            return
            [
                new Comparison { Property = nameof(s1.Id), Value1 = s1?.Id, Value2 = s2?.Id, NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.Different },
                new Comparison { Property = nameof(s1.DisplayName), Value1 = s1?.DisplayName, Value2 = s2?.DisplayName, NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.Same },
                new Comparison { Property = nameof(s1.Hint), Value1 = s1?.Hint, Value2 = s2?.Hint, NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.Different },
                new Comparison { Property = nameof(s1.StartDate), Value1 = s1?.StartDate.ToString(), Value2 = s2?.StartDate.ToString(), NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.Different },
                new Comparison { Property = nameof(s1.EndDate), Value1 = s1?.EndDate.ToString(), Value2 = s2?.EndDate.ToString(), NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.Different },
            ];
        }

        private IEnumerable<Comparison> FindComparisons(CallBackUrl? c1, CallBackUrl? c2, int nestingLevel)
        {
            return
            [
                new Comparison { Property = nameof(c1.Type), Value1 = c1?.Type, Value2 = c1?.Type, NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.Same },
                new Comparison { Property = nameof(c1.Url), Value1 = c1?.Url, Value2 = c1?.Url, NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.Different },
            ];
        }

        private IEnumerable<Comparison> FindComparisons(Resource? r1, Resource? r2, int nestingLevel)
        {
            List<Comparison> comparisons =
            [
                new Comparison { Property = nameof(r1.AppId), Value1 = r1?.AppId.ToString(), Value2 = r2?.AppId.ToString(), NestingLevel = nestingLevel, ExpectedSimilarity = Similarity.DontCare },
            ];

            comparisons.AddRange(CompareCollection(nameof(r1.Roles), r1?.Roles, r2?.Roles, nestingLevel, FindComparisons, r => r.Value));

            return comparisons;
        }

        private IEnumerable<Comparison> CompareCollection<T>(string collectionName, IEnumerable<T>? collection1, IEnumerable<T>? collection2, int nestingLevel, Func<T?, T?, int, IEnumerable<Comparison>> comparerFunc, Func<T, string> keySelectorFunc)
        {
            List<Comparison> totalComparisons = new List<Comparison>();


            totalComparisons.Add(new Comparison { Property = collectionName, IsParent = true });

            var set1 = collection1 != null ? collection1 : new T[0];
            var set2 = collection2 != null ? collection2 : new T[0];

            var maxCount = Math.Max(set1.Count(), set2.Count());
            var counter = 0;

            while (true)
            {
                var val1 = set1.Any() ? set1.First() : default;
                var val2 = default(T?);
                if (val1 != null)
                {
                    set1 = set1.Where(s => !ReferenceEquals(s, val1));

                    var matchingVal2s = set2.Where(s2 => keySelectorFunc(s2) == keySelectorFunc(val1));
                    if (matchingVal2s.Any())
                    {
                        val2 = matchingVal2s.First();
                        set2 = set2.Where(s => !ReferenceEquals(s, val2));
                    }
                }
                else
                {
                    val2 = set2.Any() ? set2.First() : default;
                    if (val2 != null)
                    {
                        set2 = set2.Where(s => !ReferenceEquals(s, val2));

                    }
                }

                if (val1 == null && val2 == null)
                    break;

                var itemComparisons = comparerFunc(val1, val2, nestingLevel +1);

                totalComparisons.Add(new Comparison { Property = $"{collectionName}.[{counter}]", IsParent = true });

                foreach (var c in itemComparisons)
                {
                    c.Property = $"{collectionName}.[{counter}].{c.Property}";
                }
                totalComparisons.AddRange(itemComparisons);
                
                counter++;
            }

            return totalComparisons;
        }


        private void EvaluateComparisons(IEnumerable<Comparison> comparisons)
        {
            foreach (var c in comparisons)
            {
                EvaluateComparison(c);
            }
        }

        private void EvaluateComparison(Comparison comparison)
        {
            if (comparison.IsParent)
            {
                comparison.ResultedSimilarity = Similarity.Undefined;
            }
            else
            {
                comparison.ResultedSimilarity = Similarity.Different;

                if (comparison.Value1 == null && comparison.Value2 == null ||
                    comparison.Value1 != null && comparison.Value1.Equals(comparison.Value2, StringComparison.Ordinal))
                {
                    comparison.ResultedSimilarity = Similarity.Same;
                }
                else if (comparison.Value1 != null && comparison.Value2 != null)
                {
                    (string env1, string env2, bool hasEnvSpecificDifference) = EvaluateEnvSpecificDifference(comparison.Value1, comparison.Value2);
                    if (hasEnvSpecificDifference)
                    {
                        comparison.ResultedSimilarity = Similarity.EnvSpecific;
                        comparison.ResultedEnv1 = env1;
                        comparison.ResultedEnv2 = env2;
                    }
                }
            }
        }

        private string RemoveEnvSpecificToken(string value)
        {
            for (int i = 0; i < environmentIdentifiers.Length; i++)
            {
                string envId = environmentIdentifiers[i];
                var valueParts = value.Split(envId, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (valueParts.Length > 1)
                {
                    return string.Join("", valueParts);
                }
            }
            return value;
        }

        private (string, string, bool) EvaluateEnvSpecificDifference(string value1, string value2)
        {
            for (int first = 0; first < environmentIdentifiers.Length; first++)
            {
                string firstEnvID = environmentIdentifiers[first];
                var value1Parts = value1.Split(firstEnvID, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                for (int second = first; second < environmentIdentifiers.Length; second++)
                {
                    string secondEnvID = environmentIdentifiers[second];
                    var value2Parts = value2.Split(secondEnvID, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    if (value1Parts.Length > 0 && value2Parts.Length > 0 && value1Parts[^1] == value2Parts[^1])
                    {
                        return (firstEnvID, secondEnvID, true);
                    }
                }
            }

            return (string.Empty, string.Empty, false);
        }

        private string GenerateHtmlReport(IEnumerable<Comparison> comparisons)
        {
            var header = """
                <!DOCTYPE html>
                <html lang="en">
                <head>
                    <meta charset="UTF-8">
                    <meta name="viewport" content="width=device-width, initial-scale=1.0">
                    <meta http-equiv="X-UA-Compatible" content="ie=edge">
                    <title>Comparison</title>
                    <style>
                        .diff-table tr:nth-child(odd) { background-color:#eee; }
                        .diff-table tr:nth-child(even) { background-color:#fff; }
                      .bad {background-color:red;}
                      .good {background-color:lightgreen;}
                      tr {transition: height 0.5s ease;}
                    </style>
                </head>
                <body>
                <table class="diff-table">
                    <thead>
                        <th>Property</th>
                        <th>Value 1</th>
                        <th>Value 2</th>
                        <th>Expectancy</th>
                        <th>Result</th>
                    </thead>
                """;
            var footer = """
                </table>
                </body>
                </html>
                """;

            var html = new StringBuilder(); 
            html.AppendLine(header);

            int mismatches = 0;

            var coms = comparisons.ToArray();
            for (int i = 0;i< coms.Length; i++)
            {
                var isParent = i + 1 < coms.Length && coms[i + 1].Property.Contains(coms[i].Property);
                var asExpected = coms[i].ExpectedSimilarity == coms[i].ResultedSimilarity ||
                                 coms[i].ExpectedSimilarity == Similarity.DontCare;
                if (!asExpected)
                {
                    mismatches++;
                }

                html.AppendLine($"""
                <tr class="level{coms[i].NestingLevel} {(isParent? "parent" : "")}">
                    <td class="prop">{HttpUtility.HtmlEncode(coms[i].Property)}</td>
                    <td class="value1">{HttpUtility.HtmlEncode(coms[i].Value1)}</td>
                    <td class="value2">{HttpUtility.HtmlEncode(coms[i].Value2)}</td>
                    <td class="expectancy">{GetSimilarityPrettyName(coms[i].ExpectedSimilarity)}</td>
                    <td class="result {(asExpected ? "good" : "bad")}">{GetResultString(coms[i])}</td>
                </tr>
                """);
            }

            html.AppendLine($"""
                <tr class="total">
                    <td class="totalValue">Total Rows {coms.Length}</td>
                    <td class=""></td>
                    <td class=""></td>
                    <td class=""></td>
                    <td class="totalValue">Total Mismatches {mismatches}</td>
                </tr>
                """);

            html.AppendLine(footer);

            return html.ToString();
        }

        private string GetResultString(Comparison c)
        {
            switch (c.ResultedSimilarity)
            {
                case Similarity.Same: return GetSimilarityPrettyName(c.ResultedSimilarity);
                case Similarity.Different: return GetSimilarityPrettyName(c.ResultedSimilarity);
                case Similarity.DontCare: return GetSimilarityPrettyName(c.ResultedSimilarity);
                case Similarity.EnvSpecific: return $"{GetSimilarityPrettyName(c.ResultedSimilarity)} ({c.ResultedEnv1}, {c.ResultedEnv2})";
                case Similarity.Undefined: return GetSimilarityPrettyName(c.ResultedSimilarity);
                default: throw new NotImplementedException();
            }
        }

        private string GetSimilarityPrettyName(Similarity s)
        {
            switch (s)
            {
                case Similarity.Same: return "Same";
                case Similarity.Different: return "Different";
                case Similarity.DontCare: return "Don't Care";
                case Similarity.EnvSpecific: return "Env Specific";
                case Similarity.Undefined: return "Undefined";
                default: throw new NotImplementedException();
            }
        }

        public void OpenReport(string filePath)
        {
            Process process = new Process();

            process.StartInfo.FileName = filePath;
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }
    }
}
