using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Samhammer.Options.Abstractions;
using Xunit;

namespace Samhammer.Options.Test
{
    public class ConfigurationExtensionsTest
    {
        private readonly ConfigurationBuilder configurationBuilder;

        private readonly List<KeyValuePair<string, string>> configurationValues;

        private IConfiguration Configuration => configurationBuilder.Build();

        public ConfigurationExtensionsTest()
        {
            configurationBuilder = new ConfigurationBuilder();
            configurationValues = new List<KeyValuePair<string, string>>();
            configurationBuilder.AddInMemoryCollection(configurationValues);
        }

        [Fact]
        public void GetOptions_FromConfiguration_WithAttribute()
        {
            // arrange
            configurationValues.Add(new KeyValuePair<string, string>("TestOptions:Url", "http://localhost"));

            // act
            var options = Configuration.GetOptions<TestOptionsWithAttribute>();

            // assert
            var expectedOptions = new TestOptionsWithAttribute { Url = "http://localhost" };
            options.Should().BeEquivalentTo(expectedOptions);
        }

        [Fact]
        public void GetOptions_FromConfiguration_WithRootAttribute()
        {
            // arrange
            configurationValues.Add(new KeyValuePair<string, string>("Url", "http://localhost"));

            // act
            var options = Configuration.GetOptions<TestOptionsWithRootAttribute>();

            // assert
            var expectedOptions = new TestOptionsWithRootAttribute { Url = "http://localhost" };
            options.Should().BeEquivalentTo(expectedOptions);
        }

        [Fact]
        public void GetOption_FromConfiguration_WithoutAttribute()
        {
            // arrange
            configurationValues.Add(new KeyValuePair<string, string>("TestOptions:Url", "http://localhost"));

            // act
            var options = Configuration.GetOptions<TestOptions>();

            // assert
            var expectedOptions = new TestOptions { Url = "http://localhost" };
            options.Should().BeEquivalentTo(expectedOptions);
        }

        [Option(sectionName: "TestOptions")]
        private class TestOptionsWithAttribute
        {
            public string Url { get; set; }
        }

        [Option(FromRootSection = true)]
        private class TestOptionsWithRootAttribute
        {
            public string Url { get; set; }
        }

        private class TestOptions
        {
            public string Url { get; set; }
        }
    }
}
