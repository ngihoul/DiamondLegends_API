namespace DiamondLegends.API.DTO
{
    public class GameViewList
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Season { get; set; }
        public TeamViewCalendar Away { get; set; }
        public TeamViewCalendar Home { get; set; }
        public int AwayRuns { get; set; } = 0;
        public int HomeRuns { get; set; } = 0;
        public int AwayHits { get; set; } = 0;
        public int HomeHits { get; set; } = 0;
        public int AwayErrors { get; set; } = 0;
        public int HomeErrors { get; set; } = 0;
    }
}
