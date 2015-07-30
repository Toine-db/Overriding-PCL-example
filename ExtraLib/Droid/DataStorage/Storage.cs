namespace DataStorage
{
    public class Storage
    {
        public MyModel GetMyData()
        {
            return new MyModel()
            {
                MyOrigin = "Droid", 
                ParentName = GetType().FullName,
                AssemblyName = GetType().AssemblyQualifiedName
            };
        }
    }
}
