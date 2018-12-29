using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAcess;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    //Interface is like a contract, forces to make a method from its interface declaration

    public class SqlConnector : IDataConnection
    {
        //basically its a public method of type PrizeModel who should return the model (object)
        public PrizeModel CreatePrize(PrizeModel model)
        {
            //THe reason for USING is because it opens the connection and then properly destroys it without memmory leaks.
            //utilsiing CnnString from global config with the required Database name
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournaments")))
            {
                //By using dapper we make a temprory object
                //then we add values like '@PlaceNumber" is related to SQL database and we insert it from the model parameter
                //recieved by this function. Hence, we add all of the required values, and then we execute the insertion to DB
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                //Using procedure to insert it, to avoid SQL injections, we specify the commandType to be a procedure
                //defined in sql
                connection.Execute("dbo.SpPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");

                return model;
            }
        }
    }
}
