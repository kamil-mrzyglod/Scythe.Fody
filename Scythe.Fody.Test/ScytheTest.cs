using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;

namespace Scythe.Fody.Test
{
    public class ScytheTest
    {
        private const string AssemblyPath = @"../../../../Scythe.TestAssembly/bin/Debug/Scythe.TestAssembly.dll";

        private string _weavedAssemblyName;

        private Assembly _assembly;

        [SetUp]
        public void SetUp()
        {
            _weavedAssemblyName = AssemblyDirectory + $"Scythe.TestAssembly{DateTime.Now.Ticks}.dll";

            var md = ModuleDefinition.ReadModule(Path.GetFullPath(AssemblyDirectory + AssemblyPath));
            var weaver = new ModuleWeaver { ModuleDefinition = md };

            weaver.Execute();
            md.Write(_weavedAssemblyName);

            _assembly = Assembly.LoadFile(_weavedAssemblyName);
        }

        private static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}