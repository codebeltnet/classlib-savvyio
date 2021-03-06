![Savvy I/O](.nuget/Savvyio.Core/icon.png)

# Savvy I/O

An open-source project (MIT license) that provides a SOLID and clean .NET class library for writing DDD, CQRS and Event Sourcing applications.

![Savvy I/O Flow](https://static.savvyio.net/savvyio.png)

It is, by heart, free, flexible and built to extend and boost your agile codebelt.

## Motivation

Savvy I/O is designed to be intuitive and follows many of the same patterns and practices that was applied to [Cuemon for .NET](https://github.com/gimlichael/Cuemon).

The grand idea and motivation was to remove the complexity normally associated with DDD, CQRS and Event Sourcing.

## State of the Union

All CI and CD integrations are done on [Microsoft Azure DevOps](https://azure.microsoft.com/en-us/services/devops/) and is currently in the process of being tweaked.

All code quality analysis are done by [SonarCloud](https://sonarcloud.io/) and [CodeCov.io](https://codecov.io/).

![License](https://img.shields.io/github/license/codebeltnet/classlib-savvyio) [![Build Status](https://dev.azure.com/codebelt/savvyio/_apis/build/status/codebeltnet.classlib-savvyio?branchName=development)](https://dev.azure.com/codebelt/savvyio/_build/latest?definitionId=2&branchName=development) [![codecov](https://codecov.io/gh/codebeltnet/classlib-savvyio/branch/development/graph/badge.svg)](https://codecov.io/gh/codebeltnet/classlib-savvyio) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=coverage)](https://sonarcloud.io/dashboard?id=savvyio) [![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.0-4baaaa.svg)](.github/CODE_OF_CONDUCT.md)

## Development Branch

The `development` branch contains the latest (and greatest) version of the code.

To consume a CI build, create a `NuGet.Config` in your root solution directory and add following content:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <!-- Savvy I/O CI build feed -->
    <add key="codebelt" value="https://nuget.codebelt.net/v3/index.json" />
    <!-- Defaul nuget feed -->
    <add key="nuget" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
```
Do note, that builds from development are preview builds and not to be considered stable.

Once tested thoroughly and feature milestone has been reached, the code will be pushed and merged to a new branch; `release`.

## Release Branch

The `release` branch contains the next version of Savvy I/O. Here it will be tested again while the next semantic version is being determined.

All CI builds are pushed to NuGet.org as either `alpha`, `beta` or `rc` releases (when deemed fit for purpose). For more information, check out [Package versioning - Pre-release Versions](https://docs.microsoft.com/en-us/nuget/concepts/package-versioning#pre-release-versions) at Microsoft.

Lastly, when things are looking all fine and dandy, the code will be pushed and merged to the `main` branch.

## Main Branch

The `main` branch always contains the current `production` ready version of Savvy I/O.

Builds performed from this repository are pushed to NuGet.org as the actual version they represent.

### Code Quality Monitoring

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=alert_status)](https://sonarcloud.io/dashboard?id=savvyio) [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=savvyio) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=savvyio) [![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=security_rating)](https://sonarcloud.io/dashboard?id=savvyio)

[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=ncloc)](https://sonarcloud.io/dashboard?id=savvyio) [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=code_smells)](https://sonarcloud.io/dashboard?id=savvyio) [![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=sqale_index)](https://sonarcloud.io/dashboard?id=savvyio) [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=bugs)](https://sonarcloud.io/dashboard?id=savvyio) [![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=savvyio) [![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=duplicated_lines_density)](https://sonarcloud.io/dashboard?id=savvyio)

# Contributing to Savvy I/O

A big welcome and thank you for considering contributing to Savvy I/O open source project!

Please read more about [contributing to Savvy I/O](.github/CONTRIBUTING.md).

# Code of Conduct

Project maintainers pledge to foster an open and welcoming environment, and ask contributors to do the same.

For more information see our [code of conduct](.github/CODE_OF_CONDUCT.md).

## Links to NuGet packages

NuGet links to all projects of Savvy I/O:

* [Savvyio.App](https://www.nuget.org/packages/Savvyio.App/) *
* [Savvyio.Commands](https://www.nuget.org/packages/Savvyio.Commands/)
* [Savvyio.Core](https://www.nuget.org/packages/Savvyio.Core/)
* [Savvyio.Domain](https://www.nuget.org/packages/Savvyio.Domain/)
* [Savvyio.Domain.EventSourcing](https://www.nuget.org/packages/Savvyio.Domain.EventSourcing/)
* [Savvyio.EventDriven](https://www.nuget.org/packages/Savvyio.EventDriven/)
* [Savvyio.Extensions.Dapper](https://www.nuget.org/packages/Savvyio.Extensions.Dapper/)
* [Savvyio.Extensions.DapperExtensions](https://www.nuget.org/packages/Savvyio.Extensions.DapperExtensions/)
* [Savvyio.Extensions.DependencyInjection](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection/)
* [Savvyio.Extensions.DependencyInjection.Dapper](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.Dapper/)
* [Savvyio.Extensions.DependencyInjection.DapperExtensions](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.DapperExtensions/)
* [Savvyio.Extensions.DependencyInjection.Domain](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.Domain/)
* [Savvyio.Extensions.DependencyInjection.EFCore](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.EFCore/)
* [Savvyio.Extensions.DependencyInjection.EFCore.Domain](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.EFCore.Domain/)
* [Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing/)
* [Savvyio.Extensions.Dispatchers](https://www.nuget.org/packages/Savvyio.Extensions.Dispatchers/)
* [Savvyio.Extensions.EFCore](https://www.nuget.org/packages/Savvyio.Extensions.EFCore/)
* [Savvyio.Extensions.EFCore.Domain](https://www.nuget.org/packages/Savvyio.Extensions.EFCore.Domain/)
* [Savvyio.Extensions.EFCore.Domain.EventSourcing](https://www.nuget.org/packages/Savvyio.Extensions.EFCore.Domain.EventSourcing/)
* [Savvyio.Queries](https://www.nuget.org/packages/Savvyio.Queries/)

*) Provides a convenient set of default API additions for building various types of .NET projects.