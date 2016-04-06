namespace Scythe.Fody.Scythes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Mono.Cecil;

    public class MethodInstructions : IScythe
    {
        /// <summary>
        /// Checks whether any method defined in assembly's types
        /// exceeds maximum instructions count, specified in the
        /// config. Note that checking for instructions count
        /// still better shows method's size than counting its lines
        /// - mostly because it checks for complexity rather than
        /// in how many lines a developer developed a functionality
        /// </summary>
        public IEnumerable<ErrorMessage> Check(MethodDefinition definition, XElement config)
        {
            var element = config.Elements("MethodInstructions").FirstOrDefault();
            if(element == null)
                yield break;

            var instructions = element.Attribute("Instructions").Value;
            var severity = element.Attribute("Severity").Value;

            if (definition.Body.Instructions.Count > int.Parse(instructions))

                yield return
                    new ErrorMessage(
                        $"Method {definition.FullName} contains {definition.Body.Instructions.Count} instructions while max is {instructions}",
                        ErrorType.MethodInstruction,
                        severity);
        }
    }
}