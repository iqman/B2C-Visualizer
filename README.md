# B2C-Visualizer

Small commandline utility for generating a visualization of Microsoft Azure AD B2C service principals, their defined app roles, scopes and granted app roles and scopes.

## Input
- The raw service principal JSON manifest that can be manually downloaded from the Azure portal.
- IDs of service principals to automatically download and a access token that allows such download.

## Output
- Mermaid visualization
- PlantUML visualization

Example of usage (Assuming a set of service principal manifests have been downloaded and put into .json files inside a folder called "sp"):
```
B2C-visualizer -i sp -o visualization.txt -f PlanUML
```
