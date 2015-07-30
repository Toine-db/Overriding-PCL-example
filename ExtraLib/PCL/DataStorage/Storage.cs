using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStorage
{
    public class Storage
    {

        public MyModel GetMyData()
        {
            // This method NEVER gets hit, so it doesn't mater what you do; like return default(MyModel);
            throw new NotImplementedException();
        }

    }
}
