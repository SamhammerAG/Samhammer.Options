using System;

namespace Samhammer.Options.Abstractions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class OptionAttribute : Attribute
    {
        public string SectionName { get; set; }

        public string IocName { get; set; }

        public bool FromRootSection { get; set; }

        public OptionAttribute()
        {
        }

        public OptionAttribute(string sectionName)
        {
            SectionName = sectionName;
        }

        public OptionAttribute(bool fromRootSection)
        {
            FromRootSection = fromRootSection;
        }
    }
}
