using B2C_visualizer.Model;

namespace B2C_visualizer.Comparison
{
    internal class RoleWithDefiningParent : Role, IEquatable<RoleWithDefiningParent?>
    {
        public string? DefiningAppId { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as RoleWithDefiningParent);
        }

        public bool Equals(RoleWithDefiningParent? other)
        {
            return other is not null &&
                   Id == other.Id &&
                   Description == other.Description &&
                   DisplayName == other.DisplayName &&
                   Value == other.Value &&
                   IsEnabled == other.IsEnabled &&
                   Type == other.Type &&
                   DefiningAppId == other.DefiningAppId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Description, DisplayName, Value, IsEnabled, Type, DefiningAppId);
        }

        public static bool operator ==(RoleWithDefiningParent? left, RoleWithDefiningParent? right)
        {
            return EqualityComparer<RoleWithDefiningParent>.Default.Equals(left, right);
        }

        public static bool operator !=(RoleWithDefiningParent? left, RoleWithDefiningParent? right)
        {
            return !(left == right);
        }
    }
}