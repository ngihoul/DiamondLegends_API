using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DiamondLegends.Domain.Models
{
    public class Game
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Season { get; set; }
        public Team Away { get; set; }
        public Team Home { get; set; }
        public int AwayRuns { get; set; } = 0;
        public int HomeRuns { get; set; } = 0;
        public int AwayHits { get; set; } = 0;
        public int HomeHits { get; set; } = 0;
        public int AwayErrors { get; set; } = 0;
        public int HomeErrors { get; set; } = 0;
    }

    public class GameQuery
    {
        public int? Season { get; set; } = null;
        public int? LeagueId { get; set; } = null;
        public int? TeamId { get; set; } = null;
        public int? Month { get; set; } = null;
        public int? Day { get; set; } = null;
    }
}
