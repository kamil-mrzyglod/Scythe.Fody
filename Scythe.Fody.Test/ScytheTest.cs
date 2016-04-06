using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Mono.Cecil;
using NUnit.Framework;

namespace Scythe.Fody.Test
{
    public class ScytheTest
    {
        private const string AssemblyPath = @"../../../../Scythe.TestAssembly/bin/Debug/Scythe.TestAssembly.dll";

        private string _weavedAssemblyName;

        private Assembly _assembly;
        private ModuleWeaver _weaver;

        [SetUp]
        public void SetUp()
        {
            _weavedAssemblyName = AssemblyDirectory + $"Scythe.TestAssembly{DateTime.Now.Ticks}.dll";

            var md = ModuleDefinition.ReadModule(Path.GetFullPath(AssemblyDirectory + AssemblyPath));
            var xe = new XElement("Scythe", new XAttribute("Instructions", 1), new XAttribute("Parameters", 1));

            _weaver = new ModuleWeaver { ModuleDefinition = md, Config = xe };

            _weaver.Execute();
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

        [Test]
        public void GivenInvalidInstructionsCount_ShouldMarkItAsError()
        {
            Assert.That(ModuleWeaver.Errors, Is.Not.Empty);
            Assert.That(ModuleWeaver.Errors.Any(_ => _.ErrorType == ErrorType.MethodInstruction), Is.True);
        }

        [Test]
        public void GivenInvalidParametersCount_ShouldMarkItAsError()
        {
            Assert.That(ModuleWeaver.Errors, Is.Not.Empty);
            Assert.That(ModuleWeaver.Errors.Any(_ => _.ErrorType == ErrorType.ParametersCount), Is.True);
        }
    }
}