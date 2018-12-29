using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess;
using TrackerLibrary.DataAcess;

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
        /// Eventually removed the list, and just keep one connection 
        /// </summary>
        public static IDataConnection Connection { get; private set; }
        

        //Here we make the first connection
        /// <summary>
        /// THis initiliaser is used in TrackerUI program.cs
        /// based on the selection there, it establishes the connection 
        /// and create an object of this connection
        /// </summary>
        /// <param name="connectionType"></param>
        public static void InitializeConnection(DatabaseType connectionType)
        {
           
            //sql connection
            if (connectionType == DatabaseType.Sql)
            {
                SqlConnector sql = new SqlConnector();
                Connection = sql;
            }
            if (connectionType == DatabaseType.TextFile)
            {
                TextConnector text = new TextConnector();
                Connection = text;

            }
        }

        //to get the location defined in app.config Tracker.UI
        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
