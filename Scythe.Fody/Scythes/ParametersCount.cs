namespace Scythe.Fody.Scythes
{
    using System.Collections.Generic;
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
            var parameters = config.Attribute("Parameters").Value;
            if (definition.Parameters.Count > int.Parse(parameters))
                yield return
                    new ErrorMessage(
                        $"Method {definition.FullName} contains {definition.Parameters.Count} parameters while max is {parameters}",
                        ErrorType.ParametersCount);
        }
    }
}