namespace Scythe.Fody.Scythes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    public class CyclomaticComplexity : IScythe
    {
        /// <summary>
        /// Checks whether cyclomatic complexity(https://en.wikipedia.org/wiki/Cyclomatic_complexity)
        /// of a method doesn't exceed predefinied threshold. 
        /// Note that current implementation is a bit different
        /// that a mathematical equation - it checks
        /// for a control transfers OpCodes and counts them
        /// to determine complexity of a method.
        /// </summary>
        public IEnumerable<ErrorMessage> Check(MethodDefinition definition, XElement config)
        {
            var element = config.Elements("CyclomaticComplexity").FirstOrDefault();
            if (element == null)
                yield break;

            if (definition.Body.Instructions.All(DoNotTransferControl()))
                yield break;

            var cyclomaticComplexity = element.Attribute("CyclomaticComplexity").Value;
            var severity = element.Attribute("Severity").Value;
            var transfers =
                definition.Body.Instructions.Count(
                    _ =>
                    _.OpCode == OpCodes.Brtrue || _.OpCode == OpCodes.Brtrue_S || _.OpCode == OpCodes.Brfalse
                    || _.OpCode == OpCodes.Brfalse_S);

            // OpCodes.Br means that there is an unconditional
            // control transfer, in most cases it means that
            // it is a return statement
            if (definition.Body.Instructions.Any(_ => _.OpCode == OpCodes.Br || _.OpCode == OpCodes.Br_S)) transfers++;

            if(transfers > int.Parse(cyclomaticComplexity))
                yield return
                    new ErrorMessage(
                        $"Method {definition.FullName} has cyclomatic complexity of {transfers} while max is {cyclomaticComplexity}",
                        ErrorType.CyclomaticComplexity,
                        severity);
        }

        private static Func<Instruction, bool> DoNotTransferControl()
        {
            return _ =>
                   _.OpCode != OpCodes.Brtrue && _.OpCode != OpCodes.Brtrue_S && _.OpCode != OpCodes.Brfalse
                   && _.OpCode != OpCodes.Brfalse_S;
        }
    }
}