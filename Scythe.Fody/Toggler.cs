namespace Scythe.Fody
{
    using System.Linq;
    using System.Xml.Linq;

    using Mono.Cecil;

    public class Toggler
    {
        public static void Toggle<T>(MethodDefinition definition, XElement value) where T : IScythe, new()
        {
            var errors = new T().Check(definition, value);
            ModuleWeaver.Errors = ModuleWeaver.Errors.Concat(errors);
        } 
    }
}