using System.Collections.Generic;
using System.Xml.Linq;
using Mono.Cecil;

namespace Scythe.Fody
{
    public interface IScythe
    {
        IEnumerable<ErrorMessage> Check(MethodDefinition definition, XElement config);
    }
}