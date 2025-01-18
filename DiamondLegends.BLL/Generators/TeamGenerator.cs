using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;
using System;
using System.Runtime.CompilerServices;

namespace DiamondLegends.BLL.Generators
{
    public class TeamGenerator
    {
        private readonly IUserRepository _userRepository;
        private readonly ICountryRepository _countryRepository;

        private readonly List<string> _Adjectives = new List<string>
        {
            "Crimson", "Golden", "Majestic", "Silver", "Blazing", "Titan", "Ironclad", "Thunderous",
            "Furious", "Viking", "Champion", "Victory", "Mighty", "Raging", "Radiant", "Iron",
            "Royal", "Blazing", "Supreme", "Frost", "Electric", "Storm", "Shadow", "Noble", "Vibrant",
            "Legendary", "Endless", "Dynamic", "Eternal", "Rising", "Unstoppable", "Invincible", "Powerful",
            "Fearless", "Epic", "Boundless", "Unyielding", "Resilient", "Invulnerable", "Dazzling", "Radiant",
            "Untamed", "Phantom", "Dreaded", "Majestic", "Shining", "Vengeful", "Steadfast", "Unstoppable",
            "Gallant", "Mighty", "Celestial", "Blazing", "Frosted", "Flaming", "Ravaging", "Vengeful",
            "Glorious", "Vibrant", "Fiery", "Astral", "Obsidian", "Brilliant", "Iron-willed", "Indomitable",
            "Titanic", "Celestial", "Luminous", "Valiant", "Infernal", "Primal", "Monumental", "Arcane",
            "Feral", "Hallowed", "Blistering", "Spectral", "Merciless", "Primordial", "Sovereign", "Mystic"
        };

        private readonly List<string> _Animals = new List<string>
        {
            "Tigers", "Lions", "Bears", "Eagles", "Sharks", "Wolves", "Falcons", "Panthers", "Cheetahs",
            "Bulls", "Rams", "Hawks", "Dragons", "Rhinos", "Cobras", "Mustangs", "Whales", "Dolphins",
            "Leopards", "Raptors", "Crocodiles", "Buffaloes", "Stallions", "Pumas", "Gorillas", "Elephants",
            "Cheetahs", "Vultures", "Snakes", "Gators", "Pythons", "Sharks", "Grizzlies", "Cougars",
            "Jackals", "Rabbits", "Lynxes", "Bison", "Wolverines", "Warthogs", "Chameleons", "Wombats",
            "Penguins", "Koalas", "Kangaroos", "Gazelles", "Zebras", "Armadillos", "Cheetahs", "Falcons",
            "Scorpions", "Hyenas", "Otters", "Egrets", "Hummingbirds", "Albatross", "Foxes", "Owls",
            "Lemurs", "Meerkats", "Geckos", "Orcas", "Stingrays", "Badgers", "Peacocks", "Ibex",
            "Hedgehogs", "Cranes", "Ospreys", "Pelicans", "Storks", "Pandas", "Elks", "Minks", "Tunas",
            "Antelopes", "Flamingos", "Kiwis", "Moose", "Basilisks", "Peregrines", "Porcupines"
        };

        private readonly List<string> _GeographicLocations = new List<string>
        {
            "Valley", "Mountains", "Bay", "Coast", "Plains", "Lakes", "Hills", "Canyon", "Forest", "Crest",
            "Islands", "Shore", "Ridge", "River", "Falls", "Peak", "Field", "Cove", "Heights", "Creek",
            "Glades", "Mesa", "Desert", "Savannah", "Harbor", "Steppes", "Tundra", "Jungle", "Wilderness",
            "Highlands", "Frontier", "Woods", "Cavern", "Ocean", "Prairie", "Isle", "Bluff", "Tide",
            "Summit", "Hollow", "Pass", "Ledge", "Crest", "Highlands", "Cliffs", "Coves", "Wellspring",
            "Knolls", "Ravine", "Ridgeway", "Fjord", "Veldt", "Moor", "Caverns", "Tundra", "Swamps",
            "Foothills", "Plateau", "Horizon", "Meadows", "Archipelago", "Groves", "Atoll", "Crater",
            "Fen", "Marsh", "Dunes", "Glacier", "Promontory", "Cape", "Peninsula", "Lagoon", "Basin"
        };

