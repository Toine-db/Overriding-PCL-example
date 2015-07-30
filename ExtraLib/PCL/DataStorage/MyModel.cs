using System;

namespace DataStorage
{
    public class MyModel
    {
        public string MyOrigin { get; set; }
        public string ParentName { get; set; }
        public string AssemblyName { get; set; }

        // This will compile but can't be used in runtime
        //public string PclOnlyProperty { get; set; }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}