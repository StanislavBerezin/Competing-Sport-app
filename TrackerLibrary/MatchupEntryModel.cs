using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class MatchupEntryModel
    {
        /// <summary>
        /// Represents a team in a matchup
        /// </summary>
        public TeamModel TeamCompeting { get; set; }
        /// <summary>
        /// The score of this team
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// Where it came from
        /// </summary>
        public MatchupModel ParentMatchup { get; set; }

        public MatchupEntryModel(double initialScore)
        {

        }
    }
}
