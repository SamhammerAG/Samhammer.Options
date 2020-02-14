# Samhammer.Options

## Usage

#### How to add this to your project:
- reference this package to your project: https://www.nuget.org/packages/Samhammer.Options/
- add own Options -> the name has to be the same as in the appsettings
- set [Options] attribute in your Options

```csharp
[Options]
public class ExampleOptions{}
```
- add an OptionsBuilder and call ResolveOptions

```csharp
public class OptionsBuilder
    {
        public static void Resolve(WebHostBuilderContext builder, IServiceCollection services)
        {
            services.ResolveOptions();
            services.PostConfigure<ExamplebOptions>(options => PostConfigureMongo(options, builder.HostingEnvironment));
        }
        
        private static void PostConfigureMongo(MongoDbOptions options, IHostingEnvironment environment)
        {
            options.Example = options.Example.Replace("{environment}", environment.EnvironmentName.ToLower());
        }
}
```

- your options must have the same name as it's called in appsettings

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