        private readonly List<string> _BaseballTerms = new List<string>
        {
            "Sluggers", "Batters", "Pitchers", "Knockouts", "Swingers", "Homers", "Strikeforce", "Diamond",
            "Hitters", "Slammers", "All-Stars", "Rangers", "Champions", "Storm", "Warriors", "Aces",
            "Legends", "Rivals", "Mavericks", "Raiders", "Vanguards", "Stormers", "Defenders", "Knights",
            "Titans", "Sharks", "Vikings", "Knockouts", "Ace", "Squad", "Blitz", "Chargers", "Braves",
            "Warlocks", "Kings", "Fury", "Invaders", "Stars", "Voyagers", "Predators", "Warhawks",
            "Thunder", "Explorers", "Rebels", "Wreckers", "Falcons", "Hurricanes", "Fury", "Dragons",
            "Blizzards", "Pirates", "Rangers", "Rivals", "Titans", "Sparks", "Wolves", "Eagles",
            "Bulls", "Royals", "Lions", "Troopers", "Hawks", "Mavericks", "Legends", "Marauders", "Spartans",
            "Clutchers", "Outlaws", "Scorchers", "Nomads", "Trailblazers", "Pioneers", "Commanders", "Enforcers",
            "Renegades", "Guardians", "Sentinels", "Dreadnoughts", "Bulldogs", "Riders", "Scouts"
        };

        List<string> _Cities = new List<string>
        {
            // Afrique
            "Lagos", "Cairo", "Johannesburg", "Nairobi", "Addis Ababa", "Casablanca", "Cape Town",
            "Algiers", "Kampala", "Dakar", "Accra", "Abidjan", "Dar es Salaam", "Kinshasa", "Luanda",
            "Harare", "Windhoek", "Tripoli", "Bamako", "Ouagadougou", "Lome", "Libreville", "Bujumbura",
            "Antananarivo", "Freetown", "Monrovia", "Kigali", "Asmara", "Djibouti", "Malabo", "Port Louis",

            // Amérique du Nord
            "New York", "Los Angeles", "Toronto", "Mexico City", "Chicago", "Houston", "Miami",
            "Vancouver", "Montreal", "Havana", "Kingston", "Guatemala City", "San Jose", "Panama City",
            "Las Vegas", "Atlanta", "Orlando", "Denver", "Boston", "Philadelphia", "Seattle", "Ottawa",
            "San Diego", "Dallas", "San Francisco", "Calgary", "Quebec City", "Port-au-Prince", "Tegucigalpa",

            // Amérique du Sud
            "Buenos Aires", "Rio de Janeiro", "São Paulo", "Bogotá", "Lima", "Santiago", "Caracas",
            "Quito", "Montevideo", "La Paz", "Asunción", "Barranquilla", "Medellín", "Cali", "Salvador",
            "Brasilia", "Maracaibo", "Manaus", "Santa Cruz", "Arequipa", "Cordoba", "Rosario", "Valparaiso",

            // Asie
            "Tokyo", "Shanghai", "Mumbai", "Seoul", "Jakarta", "Bangkok", "Beijing", "Dubai", "Hong Kong",
            "Singapore", "Riyadh", "Kuala Lumpur", "Tehran", "Karachi", "Manila", "Hanoi", "Baghdad",
            "Doha", "Kuwait City", "Dhaka", "Yerevan", "Tashkent", "Colombo", "Kathmandu", "Amman",
            "Bishkek", "Phnom Penh", "Vientiane", "Ulaanbaatar", "Muscat", "Islamabad", "Ashgabat", "Dushanbe",

            // Europe
            "London", "Paris", "Berlin", "Madrid", "Rome", "Athens", "Lisbon", "Moscow", "Stockholm",
            "Amsterdam", "Brussels", "Vienna", "Zurich", "Warsaw", "Dublin", "Oslo", "Prague", "Helsinki",
            "Copenhagen", "Budapest", "Belgrade", "Bucharest", "Sofia", "Vilnius", "Riga", "Tallinn",
            "Reykjavik", "Bratislava", "Ljubljana", "Sarajevo", "Skopje", "Valletta", "Monaco", "San Marino",
            "Andorra la Vella", "Luxembourg", "Gibraltar", "Edinburgh", "Cardiff", "Geneva", "Krakow",

            // Océanie
            "Sydney", "Melbourne", "Auckland", "Wellington", "Brisbane", "Perth", "Adelaide",
            "Christchurch", "Suva", "Port Moresby", "Honiara", "Noumea", "Papeete", "Apia",
            "Nukuʻalofa", "Port Vila", "Funafuti", "Majuro", "Yaren", "Palikir",

            // Villes célèbres ou historiques
            "Jerusalem", "Istanbul", "Kyoto", "Florence", "Petra", "Machu Picchu", "Timbuktu", "Angkor Wat",
            "Venice", "Córdoba", "Fez", "Mecca", "Bagan", "Cusco", "Athens", "Samarkand", "Dubrovnik",
            "Persepolis", "Giza", "Chichen Itza", "Tikal", "Palmyra", "Pompeii", "Ephesus", "Lhasa",
            "Carcassonne", "Bruges", "Toledo", "Ronda", "Split", "Rhodes", "Mdina"
        };


