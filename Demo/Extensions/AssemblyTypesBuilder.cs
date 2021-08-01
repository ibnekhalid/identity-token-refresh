using Persistent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Demo.Extensions
{
    public class AssemblyTypesBuilder
    {
        public static Type[] GetAllExecutingContextTypes()
        {
            
            var types = new List<Type>();

            var allAsm = new List<Assembly>();
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            foreach (string dll in Directory.GetFiles(path, "*.dll"))
            {
                allAsm.Add(AssemblyLoadContext.Default.LoadFromAssemblyPath(dll));
            }
            allAsm.Add(typeof(BaseContext).Assembly);

            types.AddRange(GetCommonExecutingContextTypes(allAsm.ToArray()));
            return types.ToArray();
        }

        public static Type[] GetCommonExecutingContextTypes(System.Reflection.Assembly[] assemblies) =>
            assemblies
                .Where(x =>
                     //x?.ManifestModule.Name! == "Common.dll" ||  // if you have common dll
                     x?.ManifestModule.Name! == "Demo.dll" ||
                    x?.ManifestModule.Name! == "Application.dll" ||
                    x?.ManifestModule.Name! == "Core.dll" ||
                    x?.ManifestModule.Name! == "Persistent.dll")
                .SelectMany(x => x?.GetTypes())
                .ToArray();
    }
}
