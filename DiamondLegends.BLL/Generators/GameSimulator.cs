using DiamondLegends.DAL.Interfaces;
using DiamondLegends.Domain.Models;

namespace DiamondLegends.BLL.Generators
{
    public class GameSimulator
    {
        #region Dependencies
        private readonly IGameRepository _gameRepository;
        #endregion

        #region Properties
        private bool _gameOver = false;
        private int _halfInnings = 0;
        private int _strikes = 0;
        private int _balls = 0;
        private int _nbOuts = 0;

        private int _currentRuns = 0;
        private int _runsAway = 0;
        private int _runsHome = 0;

        private int _currentHits = 0;
        private int _hitsAway = 0;
        private int _hitsHome = 0;

        private Game _game = new Game();

        private GameOffensiveStats?[] _bases = { null, null, null };

        private readonly List<GameOffensiveStats> _awayLineUp = new List<GameOffensiveStats>();
        private readonly List<GameOffensiveStats> _homeLineUp = new List<GameOffensiveStats>();

        private readonly GamePitchingStats _homeStartingPitcher = new GamePitchingStats();
        private readonly GamePitchingStats _awayStartingPitcher = new GamePitchingStats();

        private List<GameOffensiveStats> _defense = new List<GameOffensiveStats>();
        private List<GameOffensiveStats> _offense = new List<GameOffensiveStats>();

        private GamePitchingStats _currentPitcher = new GamePitchingStats();
        private GameOffensiveStats _currentHitter = new GameOffensiveStats();

        private GameOffensiveStats? _lastHitterHome = null;
        private GameOffensiveStats? _lastHitterAway = null;

        private GamePitchingStats _lastPitcherHome = new GamePitchingStats();
        private GamePitchingStats _lastPitcherAway = new GamePitchingStats();
        #endregion

        #region Constructor
        public GameSimulator(Game game, List<GameOffensiveStats> offensiveLineUp, GamePitchingStats startingPitcher, List<GameOffensiveStats> opponentLineUp, GamePitchingStats opponentStartingPitcher, IGameRepository gameRepository)
        {
            // Dependencies
            _gameRepository = gameRepository;

            // Game
            _game = game;

            _awayLineUp = offensiveLineUp.First().Player.Team.Id == _game.Away.Id ? offensiveLineUp : opponentLineUp;
            _homeLineUp = offensiveLineUp.First().Player.Team.Id == _game.Home.Id ? offensiveLineUp : opponentLineUp;

            _homeStartingPitcher = startingPitcher.Player.Team.Id == _game.Home.Id ? startingPitcher : opponentStartingPitcher;
            _awayStartingPitcher = startingPitcher.Player.Team.Id == _game.Away.Id ? startingPitcher : opponentStartingPitcher;

            _lastPitcherAway = _awayStartingPitcher;
            _lastPitcherAway.G++;
            _lastPitcherAway.GS++;

            _lastPitcherHome = _homeStartingPitcher;
            _lastPitcherHome.G++;
            _lastPitcherHome.GS++;

            _defense = _homeLineUp;
            _offense = _awayLineUp;

            _currentPitcher = _homeStartingPitcher;

            _currentHitter = _offense[0];
            _currentHitter.PA++;
        }
        #endregion
        // TODO: Handling pitchers' changes
        // TODO: Handling Stolen bases
        // TODO: Take in account Players' stats
        // TODO: Defensive Errors
        // TODO:
        // HBP to Hitters
        // 

        #region Methods
        public async Task<Game> Simulate()
        {
            while (!_gameOver)
            {
                while (_nbOuts < 3 && !_gameOver)
                {
                    Pitch();
                }

                ChangeField();
            }

            _game.OffensiveStats = [.. _offense, .. _defense];
            _game.PitchingStats = [_homeStartingPitcher, _awayStartingPitcher];

            _game.HalfInnings = _halfInnings;

            _game.AwayRuns = _runsAway;
            _game.HomeRuns = _runsHome;

            _game.AwayHits = _hitsAway;
            _game.HomeHits = _hitsHome;

            _game.Status = Game.PLAYED;

            // Update Game & Save Stats with transaction
            _game = await _gameRepository.Update(_game);

            return _game;
        }

