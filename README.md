# OSU Connections in a Graph
Made in collaboration with @praeludiumOrbis

## The project
Displaying the connections in the OSU community and its country clusters in a graph. 

## Examples
<img src="Examples/world.jpg" width="70%" />

## Usage
<p>
Simplest way to run is to install the dotnet 8 runtime and run as local build.<br>
Create an appsettings.json in the root folder, consult appsettings.example.json for reference.<br>
Set your OSU api key and the delay between each api call
</p>

```
dotnet run -- <arguments>
dotnet run -- --help
```

<p>
The result will be edges.csv and nodes.csv, ready for import into Gephi.
</p>
