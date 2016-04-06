namespace Scythe.Fody.Test
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    using Mono.Cecil;

    using NUnit.Framework;

    public class ParametersCountTest
    {
        private const string AssemblyPath = @"../../../../Scythe.TestParametersCount/bin/Debug/Scythe.TestParametersCount.dll";

        private string _weavedAssemblyName;

        private ModuleWeaver _weaver;

        [SetUp]
        public void SetUp()
        {
            _weavedAssemblyName = AssemblyDirectory + $"Scythe.TestParametersCount{DateTime.Now.Ticks}.dll";

            var md = ModuleDefinition.ReadModule(Path.GetFullPath(AssemblyDirectory + AssemblyPath));
            var xe = new XElement(
                "Scythe",
                new XElement(
                    "ParametersCount",
                    new XAttribute("Parameters", 3),
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
        public void GivenInvalidParametersCount_ShouldMarkItAsError()
        {
            Assert.That(ModuleWeaver.Errors, Is.Not.Empty);
            Assert.That(ModuleWeaver.Errors.All(_ => _.ErrorType == ErrorType.ParametersCount), Is.True);
            Assert.That(ModuleWeaver.Errors.Count() == 1, Is.True);
        }
    }
}