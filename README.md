# Samhammer.Options

## Usage

#### How to add this to your project:
- reference this package to your project: https://www.nuget.org/packages/Samhammer.Options/
- call ResolveOptions on the OptionBuilder

```csharp
services.ResolveOptions();
```

#### How to register a class as service
- add Inject attribute to the class

## Contribute

#### How to publish package
- set package version in Samhammer.Options.csproj
- add information to changelog
- create git tag
- dotnet pack -c Release
- nuget push .\bin\Release\Samhammer.Options.*.nupkg NUGET_API_KEY -src https://api.nuget.org/v3/index.json
- (optional) nuget setapikey NUGET_API_KEY -source https://api.nuget.org/v3/index.json
