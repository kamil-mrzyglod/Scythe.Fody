namespace Scythe.Fody.Scythes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Mono.Cecil;

    public class ParametersCount : IScythe
    {
        /// <summary>
        /// Checks whether given method parameters count
        /// is less than the defined threshold
        /// </summary>
        public IEnumerable<ErrorMessage> Check(MethodDefinition definition, XElement config)
        {
            var element = config.Elements("ParametersCount").First();
            var parameters = element.Attribute("Parameters").Value;
            var severity = element.Attribute("Severity").Value;

            if (definition.Parameters.Count > int.Parse(parameters))
                yield return
                    new ErrorMessage(
                        $"Method {definition.FullName} contains {definition.Parameters.Count} parameters while max is {parameters}",
                        ErrorType.ParametersCount,
                        severity);
        }
    }
}