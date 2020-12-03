# UpdateVersion

Update-Version is a developper tool to update the .Net project versions.

## Installing UpdateVersion

Install UpdateVersion via the following command:

```text
$ dotnet tool install -g Monbsoft.UpdateVersion
```

## How To Use

Run `update-version --help` for information about usage.

```text
update-version:
  Developper tool to update the Visual Studio project versions.

Usage:
  update-version [options] [command]

Options:
  -?, -h, --help    Show help and usage information
  --version         Show version information

Commands:
  list            Lists all project versions
  major           Increments major version number
  minor           Increments minor version number
  patch           Increments patch version number
  set <vesion>    Sets the version of the projects.
```

## Usage

### Major version update

Update major versions via the following command:

```text
$ UpdateVersion major
```

## Credit

- Jon Grythe Stødle, [[jonstodle / DotnetVersion](https://github.com/jonstodle/DotnetVersion)], DotnetVersion : A simple tool to update the version number of your project. If you know of "yarn version", this is that for NET.
- David Fallah,[[TAGC / dotnet-setversion](https://github.com/TAGC/dotnet-setversion)], dotnet-setversion: .NET Core CLI tool to update the version information in .NET Core \*.csproj files.
- [[dotnet / tye](https://github.com/dotnet/tye)], Tye is a tool that makes developing, testing, and deploying microservices and distributed applications easier. Project Tye includes a local orchestrator to make developing microservices easier and the ability to deploy microservices to Kubernetes with minimal configuration.
