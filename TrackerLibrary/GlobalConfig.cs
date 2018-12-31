using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess;
using System.Configuration;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public const string PrizesFile = "PrizeModels.csv";
        public const string PeopleFile = "PersonModels.csv";
        public const string TeamFile = "TeamModels.csv";
        public const string TournamentFile = "TournamentModels.csv";
        public const string MatchupFile = "MatchupModels.csv";
        public const string MatchupEntryFile = "MatchEntryModels.csv";

        public static IDataConnection Connection { get; set; }

        public static void InitializeConnections(Database db)
        {
            switch (db)
            {
                
                case Database.MySQL:
                    MySqlConnector mysql = new MySqlConnector();
                    Connection = mysql;
                    break;
                case Database.TextFile:
                    TextConnection text = new TextConnection();
                    Connection = text;
                    break;
                default:
                    break;
            }
        }

        public static string ConnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public static string AppKeyLookup(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