        private void NextHitter()
        {
            ResetCount();

            if (_currentHitter is null)
            {
                _currentHitter = _offense[0];
            }
            else
            {
                int positionCurrentHitter = _offense.FindIndex(p => p.Player.Id == _currentHitter.Player.Id);

                if (positionCurrentHitter == 8)
                {
                    _currentHitter = _offense[0];
                }
                else
                {
                    _currentHitter = _offense[positionCurrentHitter + 1];
                }
            }

            _currentHitter.PA++;
        }

        private void Pitch()
        {
            int randomPitch = Random.Shared.Next(0, 101);

            _currentPitcher.NP++;

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
            else if (randomPitch >= 88 && randomPitch <= 89)
            {
                HitByPitch();
            }
            else
            {
                BallInPlay();
            }
        }

        private void Ball()
        {
            // Base on balls
            if (_balls == 3)
            {
                _currentHitter.BB++;
                _currentPitcher.BB++;

                if (_bases[0] is not null && _bases[1] is not null && _bases[2] is not null)
                {
                    // Run scores
                    Score(_bases[2]);

                    // Runners move
                    _bases[2] = null;
                    _bases[2] = _bases[1];
                    _bases[1] = _bases[0];
                }
                else if (_bases[0] is not null && _bases[1] is not null)
                {
                    // Runners move
                    _bases[2] = _bases[1];
                    _bases[1] = _bases[0];
                }
                else if (_bases[0] is not null)
                {
                    // Runners move
                    _bases[1] = _bases[0];
                }

                _bases[0] = _currentHitter;

                NextHitter();
            }
            // Ball + 1
            else
            {
                _balls++;
            }
        }

        private void Strike()
        {
            if (_strikes == 2)
            {
                StrikeOut();
            }
            else
            {
                _strikes++;
            }
        }

        private void FoulBall()
        {
            if (_strikes < 2)
            {
                _strikes++;
            }
        }

        private void HitByPitch()
        {
            _currentPitcher.HB++;

            // TODO : add HBP to hitter
            if (_bases[0] is not null && _bases[1] is not null && _bases[2] is not null)
            {
                // Run scores
                Score(_bases[2]);

                // Runners move
                _bases[2] = null;
                _bases[2] = _bases[1];
                _bases[1] = _bases[0];
            }
            else if (_bases[0] is not null && _bases[1] is not null)
            {
                // Runners move
                _bases[2] = _bases[1];
                _bases[1] = _bases[0];
            }
            else if (_bases[0] is not null)
            {
                // Runners move
                _bases[1] = _bases[0];
            }

            _bases[0] = _currentHitter;

            NextHitter();
        }

        private void BallInPlay()
        {
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

            _currentHitter.AB++;

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

            _currentHitter.AB++;

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

            _currentHitter.AB++;

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
            _currentHitter.SO++;
            _currentHitter.AB++;

            _currentPitcher.SO++;

            if (_nbOuts < 2)
            {
                NextHitter();
            }

            _nbOuts++;
        }

        private void SingleHit()
        {
            _currentHitter.H++;
            _currentPitcher.H++;

            _currentHits++;

            if (_bases[2] is not null)
            {
                Score(_bases[2]);

                _bases[2] = null;
            }

            if (_bases[1] is not null)
            {
                _bases[2] = _bases[1];
            }

            if (_bases[0] is not null)
            {
                _bases[1] = _bases[0];
            }

            _bases[0] = _currentHitter;

            NextHitter();
        }

        private void DoubleHit()
        {
            _currentHitter.Double++;
            _currentPitcher.H++;

            _currentHits++;

            if (_bases[2] is not null)
            {
                Score(_bases[2]);

                _bases[2] = null;
            }

            if (_bases[1] is not null)
            {
                Score(_bases[1]);

                _bases[1] = null;
            }

            if (_bases[0] is not null)
            {
                _bases[2] = _bases[0];
            }

            _bases[1] = _currentHitter;

            NextHitter();
        }

        private void TripleHit()
        {
            _currentHitter.Triple++;
            _currentPitcher.H++;

            _currentHits++;

            if (_bases[2] is not null)
            {
                Score(_bases[2]);

                _bases[2] = null;
            }

            if (_bases[1] is not null)
            {
                Score(_bases[1]);

                _bases[1] = null;
            }

            if (_bases[0] is not null)
            {
                Score(_bases[0]);

                _bases[0] = null;
            }

            _bases[2] = _currentHitter;

            NextHitter();
        }

