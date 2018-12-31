using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    [Table(Name = "Teams")]
    public class TeamModel
    {
        public int Id { get; set; }

        public string TeamName { get; set; }

        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();
    }
}
