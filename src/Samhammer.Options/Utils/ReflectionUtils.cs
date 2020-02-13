using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Samhammer.Options.Utils
{
    public static class ReflectionUtils
    {
        public static List<Type> FindAllExportedTypesWithAttribute(IEnumerable<Assembly> assemblies, Type attributeType, bool inherit = true)
        {
            var exportedTypes = assemblies
                .SelectMany(a => a.ExportedTypes)
                .Where(t => t.GetTypeInfo().IsDefined(attributeType, inherit))
                .ToList();

            return exportedTypes;
        }

        public static void GetPropertyPath(Type objType, string parentName, List<string> types)
        {
            if (IsChildType(objType))
            {
                parentName = $"{parentName}.{objType.Name}";
                types.Add(parentName);
                return;
            }

            var properties = objType.GetProperties().OrderBy(x => x.Name);

            foreach (var propertyInfo in properties)
            {
                GetPropertyPath(propertyInfo, parentName, types);
            }
        }

        public static IEnumerable<T> GetAttributesOfType<T>(Type objectType, bool inherit = true) where T : Attribute
        {
            return objectType.GetTypeInfo().GetCustomAttributes<T>(inherit);
        }

        private static void GetPropertyPath(PropertyInfo prop, string parentName, List<string> types)
        {
            if (IsChildType(prop.PropertyType))
            {
                parentName = $"{parentName}.{prop.Name}";
                types.Add(parentName);
                return;
            }

            GetPropertyPath(prop.PropertyType, $"{parentName}.{prop.Name}", types);
        }

        private static bool IsChildType(Type objType)
        {
            return objType.IsPrimitive
                   || objType == typeof(string)
                   || (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                   || (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(List<>));
        }
    }
}
