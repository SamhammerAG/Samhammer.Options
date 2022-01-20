# Samhammer.Options

## Usage

#### How to add this to your project:
- reference this package to your project: https://www.nuget.org/packages/Samhammer.Options/
- call ResolveOptions on the IServiceCollection with IConfiguration container

```csharp
services.ResolveOptions(Configuration);
```

### How to register options ##
- add [Option] attribute to your Options class
- by default options are loaded from appsettings section with same name as the class

```csharp
[Option]
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
[Option("Test")]
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
[Option(true)]
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

## Configuration
Starting with version 3.1.5 all customizations needs to be done with the options action.

The registrations to servicecollection will no longer be used cause we dont want to use ioc to setup ioc.
@see also https://docs.microsoft.com/de-de/dotnet/core/compatibility/2.2-3.1#hosting-generic-host-restricts-startup-constructor-injection

#### How to enable logging?
By default the project will not do any logging, but you can activate it.
This will require that you provide an ILoggerFactory from Microsoft.Extensions.Logging.

###### Sample with microsoft console logger.
```csharp
var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug));
services.ResolveOptions(Configuration, options => options.SetLogging(loggerFactory));
```

###### Sample with serilog logger. (you need to setup serilog before)
```csharp
services.ResolveOptions(Configuration, options => options.SetLogging(new SerilogLoggerFactory()));
```

#### How to change assemly resolving strategy?
By default the project will only resolve types of project assemblies, but not on packages or binaries.
But you can replace the default strategy with your own implementation.

```csharp
services.ResolveOptions(Configuration, options => options.SetStrategy(new MyAssemblyResolvingStrategy()));
```

## Contribute

#### How to publish package
- Create a tag and let the github action do the publishing for you