        private void FlyOut()
        {
            if (_nbOuts < 2)
            {
                // Sac Fly
                if (_bases[2] is not null)
                {
                    Score(_bases[2]);
                }
            }

            _nbOuts++;

            if (_nbOuts < 3)
            {
                NextHitter();
            }
        }

        private void GroundOut()
        {
            if (_nbOuts < 2)
            {
                // Double play : 10 à 15%
                if (_bases[0] is not null)
                {
                    int randomDP = Random.Shared.Next(0, 101);

                    if (randomDP <= 15)
                    {
                        _bases[0] = null;
                        _nbOuts++;
                    }
                }
            }

            _nbOuts++;

            if (_nbOuts < 3)
            {
                NextHitter();
            }
        }

        private void SingleLine()
        {
            _currentHitter.H++;
            _currentPitcher.H++;

            _currentHits++;

            if (_bases[2] is not null)
            {
                Score(_bases[2]);

                _bases[2] = null;
            }

            if (_bases[1] is not null)
            {
                Score(_bases[1]);

                _bases[1] = null;
            }

            if (_bases[0] is not null)
            {
                _bases[2] = _bases[0];
            }

            _bases[0] = _currentHitter;

            NextHitter();

        }

        private void DoubleLine()
        {
            _currentHitter.Double++;
            _currentPitcher.H++;

            _currentHits++;

            if (_bases[2] is not null)
            {
                Score(_bases[2]);

                _bases[2] = null;
            }

            if (_bases[1] is not null)
            {
                Score(_bases[1]);

                _bases[1] = null;
            }

            if (_bases[0] is not null)
            {
                Score(_bases[0]);

                _bases[0] = null;
            }

            _bases[1] = _currentHitter;

            NextHitter();
        }

        private void TripleLine()
        {
            _currentHitter.Triple++;
            _currentPitcher.H++;

            _currentHits++;

            if (_bases[2] is not null)
            {
                Score(_bases[2]);

                _bases[2] = null;
            }

            if (_bases[1] is not null)
            {
                Score(_bases[1]);

                _bases[1] = null;
            }

            if (_bases[0] is not null)
            {
                Score(_bases[0]);

                _bases[0] = null;
            }

            _bases[2] = _currentHitter;

            NextHitter();
        }

        private void LineOut()
        {
            if (_nbOuts < 2)
            {
                // Double play : 5 a 10%
                if (_bases[0] is not null || _bases[1] is not null || _bases[2] is not null)
                {
                    int randomDP = Random.Shared.Next(0, 101);

                    if (_bases[0] is not null)
                    {
                        if (randomDP <= 8)
                        {
                            _bases[0] = null;
                            _nbOuts++;
                        }
                    }
                    else if (_bases[1] is not null)
                    {
                        if (randomDP <= 8)
                        {
                            _bases[1] = null;
                            _nbOuts++;
                        }
                    }
                    else if (_bases[2] is not null)
                    {
                        if (randomDP <= 8)
                        {
                            _bases[2] = null;
                            _nbOuts++;
                        }
                    }
                }
            }

            _nbOuts++;

            if (_nbOuts < 3)
            {
                NextHitter();
            }
        }

        private void Homerun()
        {
            _currentHitter.HR++;
            _currentHitter.H++;

            _currentPitcher.HR++;

            _currentHits++;

            if (_bases[2] is not null)
            {
                Score(_bases[2]);

                _bases[2] = null;
            }

            if (_bases[1] is not null)
            {
                Score(_bases[1]);

                _bases[1] = null;
            }

            if (_bases[0] is not null)
            {
                Score(_bases[0]);

                _bases[0] = null;
            }

            Score(_currentHitter);

            NextHitter();
        }

        private void Score(GameOffensiveStats scorer)
        {

            _currentRuns++;
            _currentHitter.RBI++;
            _currentPitcher.R++;
            scorer.R++;

            if (isWalkOff())
            {
                _gameOver = true;
            }
        }

        private void ResetCount()
        {
            _balls = 0;
            _strikes = 0;
        }

        private bool isWalkOff()

        {
            if (_offense == _homeLineUp && _halfInnings >= 16 && _runsHome > _runsAway)
            {
                return true;
            }

            return false;
        }

