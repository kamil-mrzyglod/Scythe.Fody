using System.Xml.Linq;
using Mono.Cecil;

namespace Scythe.Fody
{
    public class ModuleWeaver
    {
        public ModuleDefinition ModuleDefinition { get; set; }

        public XElement Config { get; set; }

        public void Execute()
        {
        }
    }
}