        public TeamGenerator(IUserRepository userRepository, ICountryRepository countryRepository)
        {
            _userRepository = userRepository;
            _countryRepository = countryRepository;
        }

        public async Task<Team> Generate(League league, int season)
        {
            return new Team()
            {
                Name = GenerateRandomName(),
                Owner = await GetBotUser(),
                City = GenerateRandomCity(),
                Country = await ChooseRandomCountry(),
                League = league,
                Season = season,
                CurrentDay = 0,
                Budget = 1000000,
                Logo = null,
                Color_1 = GenerateRandomColor("light"),
                Color_2 = GenerateRandomColor("dark"),
                // Color_3 = "FFFFFF"
            };
        }

        private string GenerateRandomName()
        {
            string name;

            int randomNumber = Random.Shared.Next(3);

            if (randomNumber == 0)
            {
                name = _Adjectives[Random.Shared.Next(_Adjectives.Count)] + " " + _GeographicLocations[Random.Shared.Next(_GeographicLocations.Count)];
            }
            else if(randomNumber == 1)
            {
                name = _Adjectives[Random.Shared.Next(_Adjectives.Count)] + " " + _BaseballTerms[Random.Shared.Next(_BaseballTerms.Count)];
            } else
            {
                name = _Adjectives[Random.Shared.Next(_Adjectives.Count)] + " " + _Animals[Random.Shared.Next(_Animals.Count)];
            }

            return name;
        }

        private string GenerateRandomCity()
        {
            return _Cities[Random.Shared.Next(_Cities.Count)];
        }

        private async Task<Country> ChooseRandomCountry()
        {
            IEnumerable<Country> countries = await _countryRepository.GetAll();
            return countries.ElementAt(Random.Shared.Next(countries.Count()));
        }

        private string GenerateRandomColor(string type)
        {
            int red = 0;
            int green = 0;
            int blue = 0;

            if (type == "light")
            {
                red = Random.Shared.Next(180, 256);
                green = Random.Shared.Next(180, 256);
                blue = Random.Shared.Next(180, 256);
            }
            else if (type == "dark")
            {
                red = Random.Shared.Next(0, 181);
                green = Random.Shared.Next(0, 181);
                blue = Random.Shared.Next(0, 181);
            }

            return $"{red:X2}{green:X2}{blue:X2}";
        }

        private async Task<User> GetBotUser()
        {
            return await _userRepository.GetById(1);
        }
    }
}
