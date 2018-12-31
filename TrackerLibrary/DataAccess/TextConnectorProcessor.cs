using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess.TextHelpers
{
    /// <summary>
    /// ALl of the are static because only textConnector as class will use it and nothing else
    /// </summary>
    public static class TextConnectorProcessor
    {
        /// <summary>
        /// Getting the filePath from App.Config defined in trackerUI, and returns it to whatever calls it
        /// word THIS allows us to use like  PrizesFile.FullFilePath() where it will recieve a parameter from PrizesFile
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FullFilePath(this string fileName) // PrizeModels.csz
        {
            // C:\data\TournamentTracker\PrizeModels.csv, note 2 \\
            return $"{ConfigurationManager.AppSettings["filePath"]}\\{fileName}";
        }

        //usage of this allows us to chain the events like  PrizesFile.FullFilePath().LoadFile(), so its parameter
        //is recievied by the first 2
        public static List<string> LoadFile(this string file) // Load the text file
        {
            if (!File.Exists(file))
            {

                return new List<string>();
            }

            //if exists read all files and get it toList and return to whatever calls this function
            return File.ReadAllLines(file).ToList();
        }


        /// <summary>
        /// list of type PrizeModel, accepts List of strings as a parameter
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        /// this is used to chain them
        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            // List that we plan to return when we finished
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {
                //each collumn we put in an array so that we can reference later,
                //like first column would be [0], and so on. (seperated by ',')
                string[] cols = line.Split(',');

                PrizeModel p = new PrizeModel();
                //making all of the strings into Int and other types we require, int.Parse(will switch "1" to 1)
                p.Id = int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]);
                p.PlaceName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);
                //eventually we go one by one and near the end we add the finalise object to output list we are going
                //to return
                output.Add(p);
            }

            return output;
        }

        //quite similar logic to above
        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                PersonModel p = new PersonModel();
                p.Id = int.Parse(cols[0]);
                p.FirstName = cols[1];
                p.LastName = cols[2];
                p.EmailAddress = cols[3];
                p.CellphoneNumber = cols[4];
                output.Add(p);
            }

            return output;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="peopleFileName"> This parameter is the filename where team members are</param>
        /// <returns></returns>
        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string peopleFileName)
        {
            List<TeamModel> Output = new List<TeamModel>();
            List<PersonModel> people = peopleFileName.FullFilePath().LoadFile().ConvertToPersonModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TeamModel t = new TeamModel();

                t.Id = int.Parse(cols[0]);
                t.TeamName = cols[1];

                //considering we know where players are located so we r gonna split
                string[] personIds = cols[2].Split('|');

                //Take the list of people in the text file and search for it and filter WHERE the id of the person
                //in the list equals the id from the collection of person in the "TeamMemers" List, binded with the team
                foreach (string id in personIds)
                {
                    t.TeamMembers.Add(people.Where(x => x.Id == int.Parse(id)).First());
                }

                Output.Add(t);
            }
            return Output;
        }


        public static List<TournamentModel> ConvertToTournamentModels(
           this List<string> lines,
           string TeamFileName,
           string peopleFileName,
           string prizeFileName)
        {
            // id = 0
            // TournamentName = 1
            // EntryFee = 2
            // EnteredTeams = 3
            // Prizes = 4
            // Rounds = 5

            //The file will have this layout:
            //id,TournamentName,EntryFee,(id|id|id - Entered Teams),(id|id|id - Prizes),(Rounds - id^id^id|id^id^id|id^id^id)
            List<TournamentModel> output = new List<TournamentModel>();
            //here we are getting lists of other models to make this one
            List<TeamModel> teams = TeamFileName.FullFilePath().LoadFile().ConvertToTeamModels(peopleFileName);
            List<PrizeModel> prizes = prizeFileName.FullFilePath().LoadFile().ConvertToPrizeModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TournamentModel tm = new TournamentModel();
                tm.Id = int.Parse(cols[0]);
                tm.TournamentName = cols[1];
                tm.EntryFee = decimal.Parse(cols[2]);

                if (cols[3].Length > 0)
                {
                    string[] teamIds = cols[3].Split('|');
                    foreach (string id in teamIds)
                    {
                        tm.EnteredTeams.Add(teams.Where(x => x.Id == int.Parse(id)).First());

                    }
                }

                if (cols[4].Length > 0)
                {
                    string[] prizeIds = cols[4].Split('|');
                    foreach (string id in prizeIds)
                    {
                        tm.Prizes.Add(prizes.Where(x => x.Id == int.Parse(id)).First());
                    }
                }

                string[] rounds = cols[5].Split('|');

                foreach (string round in rounds)
                {
                    string[] msText = round.Split('^');
                    List<MatchupModel> ms = new List<MatchupModel>();

                    foreach (string matchupModelTextId in msText)
                    {
                        ms.Add(matchups.Where(x => x.Id == int.Parse(matchupModelTextId)).First());
                    }

                    tm.Rounds.Add(ms);
                }

                // TODO - Capture rounds information

                output.Add(tm);
            }

            return output;
        }


        public static void SaveTournamentFile(this List<TournamentModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (TournamentModel tm in models)
            {
                lines.Add($@"{tm.Id},
                    {tm.TournamentName},
                    {tm.EntryFee},
                    { ConvertTeamListToString(tm.EnteredTeams) },
                    { ConvertPrizeListToString(tm.Prizes)},
                    { ConvertRoundListToString(tm.Rounds) }");

            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

       
        private static string ConvertRoundListToString(List<List<MatchupModel>> rounds)
        {
            //(Rounds - id^id^id|id^id^id|id^id^id)
            string output = "";

            if (rounds.Count == 0)
            {
                return "";
            }

            foreach (List<MatchupModel> r in rounds)
            {
                output += $"{ ConvertMatchupListToString(r) }|";
            }

            //Removes the pipe character to the end of the list of people's id
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertMatchupListToString(List<MatchupModel> matchups)
        {
            string output = "";

            if (matchups.Count == 0)
            {
                return "";
            }

            foreach (MatchupModel m in matchups)
            {
                output += $"{ m.Id }^";
            }

            //Removes the pipe character to the end of the list of people's id
            output = output.Substring(0, output.Length - 1);

            return output;
        }
        private static string ConvertPeopleListToString(List<PersonModel> People)
        {
            string output = "";

            if (People.Count == 0)
            {
                return "";
            }

            foreach (PersonModel p in People)
            {
                output += $"{ p.Id }|";
            }

            //Removes the pipe character to the end of the list of people's id
            output = output.Substring(0, output.Length - 1);

            return output;
        }
        private static string ConvertTeamListToString(List<TeamModel> Teams)
        {
            string output = "";

            if (Teams.Count == 0)
            {
                return "";
            }

            foreach (TeamModel t in Teams)
            {
                output += $"{ t.Id }|";
            }

            //Removes the pipe character to the end of the list of people's id
            output = output.Substring(0, output.Length - 1);

            return output;
        }
        private static string ConvertPrizeListToString(List<PrizeModel> Prizes)
        {
            string output = "";

            if (Prizes.Count == 0)
            {
                return "";
            }

            foreach (PrizeModel t in Prizes)
            {
                output += $"{ t.Id }|";
            }

            //Removes the pipe character to the end of the list of people's id
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        public static void SaveToTeamFile(this List<TeamModel> models, string FileName)
        {
            List<string> lines = new List<string>();

            foreach (TeamModel t in models)
            {
                lines.Add($"{ t.Id },{ t.TeamName },{ ConvertPeopleListToString(t.TeamMembers) }");
            }

            File.WriteAllLines(FileName.FullFilePath(), lines);

        }

        public static void SaveToPrizeFile(this List<PrizeModel> models, string fileName)
        {
            //list of strings for lines
            List<string> lines = new List<string>();

            //goes through the objects of PrizeModel recieved in a form of a list
            foreach (PrizeModel p in models)
            {
                //writes everything our list
                lines.Add($"{p.Id},{p.PlaceNumber},{p.PlaceName},{p.PrizeAmount},{p.PrizePercentage}");
            }

            //then it executes and writes it to the file
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        //similar to the one above
        public static void SaveToPeopleFile(this List<PersonModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in models)
            {
                lines.Add($"{p.Id}, {p.FirstName}, {p.LastName}, {p.EmailAddress}, {p.CellphoneNumber}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);

        }

    }
}
