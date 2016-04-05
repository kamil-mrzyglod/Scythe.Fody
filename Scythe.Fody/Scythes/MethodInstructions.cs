using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Mono.Cecil;

namespace Scythe.Fody.Scythes
{
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
        public IEnumerable<ErrorMessage> Check(ModuleDefinition definition, XElement config)
        {
            foreach (var method in from type in definition.GetTypes() from method in type.Methods select method)
            {
                var instructions = config.Attribute("Instructions").Value;
                if (method.Body.Instructions.Count > int.Parse(instructions))
                    yield return
                        new ErrorMessage(
                            $"Method {method.FullName} contains {method.Body.Instructions.Count} instructions, whether max is {instructions}",
                            ErrorType.MethodInstruction);
            }
        }
    }
}