        private void ChangeField()
        {
            _currentPitcher.IP++;

            UpdatingPlayersStats();

            // Check if game is over
            if ((_halfInnings >= 17 && _runsHome != _runsAway))
            {
                _gameOver = true;
            }
            else
            {
                ResetCount();

                _nbOuts = 0;
                _bases = [null, null, null];

                _halfInnings++;

                // Rotate teams
                if (_offense == _homeLineUp)
                {
                    _lastHitterHome = _currentHitter;
                    _lastPitcherAway = _currentPitcher;

                    _currentPitcher = _lastPitcherHome;

                    int positionNextHitter = _lastHitterHome is not null && _awayLineUp.FindIndex(p => p.Player.Id == _lastHitterAway.Player.Id) > -1 ? _awayLineUp.FindIndex(p => p.Player.Id == _lastHitterAway.Player.Id) + 1 : 0;

                    positionNextHitter = positionNextHitter > 8 ? 0 : positionNextHitter;

                    _currentHitter = _awayLineUp[positionNextHitter];
                    _currentHitter.PA++;

                    _runsHome += _currentRuns;
                    _currentRuns = 0;

                    _hitsHome += _currentHits;
                    _currentHits = 0;
                }
                else
                {
                    _lastHitterAway = _currentHitter;
                    _lastPitcherHome = _currentPitcher;

                    _currentPitcher = _lastPitcherAway;

                    int positionNextHitter = _lastHitterHome is not null && _homeLineUp.FindIndex(p => p.Player.Id == _lastHitterHome.Player.Id) > -1 ? _homeLineUp.FindIndex(p => p.Player.Id == _lastHitterHome.Player.Id) + 1 : 0;

                    positionNextHitter = positionNextHitter > 8 ? 0 : positionNextHitter;

                    _currentHitter = _homeLineUp[positionNextHitter];
                    _currentHitter.PA++;

                    _runsAway += _currentRuns;
                    _currentRuns = 0;

                    _hitsAway += _currentHits;
                    _currentHits = 0;
                }

                (_offense, _defense) = (_defense, _offense);
                
                // Check if last home offense is necessary
                if(isWalkOff())
                {
                    _gameOver = true;
                }
            }
        }

        private void UpdatingPlayersStats()
        {
            // Calculate offense players' stats
            foreach (GameOffensiveStats player in _offense)
            {
                player.AVG = (decimal)player.AB > 0 ? Math.Round(((decimal)player.H + (decimal)player.Double + (decimal)player.Triple + (decimal)player.HR) / (decimal)player.AB, 3) : 0;

                // OBP = (Hits + Walks + Hit by Pitch) ÷ (At Bats + Walks + Hit by Pitch + Sacrifice Flies)
                player.OBP = ((decimal)player.AB + (decimal)player.BB) > 0 ? Math.Round((((decimal)player.H + (decimal)player.Double + (decimal)player.Triple + (decimal)player.HR + (decimal)player.BB) / ((decimal)player.AB + (decimal)player.BB)), 3) : 0;

                // 1B + 2Bx2 + 3Bx3 + HRx4)/ AB
                player.SLG = (decimal)player.AB > 0 ? Math.Round(((decimal)player.H + ((decimal)player.Double * 2) + ((decimal)player.Triple * 3) + ((decimal)player.HR * 4)) / (decimal)player.AB, 3) : 0;

                // SLG + OBP
                player.OPS = Math.Round((decimal)player.SLG + (decimal)player.OBP, 3);
            }

            // Calculate pitcher stat
            _currentPitcher.ERA = _currentPitcher.IP > 0 ? Math.Round((decimal)_currentPitcher.R / (decimal)_currentPitcher.IP * 9, 3) : 0;

            // TODO : adding AB to pitchers' stats
            // _currentPitcher.AVG = _currentPitcher.AB > 0 ? Math.Round((decimal)_currentPitcher.H / (decimal)_currentPitcher.AB, 3) : 0;

            _currentPitcher.WHIP = _currentPitcher.IP > 0 ? Math.Round(((decimal)_currentPitcher.H + (decimal)_currentPitcher.BB) / (decimal)_currentPitcher.IP, 3) : 0;

            _currentPitcher.CG = _currentPitcher.IP == 9 ? 1 : 0;
        }
        #endregion
    }
}


