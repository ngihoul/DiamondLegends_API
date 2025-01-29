using DiamondLegends.BLL.Generators.Interfaces;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Generators
{
    public class GameSimulator
    {
        public bool GameOver { get; set; } = false;
        public int HalfInnings { get; set; } = 0;
        public int Strikes { get; set; } = 0;
        public int Balls { get; set; } = 0;
        public int NbOuts { get; set; } = 0;

        public int CurrentRuns { get; set; } = 0;
        public int RunsAway { get; set; } = 0;
        public int RunsHome { get; set; } = 0;

        public int CurrentHits { get; set; } = 0;
        public int HitsAway { get; set; } = 0;
        public int HitsHome { get; set; } = 0;


        Game Game = new Game();

        GameOffensiveStats[] Bases = new GameOffensiveStats[3] { null, null, null };

        List<GameOffensiveStats> AwayLineUp = new List<GameOffensiveStats>();
        List<GameOffensiveStats> HomeLineUp = new List<GameOffensiveStats>();

        GamePitchingStats HomeStartingPitcher = new GamePitchingStats();
        GamePitchingStats AwayStartingPitcher = new GamePitchingStats();

        List<GameOffensiveStats> Defense = new List<GameOffensiveStats>();
        List<GameOffensiveStats> Offense = new List<GameOffensiveStats>();

        GamePitchingStats CurrentPitcher = new GamePitchingStats();
        GameOffensiveStats CurrentHitter = null;

        GameOffensiveStats LastHitterHome = new GameOffensiveStats();
        GameOffensiveStats LastHitterAway = new GameOffensiveStats();

        GamePitchingStats LastPitcherHome = new GamePitchingStats();
        GamePitchingStats LastPitcherAway = new GamePitchingStats();

        public GameSimulator(Game game, List<GameOffensiveStats> offensiveLineUp, GamePitchingStats startingPitcher, List<GameOffensiveStats> opponentLineUp, GamePitchingStats opponentStartingPitcher)
        {
            Game = game;

            AwayLineUp = offensiveLineUp.First().Player.Team.Id == Game.Away.Id ? offensiveLineUp : opponentLineUp;
            HomeLineUp = offensiveLineUp.First().Player.Team.Id == Game.Home.Id ? offensiveLineUp : opponentLineUp;

            HomeStartingPitcher = startingPitcher.Player.Team.Id == Game.Home.Id ? startingPitcher : opponentStartingPitcher;
            AwayStartingPitcher = startingPitcher.Player.Team.Id == Game.Away.Id ? startingPitcher : opponentStartingPitcher;

            LastPitcherAway = AwayStartingPitcher;
            LastPitcherHome = HomeStartingPitcher;

            Defense = HomeLineUp;
            Offense = AwayLineUp;

            CurrentPitcher = HomeStartingPitcher;
            CurrentHitter = Offense[0];
            CurrentHitter.PA++;
        }

        // TODO: Handling pitching changes
        // TODO: Handling Stolen bases
        // TODO: Take in account Players' stats
        // TODO: AB, IP, E (instead of ER for pitchers)

        public Game Simulate()
        {
            while (!GameOver)
            {
                while (NbOuts < 3 && !GameOver)
                {
                    Pitch();
                }

                ChangeField();
            }

            Game.OffensiveStats = [.. Offense, .. Defense];
            Game.PitchingStats = [HomeStartingPitcher, AwayStartingPitcher];

            Game.AwayRuns = RunsAway;
            Game.HomeRuns = RunsHome;

            Game.AwayHits = HitsAway;
            Game.HomeHits = HitsHome;

            // Game.Status = Game.PLAYED;

            return Game;
        }

        private void NextHitter()
        {
            ResetCount();

            if (CurrentHitter is null)
            {
                CurrentHitter = Offense[0];
            }
            else
            {
                int positionCurrentHitter = Offense.FindIndex(p => p.Player.Id == CurrentHitter.Player.Id);

                if (positionCurrentHitter == 8)
                {
                    CurrentHitter = Offense[0];
                }
                else
                {
                    CurrentHitter = Offense[positionCurrentHitter + 1];
                }
            }

            CurrentHitter.PA++;
        }
        private void Pitch()
        {
            int randomPitch = Random.Shared.Next(0, 101);

            CurrentPitcher.NP++;

            if (randomPitch < 43)
            {
                Ball();
            }
            else if (randomPitch >= 44 && randomPitch <= 77)
            {
                Strike();
            }
            else if (randomPitch >= 78 && randomPitch <= 87)
            {
                FoulBall();
            }
            else
            {
                BallInPlay();
            }
        }
        private void Ball()
        {
            // Base on balls
            if (Balls == 3)
            {
                CurrentHitter.BB++;
                CurrentPitcher.BB++;

                if (Bases[0] is not null && Bases[1] is not null && Bases[2] is not null)
                {
                    // Run scores
                    Score();
                    Bases[2].R++;
                    CurrentHitter.RBI++;
                    CurrentPitcher.ER++;

                    // Runners move
                    Bases[2] = null;
                    Bases[2] = Bases[1];
                    Bases[1] = Bases[0];
                    Bases[0] = CurrentHitter;
                }
                else if (Bases[0] is not null && Bases[1] is not null)
                {
                    // Runners move
                    Bases[2] = Bases[1];
                    Bases[1] = Bases[0];
                    Bases[0] = CurrentHitter;
                }
                else if (Bases[0] is not null)
                {
                    // Runners move
                    Bases[1] = Bases[0];
                    Bases[0] = CurrentHitter;
                }

                NextHitter();
            }
            // Ball + 1
            else
            {
                Balls++;
            }
        }
        private void Strike()
        {
            if (Strikes == 2)
            {
                StrikeOut();
            }
            else
            {
                Strikes++;
            }
        }
        private void FoulBall()
        {
            if (Strikes < 2)
            {
                Strikes++;
            }
        }
        private void BallInPlay()
        {
            /*
             * According to 2024 stats
             * Single = 65%
             * Double = 19.5%
             * Triple = 0.1%
             * Homerun = 15.4%
             * https://www.baseball-reference.com/leagues/majors/bat.shtml
             */

            /*
            int randomHit = Random.Shared.Next(0, 101);

            if (randomHit < 66)
            {
                SingleHit();
            }
            else if (randomHit >= 66 && randomHit <= 84)
            {
                DoubleHit();
            }
            else if (randomHit >= 85 && randomHit <= 94)
            {
                TripleHit();
            }
            else
            {
                Homerun();
            }
            */

            /*
             * According to ChatGPT
             * Fly balls: Environ 35 - 40 %
             * Ground balls: Environ 43 - 47 %
             * Line drives: Environ 20 - 23 %
             */

            int randomHit = Random.Shared.Next(0, 101);

            if (randomHit <= 35)
            {
                FlyBall();
            }
            else if (randomHit >= 36 && randomHit <= 79)
            {
                GroundBall();
            }
            else
            {
                LineDrive();
            }
        }
        public void FlyBall()
        {
            //Fly Balls :

            //    Simples: Environ 15 %
            //    Doubles : Environ 20 %
            //    Triples : Environ 2 %
            //    Home Runs: Environ 15 %
            int randomFly = Random.Shared.Next(0, 101);

            if (randomFly <= 16)
            {
                SingleHit();
            }
            else if (randomFly >= 17 && randomFly <= 36)
            {
                DoubleHit();
            }
            else if (randomFly >= 37 && randomFly <= 38)
            {
                TripleHit();
            }
            else if (randomFly >= 39 && randomFly <= 54)
            {
                Homerun();
            }
            else
            {
                FlyOut();
            }
        }
        private void GroundBall()
        {
            //Ground Balls:

            //    Simples: Environ 23 %
            //    Doubles : Environ 3 %
            //    Triples : Moins de 1 %
            //    Home Runs: Pratiquement 0 %
            int randomGround = Random.Shared.Next(0, 101);

            if (randomGround <= 23)
            {
                SingleHit();
            }
            else if (randomGround >= 24 && randomGround <= 27)
            {
                DoubleHit();
            }
            else if (randomGround >= 28 && randomGround <= 29)
            {
                TripleHit();
            }
            else
            {
                GroundOut();
            }
        }
        private void LineDrive()
        {
            //Line Drives:

            //    Simples: Environ 70 %
            //    Doubles : Environ 20 %
            //    Triples : Environ 2 %
            //    Home Runs: Environ 6 %

            int randomLine = Random.Shared.Next(0, 101);

            if (randomLine <= 65)
            {
                SingleLine();
            }
            else if (randomLine >= 66 && randomLine <= 86)
            {
                DoubleLine();
            }
            else if (randomLine >= 87 && randomLine <= 89)
            {
                TripleLine();
            }
            else if (randomLine >= 90 && randomLine <= 95)
            {
                Homerun();
            }
            else
            {
                LineOut();
            }
        }
        private void StrikeOut()
        {
            CurrentHitter.SO++;
            CurrentPitcher.SO++;

            if (NbOuts < 2)
            {
                NextHitter();
            }

            NbOuts++;
        }
        private void SingleHit()
        {
            CurrentHitter.H++;
            CurrentPitcher.H++;

            CurrentHits++;

            Score();

            if (Bases[2] is not null)
            {
                Bases[2].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[2] = null;
            }

            if (Bases[1] is not null)
            {
                Bases[2] = Bases[1];
            }

            if (Bases[0] is not null)
            {
                Bases[1] = Bases[0];
            }

            Bases[0] = CurrentHitter;

            NextHitter();
        }
        private void DoubleHit()
        {
            CurrentHitter.Double++;
            CurrentPitcher.H++;

            CurrentHits++;

            if (Bases[2] is not null)
            {
                Bases[2].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[2] = null;
            }

            if (Bases[1] is not null)
            {
                Bases[1].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[1] = null;
            }

            if (Bases[0] is not null)
            {
                Bases[2] = Bases[0];
            }

            Bases[1] = CurrentHitter;

            NextHitter();
        }
        private void TripleHit()
        {
            CurrentHitter.Triple++;
            CurrentPitcher.H++;

            CurrentHits++;

            if (Bases[2] is not null)
            {
                Bases[2].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[2] = null;
            }

            if (Bases[1] is not null)
            {
                Bases[1].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[1] = null;
            }

            if (Bases[0] is not null)
            {
                Bases[0].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[0] = null;
            }

            Bases[2] = CurrentHitter;

            NextHitter();
        }
        private void FlyOut()
        {
            if (NbOuts < 2)
            {
                // Sac Fly
                if (Bases[2] is not null)
                {
                    Bases[2].R++;
                    Score();

                    CurrentHitter.RBI++;
                    CurrentPitcher.ER++;
                }
            }

            NbOuts++;

            if (NbOuts < 3)
            {
                NextHitter();
            }
        }
        private void GroundOut()
        {
            if (NbOuts < 2)
            {
                // Double play : 10 à 15%
                if (Bases[0] is not null)
                {
                    int randomDP = Random.Shared.Next(0, 101);

                    if (randomDP <= 15)
                    {
                        Bases[0] = null;
                        NbOuts++;
                    }
                }
            }

            NbOuts++;

            if (NbOuts < 3)
            {
                NextHitter();
            }
        }
        private void SingleLine()
        {
            CurrentHitter.H++;
            CurrentPitcher.H++;

            CurrentHits++;

            CurrentHits++;

            if (Bases[2] is not null)
            {
                Bases[2].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[2] = null;
            }

            if (Bases[1] is not null)
            {
                Bases[1].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[1] = null;
            }

            if (Bases[0] is not null)
            {
                Bases[2] = Bases[0];
            }

            Bases[0] = CurrentHitter;

            NextHitter();

        }
        private void DoubleLine()
        {
            CurrentHitter.Double++;
            CurrentPitcher.H++;

            CurrentHits++;

            if (Bases[2] is not null)
            {
                Bases[2].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[2] = null;
            }

            if (Bases[1] is not null)
            {
                Bases[1].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[1] = null;
            }

            if (Bases[0] is not null)
            {
                Bases[0].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[0] = null;
            }

            Bases[1] = CurrentHitter;

            NextHitter();
        }
        private void TripleLine()
        {
            CurrentHitter.Triple++;
            CurrentPitcher.H++;

            CurrentHits++;

            if (Bases[2] is not null)
            {
                Bases[2].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[2] = null;
            }

            if (Bases[1] is not null)
            {
                Bases[1].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[1] = null;
            }

            if (Bases[0] is not null)
            {
                Bases[0].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[0] = null;
            }

            Bases[2] = CurrentHitter;

            NextHitter();
        }
        private void LineOut()
        {
            if (NbOuts < 2)
            {
                // Double play : 5 a 10%
                if (Bases[0] is not null || Bases[1] is not null || Bases[2] is not null)
                {
                    int randomDP = Random.Shared.Next(0, 101);

                    if (Bases[0] is not null)
                    {
                        if (randomDP <= 8)
                        {
                            Bases[0] = null;
                            NbOuts++;
                        }
                    }
                    else if (Bases[1] is not null)
                    {
                        if (randomDP <= 8)
                        {
                            Bases[1] = null;
                            NbOuts++;
                        }
                    }
                    else if (Bases[2] is not null)
                    {
                        if (randomDP <= 8)
                        {
                            Bases[2] = null;
                            NbOuts++;
                        }
                    }
                }
            }

            NbOuts++;

            if (NbOuts < 3)
            {
                NextHitter();
            }
        }
        private void Homerun()
        {
            CurrentHitter.HR++;
            CurrentPitcher.HR++;

            CurrentHits++;

            if (Bases[2] is not null)
            {
                Bases[2].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[2] = null;
            }

            if (Bases[1] is not null)
            {
                Bases[1].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[1] = null;
            }

            if (Bases[0] is not null)
            {
                Bases[0].R++;
                Score();

                CurrentHitter.RBI++;
                CurrentPitcher.ER++;

                Bases[0] = null;
            }

            CurrentHitter.RBI++;

            CurrentHitter.R++;
            Score();

            CurrentPitcher.R++;

            NextHitter();
        }
        private void Score()
        {
            CurrentRuns++;

            if(isWalkOff()) {
                GameOver = true;
            }
        }
        private void ResetCount()
        {
            Balls = 0;
            Strikes = 0;
        }

        private bool isWalkOff()
        {
            if (Offense == HomeLineUp && HalfInnings >= 18 && RunsHome > RunsAway)
            {
                return true;
            }

            return false;
        }

        private void ChangeField()
        {
            ResetCount();
            NbOuts = 0;
            HalfInnings++;

            // Rotate teams
            if (Offense == HomeLineUp)
            {
                (Offense, Defense) = (Defense, Offense);

                LastHitterHome = CurrentHitter;
                LastPitcherAway = CurrentPitcher;

                CurrentPitcher = LastPitcherHome;

                int positionNextHitter = AwayLineUp.FindIndex(p => p.Player.Id == LastHitterAway.Player.Id) > -1 ? Offense.FindIndex(p => p.Player.Id == LastHitterAway.Player.Id) + 1 : 0;

                positionNextHitter = positionNextHitter > 8 ? 0 : positionNextHitter;

                CurrentHitter = Offense[positionNextHitter];

                RunsHome += CurrentRuns;
                CurrentRuns = 0;

                HitsHome += CurrentHits;
                CurrentHits = 0;
            }
            else
            {
                (Offense, Defense) = (Defense, Offense);

                LastHitterAway = CurrentHitter;
                LastPitcherHome = CurrentPitcher;

                CurrentPitcher = LastPitcherAway;

                int positionNextHitter = Offense.FindIndex(p => p.Player.Id == LastHitterAway.Player.Id) > -1 ? Offense.FindIndex(p => p.Player.Id == LastHitterAway.Player.Id) + 1 : 0;

                positionNextHitter = positionNextHitter > 8 ? 0 : positionNextHitter;

                CurrentHitter = Offense[positionNextHitter];

                RunsAway += CurrentRuns;
                CurrentRuns = 0;

                HitsAway += CurrentHits;
                CurrentHits = 0;
            }

            // Check if game is over
            if ((HalfInnings > 18 && RunsHome != RunsAway) || isWalkOff())
            {
                GameOver = true;
            }
        }
    }
}


