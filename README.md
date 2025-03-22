# OSU Connections in a Graph
Made in collaboration with [praeludiumOrbis](https://github.com/praeludiumOrbis)

## The project
This C# tool collects data from osu! user profiles. It recursively scrapes profiles starting from given user IDs, extracts connections (collabs), and processes the data into two CSV files—one for nodes (users) and one for edges (collaborations). This output can be directly imported into [Gephi](https://gephi.org/) for network analysis.

## Examples
<img src="Examples/world.jpg" width="70%" />

## Usage
<p>
The implest way to run is to install the .NET 8 runtime and run as local build.<br>
Create an appsettings.json in the root folder, consult appsettings.example.json for reference.<br>
Don't forget to set your osu! api key and the delay between each api call
</p>

```
dotnet run -- <arguments>
dotnet run -- --help
```

### Example

```bash
dotnet run -- -u "12345,67891,13456" -d 3
```

<p>
The result will be edges.csv and nodes.csv, ready for import into Gephi.
</p>
