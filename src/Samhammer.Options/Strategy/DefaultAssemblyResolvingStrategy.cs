using System.Collections.Generic;
using System.Reflection;
using Samhammer.Options.Utils;

namespace Samhammer.Options.Strategy
{
    public class DefaultAssemblyResolvingStrategy : IAssemblyResolvingStrategy
    {
        public IEnumerable<Assembly> ResolveAssemblies()
        {
            return AssemblyUtils.LoadAllAssembliesOfProject();
        }
    }

    public interface IAssemblyResolvingStrategy
    {
        IEnumerable<Assembly> ResolveAssemblies();
    }
}
