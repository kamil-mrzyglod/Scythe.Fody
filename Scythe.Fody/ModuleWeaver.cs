﻿namespace Scythe.Fody
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

        public Action<string> LogWarning { get; set; }

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
            LogWarning = s => { };
        }

        public void Execute()
        {
            foreach (var method in from type in ModuleDefinition.Types from method in type.Methods select method)
            {
                Toggler.Toggle<MethodInstructions>(method, Config);
                Toggler.Toggle<ParametersCount>(method, Config);
                Toggler.Toggle<CyclomaticComplexity>(method, Config);
                Toggler.Toggle<Boxing>(method, Config);
            }
            
            foreach (var error in Errors.Where(_ => _.Severity == Severity.Error))
            {
                LogError(error.ToString());
            }

            foreach (var error in Errors.Where(_ => _.Severity == Severity.Warning))
            {
                LogWarning(error.ToString());
            }
        }
    }
}
