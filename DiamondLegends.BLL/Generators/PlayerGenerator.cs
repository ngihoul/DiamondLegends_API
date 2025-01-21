using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Enums;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Generators
{
    public class PlayerGenerator
    {
        private static Dictionary<string, int> _Countries = new Dictionary<string, int>()
        {
            // Between 1 and 60 = USA - id = 185
            // Between 61 and 73 = Dominican Republic - id = 50
            // Between 75 and 79 = Venezuela - id = 189
            // Between 80 and 84 = Cuba - id = 44
            // Between 85 and 87 = Japan - id = 84
            // Between 88 and 90 = Mexico - id = 112
            // Between 91 and 93 = South Korea - id = 90
            // Between 93 and 95 = Canada - id = 31
            // Between 96 and 98 = Colombia - id = 37
            // Between 98 and 100 = Panama - id = 133
            { "USA", 185 },
            { "Dominican Republic", 50 },
            { "Venezuela", 189 },
            { "Cuba", 44 },
            { "Japan", 84 },
            { "Mexico", 112 },
            { "South Korea", 90 },
            { "Canada", 31 },
            { "Colombia", 37 },
            { "Panama", 133 }
        };

        #region Props
        private readonly ICountryRepository _countryRepository;

        private static List<int> _NationalityIds = new List<int>()
        {
            _Countries["USA"], _Countries["Dominican Republic"], _Countries["Venezuela"], _Countries["Cuba"], _Countries["Japan"], _Countries["Mexico"], _Countries["South Korea"], _Countries["Canada"], _Countries["Colombia"], _Countries["Panama"]
        };

        private static List<string> _EnglishFirstnames = new List<string>
        {
            "James", "John", "Robert", "Michael", "William", "David", "Richard", "Joseph", "Charles", "Thomas",
            "Christopher", "Daniel", "Paul", "Mark", "Donald", "George", "Kenneth", "Steven", "Edward", "Brian",
            "Ronald", "Anthony", "Kevin", "Gary", "Timothy", "Jose", "Larry", "Jeffrey", "Frank", "Scott",
            "Eric", "Stephen", "Andrew", "Gregory", "Joshua", "Jerry", "Dennis", "Walter", "Patrick", "Peter",
            "Harold", "Douglas", "Henry", "Arthur", "Ryan", "Jack", "Joe", "Albert", "Austin", "Jacob", "Ethan",
            "Nathan", "Aaron", "Samuel", "Caleb", "Lucas", "Owen", "Zachary", "Adam", "Luke", "Matthew", "Isaac",
            "Benjamin", "Elijah", "Liam", "Mason", "Alexander", "Oliver", "Michael", "Isaiah", "Cooper", "Carter",
            "Wyatt", "Grayson", "Dylan", "Gavin", "Austin", "Hunter", "Logan", "Jaxon", "Chase", "Landon", "Bryce",
            "Cole", "Alex", "Nathaniel", "Jace", "Miles", "Seth", "Max", "Nolan", "Xander", "Cameron", "Brayden",
            "Theo", "Benjamin", "Aidan", "Nash", "Simon", "Jared", "Mason", "Emmett", "Harrison", "Silas", "Victor",
            "Jasper", "Riley", "Christian", "Finn", "Kingston", "Asher", "Kai", "Jesse", "Bennett", "Milo", "Toby",
            "Levi", "Emerson", "Zane", "Blake", "Ryder", "Tristan", "Nash", "Zachary", "Spencer", "Maddox", "Eli",
            "Noah", "Jordan", "Travis", "Brock", "Nico", "Ford", "Malcolm", "Theo", "Baxter", "Calvin", "Victor",
            "Bryan", "Eli", "Julian", "Francis", "Dylan", "Chandler", "Graham", "Louis", "Arthur", "Mason", "Jared",
            "Jensen", "Lincoln", "Liam", "Quinn", "Mack", "Coleman", "Rory", "Dean", "Bennett", "Zane", "Clyde",
            "Dante", "Gage", "Lennox", "Cyrus", "Finnian", "Beau", "Wade", "Dax", "Bennett", "Damien", "Brock",
            "Caden", "Jaden", "Cale", "Graham", "Tate", "Rowan", "Beckett", "Vance", "Jett", "Colton", "Julius",
            "Reed", "Zeke", "Rhett", "Hale", "Stefan", "Gordon", "Rufus", "Malcolm", "Trent", "Tanner", "Riley",
            "Quincy", "Felix", "Kendall", "Dillon", "Thaddeus", "Alfred", "Benson", "Franklin", "Sterling", "Blaise",
            "Harvey", "Maverick", "Dorian", "Elliot", "Ezekiel", "Thatcher", "Milo", "Trenton", "Soren", "Seth",
            "Winston", "Alden", "Niles", "Marvin", "Devon", "Lyle", "Saul", "Harlan", "Wilder"
        };

        private static List<string> _EnglishLastnames = new List<string>
        {
            "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor",
            "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin", "Thompson", "Garcia", "Martinez", "Roberts",
            "Clark", "Rodriguez", "Lewis", "Lee", "Walker", "Hall", "Allen", "Young", "King", "Wright",
            "Scott", "Torres", "Nguyen", "Hill", "Adams", "Baker", "Nelson", "Carter", "Mitchell", "Perez",
            "Robinson", "Gonzalez", "Sanchez", "Patel", "Ramirez", "Ross", "Cole", "Murphy", "Bailey", "Rivera",
            "Cooper", "Richardson", "Howard", "Ward", "Flores", "Diaz", "Lopez", "Graham", "Kelly", "Sanders",
            "Price", "Bennett", "Wood", "Barnes", "Ross", "Hughes", "Chavez", "James", "Butler", "Simmons",
            "Foster", "Bryant", "Alexander", "Russell", "Griffin", "Diaz", "Henderson", "Douglas", "Carlson",
            "Curtis", "Hoffman", "Gibson", "Gonzales", "Glover", "Mendoza", "McDonald", "McCarthy", "Stone",
            "Harrison", "Franklin", "Richards", "Jordan", "Ferguson", "Marshall", "Burns", "Phillips", "Reed",
            "Vasquez", "Fox", "Warren", "Webb", "Simpson", "James", "Curtis", "Elliott", "Chavez", "Daniels",
            "Harper", "Gibson", "Jenkins", "Mills", "Griffith", "Jensen", "Stone", "Knight", "Meyers", "Davidson",
            "Palmer", "Murray", "Wheeler", "Gregory", "Sullivan", "Berry", "Murphy", "Chapman", "Curtis", "Bishop",
            "Hendrix", "Vega", "Woods", "Carr", "Hunter", "Pearson", "Wallace", "Gaines", "Norris", "Ryan",
            "Jameson", "Robles", "Bates", "Harrison", "Kennedy", "Sutton", "Armstrong", "Schmidt", "Perry"
        };

        private static List<string> _SpanishFirstnames = new List<string>
        {
            "José", "Juan", "Carlos", "Luis", "Jorge", "Miguel", "Antonio", "Francisco", "David", "Manuel",
            "Pedro", "Alejandro", "Ricardo", "Fernando", "Daniel", "Eduardo", "Diego", "Sergio", "Raúl", "Andrés",
            "Pablo", "Ramón", "Álvaro", "Víctor", "Antonio", "Emilio", "Mario", "Rafael", "Iván", "Héctor",
            "César", "Julio", "Javier", "Esteban", "Martín", "Guillermo", "Salvador", "José Antonio", "Felipe",
            "Cristian", "Samuel", "Ángel", "Adrián", "Óscar", "Lorenzo", "Isaac", "Tomás", "Simón", "Iván",
            "Alfredo", "Félix", "Nicolás", "Hugo", "Benjamín", "Maximiliano", "Luis Ángel", "Ramiro", "Marcelo",
            "Adolfo", "Santiago", "Ricardo", "Ezequiel", "Bernardo", "Joaquín", "Gonzalo", "Bautista", "Arturo",
            "Martín", "José Luis", "Francisco Javier", "Emiliano", "Leonardo", "Rubén", "Aarón", "Santiago",
            "Fernando", "Víctor Manuel", "Juan Carlos", "Raúl", "Carlos Alberto", "Carlos Javier", "Luis Fernando",
            "Agustín", "Tobías", "Ángel David", "Sergio Daniel", "Mauricio", "René", "Germán", "Ricardo Alberto",
            "Pascual", "Gerardo", "Leandro", "José Miguel", "Eduardo Luis", "Juan José", "Pablo Antonio", "Martín Ángel",
            "Ricardo José", "Antonio José", "José Luis", "David Alejandro", "Julian", "Fabián", "Felipe", "Raúl Ángel"
        };

        private static List<string> _SpanishLastnames = new List<string>
        {
            "García", "Martínez", "López", "Hernández", "Pérez", "González", "Rodríguez", "Fernández", "Luna", "Díaz",
            "Álvarez", "Jiménez", "Morales", "Mendoza", "Ruiz", "Sánchez", "Ramírez", "Torres", "Vázquez", "Moreno",
            "Ríos", "Cruz", "Reyes", "Blanco", "Jiménez", "Castro", "Delgado", "Flores", "Vargas", "Gutiérrez",
            "Ramírez", "Chávez", "Cabrera", "Guerrero", "Soto", "Mora", "Guerrero", "Ortega", "Vázquez", "Torres",
            "Serrano", "Márquez", "Rojas", "Salazar", "Castillo", "Santos", "Acosta", "Hernández", "Morales",
            "Valdez", "Ruiz", "Giménez", "Martín", "Cardoso", "Navarro", "Paredes", "Serrano", "Suárez",
            "Gonzales", "Muñoz", "Zapata", "Del Río", "Navarro", "Barrios", "Escobar", "Molina", "Carvalho",
            "Rosales", "Pinto", "Montoya", "Salas", "Bermúdez", "Gálvez", "López", "Ávila", "Cordero", "Bautista",
            "Serrano", "Pizarro", "Castro", "Peña", "Moya", "Escobar", "Valencia", "López", "Reyes", "Sánchez",
            "Bravo", "González", "Mármol", "Guerra", "Gonzalo", "Hidalgo", "García", "Del Castillo", "Jiménez",
            "Zúñiga", "Guzmán", "Fuentes", "Salazar", "Gallegos", "Medina", "Acevedo", "Alvarado", "Cisneros",
            "Díaz", "Pacheco", "Serrato", "Lozano", "Castillo", "Navarro", "Vega", "Arias", "López", "Varela",
            "Domínguez", "Méndez", "Herrera", "Blanco", "Mora", "Bermúdez", "Cordero", "Ríos", "González", "Ferrer"
        };

        private static List<string> _JapaneseFirstnames = new List<string>
        {
            "Hiroshi", "Takeshi", "Yuki", "Takashi", "Ryo", "Shota", "Satoshi", "Daiki", "Yusuke", "Kaito",
            "Shinji", "Sho", "Yuji", "Kenta", "Ren", "Tomo", "Kei", "Ryota", "Haruto", "Riku",
            "Keita", "Takuya", "Shohei", "Ryuji", "Issei", "Tetsuya", "Haruki", "Satoru", "Naoki",
            "Ryusei", "Seiji", "Takumi", "Hiroki", "Yuuto", "Junya", "Tsubasa", "Ichiro", "Masashi",
            "Kouki", "Yuya", "Hikaru", "Yoshiki", "Yuta", "Hosei", "Kazuki", "Kazuma", "Ryosuke",
            "Shou", "Yamato", "Shun", "Taichi", "Satoru", "Kazuya", "Kou", "Kiyoshi", "Shoma",
            "Genta", "Keisuke", "Satoshi", "Yuichiro", "Haruma", "Kouhei", "Rei", "Yuto", "Mitsuki",
            "Kiyoshi", "Seiji", "Natsuki", "Jiro", "Shiro", "Riki", "Yuki", "Taro", "Shohei",
            "Eita", "Takahiro", "Katsuo", "Ryu", "Kenta", "Koji", "Keiji", "Renji", "Ryohei",
            "Isao", "Masato", "Ichiro", "Naoya", "Shunya", "Eiji", "Aki", "Minoru", "Ryoji"
        };

        private static List<string> _JapaneseLastnames = new List<string>
        {
            "Takahashi", "Yamamoto", "Kobayashi", "Sato", "Tanaka", "Watanabe", "Ito", "Nakamura", "Kato", "Yoshida",
            "Yamashita", "Fujimoto", "Matsumoto", "Inoue", "Kimura", "Shimizu", "Kaneko", "Hasegawa", "Ogawa", "Mori",
            "Ishikawa", "Murakami", "Sasaki", "Yamada", "Nakajima", "Hirata", "Goto", "Okada", "Fujii", "Shibata",
            "Suzuki", "Hashimoto", "Takahara", "Abe", "Nishimura", "Ueda", "Kusunoki", "Miyamoto", "Matsuda", "Oda",
            "Takeuchi", "Kobayashi", "Asano", "Endo", "Arakawa", "Noda", "Okamoto", "Saito", "Fukuda", "Sakai",
            "Mizuno", "Aoki", "Hoshino", "Kuroda", "Sakamoto", "Tani", "Kagawa", "Tomioka", "Sakamoto", "Wada",
            "Imai", "Yamaguchi", "Kubo", "Sekiguchi", "Okazaki", "Kobayashi", "Furukawa", "Taniguchi", "Kishi", "Murata",
            "Kobayashi", "Nakanishi", "Fujita", "Yokoyama", "Yamamoto", "Hirano", "Tachibana", "Kawai", "Kondo", "Tominari",
            "Sakai", "Kurosawa", "Oshima", "Hamada", "Ogino", "Yukimura", "Kawasaki", "Hiraki", "Otake", "Mukai",
            "Sakai", "Ichikawa", "Saeki", "Tominaga", "Sekiya", "Kawaguchi", "Kobori", "Yasuda", "Yamane", "Oda",
            "Kanemoto", "Hachiya", "Itoh", "Tsukamoto", "Tomiya", "Nagata", "Fukui", "Tanioka", "Toyoda", "Yamagishi",
            "Koyama", "Ishii", "Hasegawa", "Shibayama", "Fujisawa", "Harada", "Shinozaki", "Tomioka", "Mimura"
        };

        private static List<string> _KoreanFirstnames = new List<string>
        {
            "Jin", "Minho", "Jiho", "Seojin", "Minseok", "Seungmin", "Jihoon", "Sungjae", "Jiwon", "Hyun",
            "Jiwon", "Kyung", "Seungwoo", "Hyunji", "Sungmin", "Jungwoo", "Sangho", "Hojin", "Jinhee", "Sumin",
            "Soojin", "Jiwon", "Seungwoo", "Jimin", "Sungjae", "Minseok", "Chanyeol", "Kyungsoo", "Sungkyu",
            "Seokjin", "Haein", "Jinwoo", "Minji", "Gyu", "Yujin", "Hyunseok", "Seungyoon", "Sungwoo", "Myeong",
            "Jaemin", "Jaeyoung", "Sungmin", "Jinmo", "Seungwoo", "Kyungmin", "Yunho", "Hoseok", "Jiwon",
            "Sungjae", "Sungjin", "Jisoo", "Seongmin", "Eunji", "Jungmin", "Sangmin", "Jinwook", "Sangho",
            "Sangmin", "Minsu", "Yunji", "Haejun", "Sungjoo", "Kyungjoo", "Taehyung", "Seungjin", "Seonwoo",
            "Taeyang", "Youngho", "Woobin", "Jaesun", "Yunho", "Jiwon", "Jungwoo", "Sungmin", "Gyuho", "Myeong",
            "Jiwon", "Sooho", "Hyeon", "Seongjin", "Dongho", "Jihyun", "Sungmin", "Hojin", "Jaekyung", "Jinsung"
        };

        private static List<string> _KoreanLastnames = new List<string>
        {
            "Kim", "Lee", "Park", "Choi", "Jang", "Cho", "Yoon", "Im", "Han", "Kang",
            "Jeong", "Joo", "Ryu", "Oh", "Seo", "Lim", "Shin", "Jung", "Bae", "Chung",
            "Ahn", "Song", "Hwang", "Yang", "Moon", "Ko", "Choi", "Heo", "Hong",
            "Kim", "Sohn", "Yoo", "Noh", "Huh", "Kwon", "Park", "Suh", "Jung", "Jo",
            "Lee", "Cha", "Byun", "Oh", "Han", "Lee", "Kim", "Seo", "Park", "Jeong",
            "Son", "Baek", "Kang", "Nam", "Baek", "Kang", "Lim", "Lee", "Shim", "Cho",
            "Bae", "Ryu", "Joo", "Kim", "Yu", "Yun", "Kim", "Jang", "Go", "Kim",
            "Jung", "Yoo", "Choi", "Song", "Ahn", "Jung", "Hwang", "Bae", "Seo",
            "Noh", "Lee", "Jung", "Woo", "Hwang", "Lim", "Oh", "Baek", "Kim", "Song",
            "Jung", "Im", "Kim", "Park", "Yun", "Yoo", "Han", "Lee", "Jeong"
        };

        private static List<string> _FrenchFirstnames = new List<string>
        {
            "Pierre", "Jean", "Michel", "Paul", "Jacques", "François", "Louis", "Henri", "André", "Claude",
            "Éric", "Bernard", "Robert", "Alain", "Georges", "Luc", "Thierry", "Christian", "Daniel", "Patrick",
            "Jacques", "Nicolas", "Marc", "Antoine", "Yves", "Mathieu", "David", "Vincent", "Gérard", "Frédéric",
            "Philippe", "Éric", "Charles", "Dominique", "Guillaume", "Thierry", "Benoît", "Régis", "Julien",
            "Laurent", "Sylvain", "Maurice", "René", "Olivier", "Pascal", "Michel", "Simon", "Sébastien", "Arnaud",
            "Armand", "Jacques", "Bruno", "Maxime", "Léon", "Julien", "Bastien", "Cédric", "Victor", "Romain",
            "Jérôme", "Stéphane", "Léonard", "Henri", "Marcel", "Claude", "Édouard", "Xavier", "Frédéric",
            "Yannick", "Hervé", "Louis", "Thibault", "Lucien", "Bernard", "Eddy", "Félix", "Augustin", "Georges"
        };

        private static List<string> _FrenchLastnames = new List<string>
        {
            "Dupont", "Martin", "Bernard", "Thomas", "Petit", "Robert", "Richard", "Lemoine", "Moreau", "Lemoine",
            "Durand", "Lefevre", "Benoit", "Girard", "Lambert", "David", "Caron", "Lemoine", "Gauthier", "Dufresne",
            "Leclerc", "Boucher", "Michaud", "Chartrand", "Faure", "Deschamps", "Chauvin", "Gagnon", "Joly",
            "Roy", "Lemoine", "Perrot", "Gérard", "Marchand", "Leclerc", "Pires", "Dufresne", "Faust",
            "Hebert", "Barbier", "Delacroix", "Mallet", "Lemoine", "Barreau", "Lemoine", "Brunet", "Meunier",
            "Boulanger", "Lemoine", "Dupuis", "Roux", "Perrin", "Lemoine", "Legrand", "Dufresne", "Boutin",
            "Roche", "Thibault", "Deslauriers", "Chauveau", "Laborde", "Francois", "Gagnon", "Lemoine", "Dupuy",
            "Lemoine", "Pelletier", "Caron", "Berthelot", "Côté", "Lemoine", "Descamps", "Fournier", "Chapelle",
            "Dufresne", "Gosselin", "Benoit", "Fortin", "Plante", "Caron", "Lemoine", "Lemoine", "Chaput", "Lemoine",
            "Marechal", "Chevalier", "Vermette", "Lemoine", "Malo", "Joly", "Lemoine"
        };

        private static Dictionary<string, List<string>> _Languages = new Dictionary<string, List<string>>()
        {
            {
                "english",
                new List<string>
                {
                    "us",
                }
            },
            {
                "spanish",
                new List<string>
                {
                    "cu", "do", "mx", "ve", "co", "pa"
                }
            },
            {
                "french",
                new List<string>
                {
                    "ca",
                }
            },
            {
                "japanese",
                new List<string>
                {
                    "jp"
                }
            },
            {
                "korean",
                new List<string>
                {
                    "kr"
                }
            }
        };

        private static Dictionary<string, Dictionary<string, List<string>>> _Names = new Dictionary<string, Dictionary<string, List<string>>>
        {
            {"english",
                new Dictionary<string, List<string>>
                {
                    {"firstnames", _EnglishFirstnames},
                    {"lastnames", _EnglishLastnames}
                }
            },
            {"spanish",
                new Dictionary<string, List<string>>
                {
                    {"firstnames", _SpanishFirstnames},
                    {"lastnames", _SpanishLastnames}
                }
            },
            {"french",
                new Dictionary<string, List<string>>
                {
                    {"firstnames", _FrenchFirstnames},
                    {"lastnames", _FrenchLastnames}
                }
            },
            {"japanese",
                new Dictionary<string, List<string>>
                {
                    {"firstnames", _JapaneseFirstnames},
                    {"lastnames", _JapaneseLastnames}
                }
            },
            {"korean",
                new Dictionary<string, List<string>>
                {
                    {"firstnames", _KoreanFirstnames},
                    {"lastnames", _KoreanLastnames}
                }
            },
        };
        #endregion

        #region Constructor
        public PlayerGenerator(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        #endregion

        #region Methods
        public async Task<Player> Generate(IEnumerable<Position> positions)
        {
            Country nationality = await GenerateRandomNationality();
            string language = GetLanguage(nationality);
            int power = GenerateBattingSkills(positions);
            int running = GenerateRunning(positions, power);
            int defense = GenerateSkill(20, 91);
            int mental = GenerateSkill(30, 96);
            int stamina = GenerateSkill(30, 96);
            int control = GenerateControlSkill(positions);
            int velocity = GenerateVelocitySkill(positions, control);
            int movement = GenerateMovement(positions, control);

            return new Player()
            {
                Firstname = GenerateRandomFirstname(language),
                Lastname = GenerateRandomLastname(language),
                Nationality = nationality,
                DateOfBirth = GenerateRandomDateOfBirth(),
                Throw = Random.Shared.Next(2), // 0 = RHP or 1 = LHP
                Bat = Random.Shared.Next(2), // 0 = Right or 1 = Left
                Salary = 20000,
                Positions = positions,
                Contact = GenerateBattingSkills(positions),
                ContactPotential = GenerateBattingPotentialSkills(positions),
                Power = power,
                PowerPotential = GenerateBattingPotentialSkills(positions),
                Running = running,
                RunningPotential = running + 20 >= 100 ? 100 : running + 20,
                Defense = defense,
                DefensePotential = defense + 30 >= 100 ? 100 : defense + 30,
                Mental = mental,
                MentalPotential = mental + 30 >= 100 ? 100 : mental + 30,
                Stamina = stamina,
                StaminaPotential = stamina + 30 >= 100 ? 100 : stamina + 30,
                Control = control,
                ControlPotential = control + 20 >= 100 ? 100 : control + 20,
                Velocity = velocity,
                VelocityPotential = velocity + 30 >= 100 ? 100 : velocity + 30,
                Movement = movement,
                MovementPotential = movement + 30 >= 100 ? 100 : movement + 30
            };
        }

        private async Task<Country> GenerateRandomNationality()
        {
            

            int randomNumber = Random.Shared.Next(101);
            int countryId = 0;

            switch (randomNumber)
            {
                case >= 1 and <= 60:
                    countryId = _Countries["USA"];
                    break;
                case >= 61 and <= 73:
                    countryId = _Countries["Dominican Republic"];
                    break;
                case >= 75 and <= 79:
                    countryId = _Countries["Venezuela"];
                    break;
                case >= 80 and <= 84:
                    countryId = _Countries["Cuba"];
                    break;
                case >= 85 and <= 87:
                    countryId = _Countries["Japan"];
                    break;
                case >= 88 and <= 90:
                    countryId = _Countries["Mexico"];
                    break;
                case >= 91 and <= 93:
                    countryId = _Countries["South Korea"];
                    break;
                case >= 93 and <= 95:
                    countryId = _Countries["Canada"];
                    break;
                case >= 96 and <= 98:
                    countryId = _Countries["Colombia"];
                    break;
                case >= 98 and <= 100:
                    countryId = _Countries["Panama"];
                    break;
                default:
                    countryId = _Countries["USA"];
                    break;
            }

            Country? nationality = await _countryRepository.GetById(countryId);

            if(nationality is null)
            {
                throw new Exception("Le pays n'existe pas");
            }

            return nationality;
        }

        private string GetLanguage(Country nationality)
        {
            return _Languages.FirstOrDefault(lang => lang.Value.Contains(nationality.Alpha2)).Key;
        }

        private string GenerateRandomFirstname(string language)
        {
            return _Names[language]["firstnames"][Random.Shared.Next(_Names[language]["firstnames"].Count())];
        }

        private string GenerateRandomLastname(string language)
        {
            return _Names[language]["lastnames"][Random.Shared.Next(_Names[language]["lastnames"].Count())];
        }

        private DateTime GenerateRandomDateOfBirth()
        {
            DateTime today = DateTime.Today;
            DateTime dateOfBirth = new DateTime();

            int randomNumber = Random.Shared.Next(101);

            switch (randomNumber)
            {
                // - - de 25 ans : 17 %
                case >= 1 and <= 17:
                    dateOfBirth = GenerateDate(today.Year - 24, today.Year - 17);
                    break;

                // - 25 - 29 ans : 30 %
                case >= 18 and <= 48:
                    dateOfBirth = GenerateDate(today.Year - 28, today.Year - 25);
                    break;

                // - 30 - 34 ans : 28 %
                case >= 49 and <= 76:
                    dateOfBirth = GenerateDate(today.Year - 34, today.Year - 29);
                    break;

                // - 35 - 39 ans : 18 %
                case >= 77 and <= 94:
                    dateOfBirth = GenerateDate(today.Year - 39, today.Year - 35);
                    break;

                // - + de 40 ans : 7 %
                case >= 95 and <= 100:
                    dateOfBirth = GenerateDate(today.Year - 46, today.Year - 40);
                    break;

                default:
                    dateOfBirth = GenerateDate(today.Year - 24, today.Year - 17);
                    break;
            }

            return dateOfBirth;
        }
        private DateTime GenerateDate(int minYear, int maxYear)
        {
            return new DateTime(Random.Shared.Next(minYear, maxYear), Random.Shared.Next(1, 13), Random.Shared.Next(1, 29));
        }

        private int GenerateBattingSkills(IEnumerable<Position> positions)
        {
            int skill;

            if (positions.Contains(Position.StartingPitcher) || positions.Contains(Position.ReliefPitcher) || positions.Contains(Position.CloserPitcher))
            {
                skill = GenerateSkill(5, 61);
            }
            else if (positions.Contains(Position.Catcher))
            {
                skill = GenerateSkill(40, 81);
            }
            else
            {
                skill = GenerateSkill(40, 96);
            }

            return skill;
        }

        private int GenerateBattingPotentialSkills(IEnumerable<Position> positions)
        {
            int skill;

            if (positions.Contains(Position.StartingPitcher) || positions.Contains(Position.ReliefPitcher) || positions.Contains(Position.CloserPitcher))
            {
                skill = GenerateSkill(30, 71);
            }
            else if (positions.Contains(Position.Catcher))
            {
                skill = GenerateSkill(60, 96);
            }
            else
            {
                skill = GenerateSkill(60, 101);
            }

            return skill;
        }

        private int GenerateRunning(IEnumerable<Position> positions, int power)
        {
            int skill;

            if(positions.Contains(Position.FirstBase) || positions.Contains(Position.DesignatedHitter) || power >= 80) {
                skill = GenerateSkill(20, 61);
            }
            else
            {
                skill = GenerateSkill(40, 96);
            }

            return skill;
        }

        private int GenerateControlSkill(IEnumerable<Position> positions)
        {
            int skill;

            if(positions.Contains(Position.StartingPitcher) || positions.Contains(Position.ReliefPitcher) || positions.Contains(Position.CloserPitcher))
            {
                skill = GenerateSkill(50, 81);
            } else
            {
                skill = GenerateSkill(10, 35);
            }

            return skill;
        }

        private int GenerateVelocitySkill(IEnumerable<Position> positions, int control)
        {
            int skill;

            if (positions.Contains(Position.StartingPitcher) || positions.Contains(Position.ReliefPitcher) || positions.Contains(Position.CloserPitcher))
            {
                if(control >= 80)
                {
                    skill = GenerateSkill(60, 81);
                } else
                {
                    skill = GenerateSkill(70, 91);
                }
            }
            else
            {
                skill = GenerateSkill(10, 35);
            }

            return skill;
        }

        private int GenerateMovement(IEnumerable<Position> positions, int control)
        {
            int skill;

            if (positions.Contains(Position.StartingPitcher) || positions.Contains(Position.ReliefPitcher) || positions.Contains(Position.CloserPitcher))
            {
                if (control >= 80)
                {
                    skill = GenerateSkill(70, 91);
                }
                else
                {
                    skill = GenerateSkill(60, 81);
                }
            }
            else
            {
                skill = GenerateSkill(10, 46);
            }

            return skill;
        }

        private int GenerateSkill(int min, int max)
        {
            return Random.Shared.Next(min, max);
        }
        #endregion

        // Skills
        // 18-22 ans : C'est l'âge où un joueur est en début de carrière, avec des compétences physiques plus élevées et un potentiel de développement important.
        // 23-27 ans : Les joueurs sont en pleine forme, avec un haut niveau de potentiel dans la plupart des compétences.
        // 28-32 ans : Compétences plus stables, avec des valeurs solides dans la plupart des domaines, mais certaines compétences (comme la vitesse ou stamina) peuvent commencer à décliner.
        // 33 ans et plus : Les joueurs commencent à perdre des capacités physiques, notamment la vitesse, la puissance et l'endurance. Le mental (compétences de pitching et batting) peut rester stable, voire augmenter avec l'expérience.
    }
}
