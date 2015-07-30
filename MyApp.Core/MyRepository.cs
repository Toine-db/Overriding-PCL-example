
using DataStorage;

namespace MyApp.Core
{
    public class MyRepository
    {

        public MyModel GetMyModel()
        {
            var myModel = new Storage().GetMyData();

            /// Missing Method Exception at runtime
            //myModel.PclOnlyProperty = "BOOM"; 

            /// VS/Resharper uses the 'portable' reference, so this won't compile at all
            //myModel.DroidOnlyProperty = "NOBODY SEES ME";

            return myModel;
        }

    }
}
