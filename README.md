# update-version

![GitHub](https://img.shields.io/github/license/Monbsoft/UpdateVersion)
![Build](https://github.com/Monbsoft/UpdateVersion/workflows/Build/badge.svg)
![Nuget](https://img.shields.io/nuget/v/Monbsoft.UpdateVersion)

update-version is a developer tool to update the .NET project versions.

## Setup update-version

Install update-version via the following command:

```text
$ dotnet tool install -g Monbsoft.UpdateVersion
```

## How To Use

Run `update-version --help` for information about usage.

```text
update-version:
  Developer tool to update the Visual Studio project versions.

Usage:
  update-version [options] [command]

Options:
  -?, -h, --help    Show help and usage information
  --version         Show version information

Commands:
  list             Lists all project versions
  major            Increment major version number
  minor            Increment minor version number
  patch            Increment patch version number
  build            Increment build version number
  pre              Increment pre-release version number
  set <version>    Set the version of the projects.
```

## Usage

### Update minor version

Update minor versions with the following command:

```text
$ update-version minor
```

### Set version

Set versions with following command:

```text
$ update-version set 1.0.2
```

### Update Major version with git commit

Update major versions via the following command:

```text
$ update-version major  --message "Change version"
```

### Git options

Git options via the following command:

```text
Options:
  -m, --message <message>                    Message of the git commit
  -a, --add                                  All files in the entire working tree
  -t, --tag                                  Tag of the git commit
```

## Credit

- Jon Grythe Stødle, [[jonstodle / DotnetVersion](https://github.com/jonstodle/DotnetVersion)], DotnetVersion : A simple tool to update the version number of your project. If you know of "yarn version", this is that for NET.
- David Fallah,[[TAGC / dotnet-setversion](https://github.com/TAGC/dotnet-setversion)], dotnet-setversion: .NET Core CLI tool to update the version information in .NET Core \*.csproj files.
- [[dotnet / tye](https://github.com/dotnet/tye)], Tye is a tool that makes developing, testing, and deploying microservices and distributed applications easier. Project Tye includes a local orchestrator to make developing microservices easier and the ability to deploy microservices to Kubernetes with minimal configuration.
- [[dotnet / command-line-api]](https://github.com/dotnet/command-line-api),command-line-api: Command line parsing, invocation, and rendering of terminal output.
