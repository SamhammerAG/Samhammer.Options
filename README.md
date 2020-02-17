[![Build Status](https://travis-ci.com/SamhammerAG/Samhammer.Options.svg?branch=master)](https://travis-ci.com/SamhammerAG/Samhammer.Options)
 
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
    public string MyKey { get; set; }
}
```

appsettings.json
```json
"ExampleOptions": {
  "MyKey": "1234"
}
```

### How to register options loaded from other section ##
It might be that the section name is not the same as your class name.

```csharp
[Options("Test")]
public class ExampleOptions{
    public string ApiKey { get; set; }
    public string ApiUrl { get; set; }
}
```

appsettings.json
```json
"Test": {
  "ApiKey": "1234",
  "ApiUrl": "http://api.my.de"
}
```

### How to register options loaded from root ##
Some options may be defined in the root of appsettings and not inside a section.

```csharp
[Options(true)]
public class RootOptions {
    public string RootUrl { get; set; }
}
```

appsettings.json
```json
"RootUrl": "http://api.my.de"
```

### How to register named options ##
It is possible to load multiple options of the same type, but with different name.

```csharp
public IOptionsSnapshot<ApiOptions> ApiOptions { get; set; }
    
var apiOptions1 = ApiOptions.Get("api1");
var apiOptions2 = ApiOptions.Get("api2");
```

```csharp
[Option("api1", IocName = "api1")]
[Option("api2", IocName = "api2")]
public class ApiOptions
{
    public string ApiKey { get; set; }
}
```
appsettings.json
```json
"api1": {
  "ApiKey": "1234",
  "ApiUrl": "http://api.my.de"
}

"api2": {
  "ApiKey": "6789",
  "ApiUrl": "http://api2.my.de"
}
```

## Contribute

#### How to publish package
- set package version in Samhammer.Options.csproj
- add information to changelog
- create git tag
- dotnet pack -c Release
- nuget push .\bin\Release\Samhammer.Options.*.nupkg NUGET_API_KEY -src https://api.nuget.org/v3/index.json
- (optional) nuget setapikey NUGET_API_KEY -source https://api.nuget.org/v3/index.json
