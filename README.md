# Samhammer.Options

## Usage

#### How to add this to your project:
- reference this package to your project: https://www.nuget.org/packages/Samhammer.Options/
- call ResolveOptions on the IServiceCollection

```csharp
services.ResolveOptions();
```

### How to register options ##
- add [Options] attribute to your Options class
- by default options are loaded from appsettings section with same name as the class

```csharp
[Options]
public class ExampleOptions{
    public string ApiKey { get; set; }
    public string ApiUrl { get; set; }
}
```

appsettings.json
```json
"ExampleOptions": {
  "ApiKey": "1234",
  "ApiUrl": "http://api.my.de"
}
```

### How to register options loaded from other section ##
TODO

### How to register options loaded from root ##
TODO

### How to register named options ##
TODO

## Contribute

#### How to publish package
- set package version in Samhammer.Options.csproj
- add information to changelog
- create git tag
- dotnet pack -c Release
- nuget push .\bin\Release\Samhammer.Options.*.nupkg NUGET_API_KEY -src https://api.nuget.org/v3/index.json
- (optional) nuget setapikey NUGET_API_KEY -source https://api.nuget.org/v3/index.json
