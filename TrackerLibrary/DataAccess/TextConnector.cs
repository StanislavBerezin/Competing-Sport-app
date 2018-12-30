using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAcess;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;
namespace TrackerLibrary.DataAccess
{
    //Interface is like a contract, forces to make a method from its interface declaration
    //in which case is the PrizeModel

    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "PrizeModels.csv";
        private const string PeopleFile = "PersonModels.csv";

        //basically its a public method of type PrizeModel who should return the model (object)
        /// <summary>
        /// THis Method writes to text file, most of the functionality is ddefined in TextConnectorProcessor
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            // Load the text file
            // Convert the text to List<PrizeModels>, its all possible due to this word in method
            //definition
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            // Find the max ID
            int currentId = 1;

            if (prizes.Count > 0)
            {
                //order the list by ID
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }

            //updating the ID by descending
            model.Id = currentId;

            // Add the new record with the new ID(max + 1)
            prizes.Add(model);


            //writes everything
            prizes.SaveToPrizeFile(PrizesFile);

            return model;
        }


        public PersonModel CreatePerson(PersonModel model)
        {
            List<PersonModel> people = PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            int currentId = 1;

            if (people.Count > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;
            }
            model.Id = currentId;

            people.Add(model);

            people.SaveToPeopleFile(PeopleFile);

            return model;
        }

    }
}
