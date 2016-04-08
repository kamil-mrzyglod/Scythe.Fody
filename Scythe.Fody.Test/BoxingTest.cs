using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Mono.Cecil;
using NUnit.Framework;

namespace Scythe.Fody.Test
{
    public class BoxingTest
    {
        private const string AssemblyPath = @"../../../../Scythe.TestBoxing/bin/Debug/Scythe.TestBoxing.dll";

        private string _weavedAssemblyName;

        private ModuleWeaver _weaver;

        [SetUp]
        public void SetUp()
        {
            _weavedAssemblyName = AssemblyDirectory + $"Scythe.TestBoxing{DateTime.Now.Ticks}.dll";

            var md = ModuleDefinition.ReadModule(Path.GetFullPath(AssemblyDirectory + AssemblyPath));
            var xe = new XElement(
                "Scythe",
                new XElement(
                    "Boxing",
                    new XAttribute("Severity", "Warning")));

            _weaver = new ModuleWeaver { ModuleDefinition = md, Config = xe };

            _weaver.Execute();
            md.Write(_weavedAssemblyName);
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
        public void GivenInvalidBoxing_ShouldMarkItAsError()
        {
            Assert.That(ModuleWeaver.Errors, Is.Not.Empty);
            Assert.That(ModuleWeaver.Errors.All(_ => _.ErrorType == ErrorType.Boxing), Is.True);
            Assert.That(ModuleWeaver.Errors.All(_ => _.Severity == Severity.Warning), Is.True);
            Assert.That(ModuleWeaver.Errors.Count() == 1, Is.True);
        }
    }
}