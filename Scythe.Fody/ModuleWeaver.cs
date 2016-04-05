using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Mono.Cecil;
using Scythe.Fody.Scythes;

namespace Scythe.Fody
{
    public class ModuleWeaver
    {
        public ModuleDefinition ModuleDefinition { get; set; }

        public XElement Config { get; set; }

        public Action<string> LogError { get; set; }

        /// <summary>
        /// Contains all error generated from scythes
        /// </summary>
        public IEnumerable<ErrorMessage> Errors { get; set; } 

        public ModuleWeaver()
        {
            LogError = s => { };
        }

        public void Execute()
        {
            Errors = new MethodInstructions().Check(ModuleDefinition, Config);
            foreach (var error in Errors)
            {
                LogError(error.ToString());
            }
        }
    }
}
