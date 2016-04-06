namespace Scythe.Fody.Test
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    using Mono.Cecil;

    using NUnit.Framework;

    public class CyclomaticComplexityTest
    {
        private const string AssemblyPath = @"../../../../Scythe.CyclomaticComplexityTest/bin/Debug/Scythe.CyclomaticComplexityTest.dll";

        private string _weavedAssemblyName;

        private ModuleWeaver _weaver;

        [SetUp]
        public void SetUp()
        {
            _weavedAssemblyName = AssemblyDirectory + $"Scythe.CyclomaticComplexityTest{DateTime.Now.Ticks}.dll";

            var md = ModuleDefinition.ReadModule(Path.GetFullPath(AssemblyDirectory + AssemblyPath));
            var xe = new XElement(
                "Scythe",
                new XElement(
                    "CyclomaticComplexity",
                    new XAttribute("CyclomaticComplexity", 5),
                    new XAttribute("Severity", "Error")));

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
        public void GivenInvalidCyclomaticComplexity_ShouldMarkItAsError()
        {
            Assert.That(ModuleWeaver.Errors, Is.Not.Empty);
            Assert.That(ModuleWeaver.Errors.All(_ => _.ErrorType == ErrorType.CyclomaticComplexity), Is.True);
            Assert.That(ModuleWeaver.Errors.Count() == 1, Is.True);
        }
    }
}