# dotnet ROSMSG

## Installation

```
dotnet tool install -g RobSharper.Ros.MessageCli --version 1.0.0-RC1
```

## Configuration

```
dotnet rosmsg config --help
```

### Custom NuGet feeds
ROSMSG support custom nuget feeds.

List configured NuGet feeds:
```
dotnet rosmsg config feeds
```

Add new feed:
```
dotnet rosmsg config feeds add <NAME> --source <URL|PATH> [--protocol <PROTOCOL_VERSION>]
```

Remove an existing feed:
```
dotnet rosmsg config feeds remove <NAME>
```

## Usage

TODO