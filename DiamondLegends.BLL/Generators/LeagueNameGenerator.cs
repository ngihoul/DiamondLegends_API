using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLegends.BLL.Generators
{
    public class LeagueNameGenerator
    {
        private readonly List<string> _Adjectives = new List<string>()
        {
            "Crimson", "Golden", "Majestic", "Silver", "Blaze", "Titan", "Ironclad", "Wild", "Blazing", "Champion", "Victory", "Pioneer",
            "Royal", "Emerald", "Viking", "Electric", "Shadow", "Eternal", "Scarlet", "Frozen", "Radiant", "Noble", "Glorious", "Thunder",
            "Supreme", "Storm", "Mystic", "Celestial", "Inferno", "Astral", "Dark", "Frost", "Luminous", "Phantom", "Endless", "Raging",
            "Thunderous", "Majestic", "Mighty", "Rising", "Furious", "Invincible", "Eternal", "Steel", "Prime", "Flaming", "Untamed",
            "Vibrant", "Scarlet", "Blazing", "Vast", "Indomitable", "Legendary", "Feral", "Radiant", "Twilight", "Opulent", "Valiant",
            "Celestial", "Thunderous", "Blazing", "Inexorable", "Epic", "Boundless", "Invulnerable", "Unyielding", "Dazzling", "Resilient",
            "Shining", "Vengeful", "Ethereal", "Steadfast", "Unstoppable", "Titanic", "Resplendent", "Galactic", "Unbreakable", "Timeless"
        };

        private readonly List<string> _GeographicLocations = new List<string>()
        {
            "Eastern", "Western", "Pacific Coast", "Great Lakes", "Southern", "Northern", "Midwest", "Mountain", "Central",
            "Bay Area", "Rocky Mountains", "Atlantic Coast", "Mid-Atlantic", "Southwest", "Desert", "Golden Coast", "Northern Lights",
            "Northwest", "Great Plains", "Southern Highlands", "Deep South", "Northern Star", "Skyline", "Blue Ridge", "Sunset", "Silver Peak",
            "Emerald Coast", "Golden Valley", "Crystal Shores", "Redwood Forest", "Diamond Coast", "Timberland", "Pine Hills", "Blue Bayou",
            "Sunrise", "Canyon", "Emerald Hills", "Thunder Bay", "Mystic Isles", "Ironclad Peak", "Azure Coast", "Windward Coast", "Lunar Valley",
            "Crystal Lake", "Frostlands", "Sierra Nevada", "Diamond Ridge", "Golden Horizon", "Lush Valley", "Red Sky", "Ironwood Forest",
            "Sunset Valley", "Wildflower Plains", "Shadowed Forest", "Starfall Coast", "Blizzard Ridge", "Opal Coast", "Blackstone Ridge",
            "Frostbite Hills", "Sapphire Shores", "Twilight Peaks", "Mirage Valley", "Dragon's Spine", "Moonlit Bay", "Cobalt Coast",
            "Obsidian Plains", "Golden Reef", "Majestic Mountains", "Whispering Forest", "Blue Lagoon", "Dawn Ridge", "Verdant Hills"
        };

        private readonly List<string> _Themes = new List<string>()
        {
            "Baseball League", "Pro League", "Championship", "League", "Division", "Tournament", "League Championship",
            "All-Star League", "Elite League", "Major League", "World Series", "Elite Championship", "International League",
            "Super League", "Ultimate League", "Diamond League", "Legends League", "Premier League", "National Championship",
            "Grand League", "Victory League", "Global League", "Premier Championship", "United League", "Hall of Fame League",
            "Champion's League", "Star League", "Skyline League", "Cosmic Championship", "Supreme League", "Glory League",
            "Dynasty League", "Titan League", "Future League", "Celestial League", "Legendary League", "Victory Championship",
            "King's League", "Realm League", "Zenith League", "Infinity League", "Eternal League", "Prime Championship", "Golden League",
            "Valor League", "Galaxy League", "Champion's Division", "Vanguard League", "Royal Championship", "Ascendant League",
            "Finals League", "Celestial Division", "Ultimate Championship", "Titanic League", "Champion League", "Future Championship",
            "Noble League", "Celestial Championship", "Starborn League", "Ultimate Dynasty", "Victory Realm", "Apex League",
            "Rising Stars League", "Power League", "Champion's Circuit", "Ascendant Championship", "Primordial League", "Twilight League",
            "Eclipse League", "Galactic Championship", "Eternal Realm", "Legendary Circuit", "Vanguard Championship", "Final Frontier League"
        };

        public string Generate()
        {
            string name;

            int randomNumber = Random.Shared.Next(2);

            if(randomNumber == 0)
            {
                name = _Adjectives[Random.Shared.Next(_Adjectives.Count)] + " " + _GeographicLocations[Random.Shared.Next(_GeographicLocations.Count)];
            }
            else
            {
                name = _Adjectives[Random.Shared.Next(_Adjectives.Count)] + " " + _Themes[Random.Shared.Next(_Themes.Count)];
            }

            return name;
        }
    }
}
