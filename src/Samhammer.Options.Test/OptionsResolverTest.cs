using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Samhammer.Options.Abstractions;
using Xunit;

namespace Samhammer.Options.Test
{
    public class OptionsResolverTest
    {
        private readonly IServiceCollection services;
        private readonly ConfigurationBuilder configurationBuilder;
        private readonly List<KeyValuePair<string, string>> configurationValues;

        private ServiceProvider ServiceProvider => services.BuildServiceProvider();

        private IConfiguration Configuration => configurationBuilder.Build();

        public OptionsResolverTest()
        {
            var logger = Substitute.For<ILogger<OptionsResolver>>();

            services = new ServiceCollection();
            services.AddSingleton(logger);

            configurationBuilder = new ConfigurationBuilder();
            configurationValues = new List<KeyValuePair<string, string>>();
            configurationBuilder.AddInMemoryCollection(configurationValues);
        }

        [Fact]
        public void ConfigurationFromMemory()
        {
            configurationValues.Add(new KeyValuePair<string, string>("testSection:Password", "test"));

            var section = Configuration.GetSection("testSection");
            var value = section?.GetValue<string>("Password");

            section.Should().NotBeNull();
            value.Should().BeEquivalentTo("test");
        }

        [Fact]
        public void OptionsFromCode()
        {
            services.Configure<TestOptions>(o => o.Password = "test");

            var options = ServiceProvider.GetService<IOptions<TestOptions>>();
            var expected = new TestOptions { Password = "test" };

            options.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void OptionsFromConfiguration()
        {
            configurationValues.Add(new KeyValuePair<string, string>("testSection:Password", "test"));
            
            services.AddSingleton(Configuration);
            services.ResolveOptions();

            var result = ServiceProvider.GetService<IOptions<TestOptions>>();
            var expected = new TestOptions { Password = "test" };

            result.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void OptionsFromConfigurationAndCode()
        {
            configurationValues.Add(new KeyValuePair<string, string>("testSection:Password", "test"));

            services.AddSingleton(Configuration);
            services.ResolveOptions();
            services.PostConfigure<TestOptions>(o => o.Password = o.Password + "123");

            var result = ServiceProvider.GetService<IOptions<TestOptions>>();
            var expected = new TestOptions { Password = "test123" };

            result.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void OptionsFromConfigurationAndCodeNamed()
        {
            // Arrange
            configurationValues.Add(new KeyValuePair<string, string>("testSectionOne:Password", "testOne"));
            configurationValues.Add(new KeyValuePair<string, string>("testSectionTwo:Password", "testTwo"));
            configurationValues.Add(new KeyValuePair<string, string>("testSectionThree:Password", "testThree"));
            configurationValues.Add(new KeyValuePair<string, string>("testSectionFour:Password", "testFour"));

            services.AddSingleton(Configuration);
            services.ResolveOptions();

            // Act
            var result = ServiceProvider.GetService<IOptionsSnapshot<TestOptionsNamed>>();
            var namedOptionOne = result.Get("testSectionOneIocName");
            var namedOptionTwo = result.Get("testSectionTwoIocName");
            var namedOptionDuplicate = result.Get("duplicateName");

            // Assert
            namedOptionOne.Should().BeEquivalentTo(new TestOptionsNamed { Password = "testOne" });
            namedOptionTwo.Should().BeEquivalentTo(new TestOptionsNamed { Password = "testTwo" });
            namedOptionDuplicate.Should().BeEquivalentTo(new TestOptionsNamed { Password = "testFour" });
        }

        [Fact]
        public void OptionsFromRootSection()
        {
            configurationValues.Add(new KeyValuePair<string, string>("MyRootSetting1", "testFromRoot"));

            services.AddSingleton(Configuration);
            services.ResolveOptions();

            var result = ServiceProvider.GetService<IOptions<TestRootOptions>>();
            var expected = new TestRootOptions { MyRootSetting1 = "testFromRoot" };

            result.Value.Should().BeEquivalentTo(expected);
        }

        [Option("testSection")]
        public class TestOptions
        {
            public string Password { get; set; }
        }

        [Option("testSectionOne", IocName = "testSectionOneIocName")]
        [Option("testSectionTwo", IocName = "testSectionTwoIocName")]
        [Option("testSectionThree", IocName = "duplicateName")]
        [Option("testSectionFour", IocName = "duplicateName")]
        public class TestOptionsNamed
        {
            public string Password { get; set; }
        }

        [Option(true)]
        public class TestRootOptions
        {
            public string MyRootSetting1 { get; set; }
        }
    }
}
