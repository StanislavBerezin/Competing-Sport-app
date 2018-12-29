using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        /// <summary>
        /// Creates a list of connections from interface
        /// only those inside of config class can change the values of connections
        /// because its a type of IData, all connections added to that list have methods of that interface
        /// sqlConnector and text.
        /// 
        /// LIST:  public static List<IDataConnection> Connections { get; private set; }
        /// 
        /// </summary>
        public static IDataConnection Connection { get; private set; }
        
        public static void InitializeConnection(bool database, bool textFiles)
        {
            if (database)
            {

            }
            if (textFiles)
            {

            }
        }
    }
}
