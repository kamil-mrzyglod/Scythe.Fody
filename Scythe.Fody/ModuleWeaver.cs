namespace Scythe.Fody
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Mono.Cecil;

    using Scythe.Fody.Scythes;

    public class ModuleWeaver
    {
        public ModuleDefinition ModuleDefinition { get; set; }

        public XElement Config { get; set; }

        public Action<string> LogError { get; set; }

        /// <summary>
        /// Contains all error generated from scythes
        /// </summary>
        public static IEnumerable<ErrorMessage> Errors { get; set; }

        static ModuleWeaver()
        {
            Errors = Enumerable.Empty<ErrorMessage>();
        }

        public ModuleWeaver()
        {
            LogError = s => { };
        }

        public void Execute()
        {
            Toggler.Toggle<MethodInstructions>(ModuleDefinition, Config);

            foreach (var error in Errors)
            {
                LogError(error.ToString());
            }
        }
    }
}
