using System;

namespace DataStorage
{
    public class MyModel
    {
        public string MyOrigin { get; set; }
        public string ParentName { get; set; }
        public string AssemblyName { get; set; }

        public string DroidOnlyProperty { get; set; }

        public override string ToString()
        {
            return string.Format("I'm comming from a '{0}' library, {3}and I'm initiated by '{1}' {3}{3}from assembly : {2}", MyOrigin, ParentName, AssemblyName, Environment.NewLine);
        }
    }
}