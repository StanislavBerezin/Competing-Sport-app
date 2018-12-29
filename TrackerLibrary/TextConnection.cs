using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    //Interface is like a contract, forces to make a method from its interface declaration
    public class TextConnection : IDataConnection
    {
        //basically its a public method of type PrizeModel who should return the model (object)
        public PrizeModel CreatePrize(PrizeModel model)
        {
            throw new NotImplementedException();
        }
    }
}
