# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [3.2.1] - 2022-02-12
### Changed
- Updgraded .NET dependencies
- Upgraded ChustaSoft internal dependencies

## [3.2.0] - 2021-08-24
### Added
- Added support to .NET 6.0

## [3.1.1] - 2021-08-24
### Fixed
- Fixed wrong precompilation directive for NET 5.0 DBAccess configuration

## [3.1.0] - 2021-07-14
### Fixed
- Fixed multiple first level Include
- Fixed ICollection deep navigations

## [3.0.0] - 2021-07-12
### Added
- Added support to .NET 5.0
- SelectablePropertiesBuilder refactorized, syntax in queries changed
- Documentation improved

## [2.0.2] - 2021-03-29
### Changed
- Allowed Queryables from AsyncRepository

## [2.0.1] - 2020-10-12
### Changed
- GetSingleAsync Behaviour align on IAsyncRepository like in IRepository

## [2.0.0] - 2020-09-08
### Added
- Added new abstractios for Builder filtering Actions, improving queries syntax
- Descending sort added
- IAsyncRepository adds support for IAsyncEnumerable on .NET Core 3.1
- Query property exposed from IReadonlyRepository
- Added TakeFrom method allowing to filter starting from a condition, being discriminator entitie inclusive or not
- Full CRUD implementation for AsyncRepositories

## [1.0.0] - 2020-08-14
### Added
- Provided default interfaces for Guid Keys
- Added configuration for .NET Framework with Unity container
- IUnitOfWork interface provided
- IRepositoryBase interface provided
- IAsyncRepositoryBase interface provided
- IRepositoryBase implementation provided
- IAsyncRepositoryBase implementation provided
- IQueryableExtensions extension methods provided for pagination, included properties, filters and sorting
- Configuration provided for .NET Core projects- 
### Fixed
- Fixed error when casting default repositories (from pre-release versions)
### Removed
- Removed restriction on CommonNET IKeyable interface (from pre-release versions)
 