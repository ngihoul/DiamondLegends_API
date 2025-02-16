﻿using DiamondLegends.BLL.Services.Interfaces;
using DiamondLegends.DAL.Repositories.Interfaces;
using DiamondLegends.Domain.Models;
using System.Collections.ObjectModel;

namespace DiamondLegends.BLL.Generators
{
    public class GameSimulator
    {
        #region Dependencies
        private readonly IGameRepository _gameRepository;
        #endregion

        #region Properties
        private bool _playByPlay = false;

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

        private ObservableCollection<GameEvent> _events = new ObservableCollection<GameEvent>();

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
        public GameSimulator(Game game, List<GameOffensiveStats> offensiveLineUp, GamePitchingStats startingPitcher, List<GameOffensiveStats> opponentLineUp, GamePitchingStats opponentStartingPitcher, bool playByPlay, IGameRepository gameRepository)
        {
            // Dependencies
            _gameRepository = gameRepository;

            // Game
            _playByPlay = playByPlay;

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
        // Adding stats HBP to Hitters
        // 

        #region Methods
        public ObservableCollection<GameEvent> GetEvents()
        {
            return _events;
        }

        public async Task<Game> Simulate()
        {
            if (_playByPlay)
            {
                _events.Add(CreateGameEvent($"Le match entre {_game.Away.Name} et {_game.Home.Name} commence."));
            }

            while (!_gameOver)
            {
                while (_nbOuts < 3 && !_gameOver)
                {
                    
                    await Pitch();

                    if(_playByPlay)
                    {
                        await Task.Delay(3000);
                    }
                }

                await ChangeField();
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

            if (_playByPlay)
            {
               _events.Add(CreateGameEvent($"Le match entre {_game.Away.Name} et {_game.Home.Name} se termine. Victoire de {(_runsHome > _runsAway ? _game.Home.Name : _game.Away.Name)} sur un score de {_runsAway} - {_runsHome} !"));
            }

            return _game;
        }

        private async Task NextHitter()
        {
            await ResetCount();

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

        private async Task Pitch()
        {
            int randomPitch = Random.Shared.Next(0, 101);

            if (_playByPlay)
            {
                _events.Add(CreateGameEvent($"{_currentPitcher.Player.Firstname.Substring(0, 1)}. {_currentPitcher.Player.Lastname} lance ..."));
            }

            _currentPitcher.NP++;

            if (randomPitch < 43)
            {
                await Ball();
            }
            else if (randomPitch >= 44 && randomPitch <= 77)
            {
                await Strike();
            }
            else if (randomPitch >= 78 && randomPitch <= 87)
            {
                await FoulBall();
            }
            else if (randomPitch >= 88 && randomPitch <= 89)
            {
                await HitByPitch();
            }
            else
            {
                await BallInPlay();
            }
        }

        private async Task Ball()
        {
            // Base on balls
            if (_balls == 3)
            {
                if (_playByPlay)
                {
                    _events.Add(CreateGameEvent($"Base on balls pour {_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname}. Il avance en 1e base."));
                }

                _currentHitter.BB++;
                _currentPitcher.BB++;

                if (_bases[0] is not null && _bases[1] is not null && _bases[2] is not null)
                {
                    // Run scores
                    await Score(_bases[2]);

                    // Runners move
                    _bases[2] = null;
                    _bases[2] = _bases[1];

                    if (_playByPlay)
                    {
                        _events.Add(CreateGameEvent($"{_bases[2].Player.Firstname.Substring(0, 1)} {_bases[2].Player.Lastname} avance en 3e base."));
                    }

                    _bases[1] = _bases[0];

                    if (_playByPlay)
                    {
                        _events.Add(CreateGameEvent($"{_bases[0].Player.Firstname.Substring(0, 1)} {_bases[0].Player.Lastname} avance en 2e base."));
                    }
                }
                else if (_bases[0] is not null && _bases[1] is not null)
                {
                    // Runners move
                    _bases[2] = _bases[1];

                    if (_playByPlay)
                    {
                        _events.Add(CreateGameEvent($"{_bases[2].Player.Firstname.Substring(0, 1)} {_bases[2].Player.Lastname} avance en 3e base."));
                    }

                    _bases[1] = _bases[0];

                    if (_playByPlay)
                    {
                        _events.Add(CreateGameEvent($"{_bases[0].Player.Firstname.Substring(0, 1)} {_bases[0].Player.Lastname} avance en 2e base."));
                    }
                }
                else if (_bases[0] is not null)
                {
                    // Runners move
                    _bases[1] = _bases[0];

                    if (_playByPlay)
                    {
                        _events.Add(CreateGameEvent($"{_bases[0].Player.Firstname.Substring(0, 1)} {_bases[0].Player.Lastname} avance en 2e base."));
                    }
                }

                _bases[0] = _currentHitter;

                await NextHitter();
            }
            // Ball + 1
            else
            {
                _balls++;

                if(_playByPlay)
                {
                    _events.Add(CreateGameEvent($"Ball {_balls} !"));
                }
            }
        }

        private async Task Strike()
        {
            if (_strikes == 2)
            {
                await StrikeOut();
            }
            else
            {
                _strikes++;

                if(_playByPlay)
                {
                    _events.Add(CreateGameEvent($"Strike {_strikes}"));
                }
            }
        }

        private async Task FoulBall()
        {
            if (_strikes < 2)
            {
                _strikes++;
            }

            if (_playByPlay)
            {
                _events.Add(CreateGameEvent($"{_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} frappe en fausse balle."));
            }
        }

        private async Task HitByPitch()
        {
            if (_playByPlay)
            {
                _events.Add(CreateGameEvent($"{_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} est touché. Il avance en 1e base."));
            }

            _currentPitcher.HB++;

            // TODO : add HBP to hitter
            if (_bases[0] is not null && _bases[1] is not null && _bases[2] is not null)
            {
                // Run scores
                await Score(_bases[2]);

                // Runners move
                _bases[2] = null;
                _bases[2] = _bases[1];

                if (_playByPlay)
                {
                    _events.Add(CreateGameEvent($"{_bases[2].Player.Firstname.Substring(0, 1)} {_bases[2].Player.Lastname} avance en 3e base."));
                }

                _bases[1] = _bases[0];

                if (_playByPlay)
                {
                    _events.Add(CreateGameEvent($"{_bases[1].Player.Firstname.Substring(0, 1)} {_bases[1].Player.Lastname} avance en 2e base."));
                }
            }
            else if (_bases[0] is not null && _bases[1] is not null)
            {
                // Runners move
                _bases[2] = _bases[1];

                if (_playByPlay)
                {
                    _events.Add(CreateGameEvent($"{_bases[2].Player.Firstname.Substring(0, 1)} {_bases[2].Player.Lastname} avance en 3e base."));
                }

                _bases[1] = _bases[0];

                if (_playByPlay)
                {
                    _events.Add(CreateGameEvent($"{_bases[1].Player.Firstname.Substring(0, 1)} {_bases[1].Player.Lastname} avance en 2e base."));
                }
            }
            else if (_bases[0] is not null)
            {
                // Runners move
                _bases[1] = _bases[0];

                if (_playByPlay)
                {
                    _events.Add(CreateGameEvent($"{_bases[1].Player.Firstname.Substring(0, 1)} {_bases[1].Player.Lastname} avance en 2e base."));
                }
            }

            _bases[0] = _currentHitter;

            await NextHitter();
        }

        private async Task BallInPlay()
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
                await FlyBall();
            }
            else if (randomHit >= 36 && randomHit <= 79)
            {
                await GroundBall();
            }
            else
            {
                await LineDrive();
            }
        }

        public async Task FlyBall()
        {
            //Fly Balls :

            //    Simples: Environ 15 %
            //    Doubles : Environ 20 %
            //    Triples : Environ 2 %
            //    Home Runs: Environ 15 %

            _currentHitter.AB++;

            int randomFly = Random.Shared.Next(0, 101);

            if (_playByPlay)
            {
                _events.Add(CreateGameEvent($"{_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} frappe en l'air ..."));
            }

            if (randomFly <= 16)
            {
                await SingleHit();
            }
            else if (randomFly >= 17 && randomFly <= 36)
            {
                await DoubleHit();
            }
            else if (randomFly >= 37 && randomFly <= 38)
            {
                await TripleHit();
            }
            else if (randomFly >= 39 && randomFly <= 54)
            {
                await Homerun();
            }
            else
            {
                await FlyOut();
            }
        }

        private async Task GroundBall()
        {
            //Ground Balls:

            //    Simples: Environ 23 %
            //    Doubles : Environ 3 %
            //    Triples : Moins de 1 %
            //    Home Runs: Pratiquement 0 %

            _currentHitter.AB++;

            if (_playByPlay)
            {
                _events.Add(CreateGameEvent($"{_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} frappe au sol ..."));
            }

            int randomGround = Random.Shared.Next(0, 101);

            if (randomGround <= 23)
            {
                await SingleHit();
            }
            else if (randomGround >= 24 && randomGround <= 27)
            {
                await DoubleHit();
            }
            else if (randomGround >= 28 && randomGround <= 29)
            {
                await TripleHit();
            }
            else
            {
                await GroundOut();
            }
        }

        private async Task LineDrive()
        {
            //Line Drives:

            //    Simples: Environ 70 %
            //    Doubles : Environ 20 %
            //    Triples : Environ 2 %
            //    Home Runs: Environ 6 %

            _currentHitter.AB++;

            if (_playByPlay)
            {
                _events.Add(CreateGameEvent($"{_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} frappe une line drive ..."));
            }

            int randomLine = Random.Shared.Next(0, 101);

            if (randomLine <= 65)
            {
                await SingleLine();
            }
            else if (randomLine >= 66 && randomLine <= 86)
            {
                await DoubleLine();
            }
            else if (randomLine >= 87 && randomLine <= 89)
            {
                await TripleLine();
            }
            else if (randomLine >= 90 && randomLine <= 95)
            {
                await Homerun();
            }
            else
            {
                await LineOut();
            }
        }

        private async Task StrikeOut()
        {
            _currentHitter.SO++;
            _currentHitter.AB++;

            _currentPitcher.SO++;

            if (_playByPlay)
            {
                _events.Add(CreateGameEvent($"{_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} est strike out : {_nbOuts + 1} out{(_nbOuts + 1 > 1 ? "s" : "")}"));
            }

            if (_nbOuts < 2)
            {
                await NextHitter();
            }

            _nbOuts++;
        }

        private async Task SingleHit()
        {
            _currentHitter.H++;
            _currentPitcher.H++;

            if (_playByPlay)
            {
                _events.Add(CreateGameEvent($"{_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} frappe un simple !"));
            }

            _currentHits++;

            if (_bases[2] is not null)
            {
                await Score(_bases[2]);

                _bases[2] = null;
            }

            if (_bases[1] is not null)
            {
                _bases[2] = _bases[1];

                if (_playByPlay)
                {
                    _events.Add(CreateGameEvent($"{_bases[2].Player.Firstname.Substring(0, 1)} {_bases[2].Player.Lastname} avance en 3e base."));
                }
            }

            if (_bases[0] is not null)
            {
                _bases[1] = _bases[0];

                if (_playByPlay)
                {
                    _events.Add(CreateGameEvent($"{_bases[1].Player.Firstname.Substring(0, 1)} {_bases[1].Player.Lastname} avance en 2e base."));
                }
            }

            _bases[0] = _currentHitter;

            await NextHitter();
        }

        private async Task DoubleHit()
        {
            _currentHitter.Double++;
            _currentPitcher.H++;

            if (_playByPlay)
            {
                _events.Add(CreateGameEvent($"{_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} frappe un double. Il avance en 2e base."));
            }

            _currentHits++;

            if (_bases[2] is not null)
            {
                await Score(_bases[2]);

                _bases[2] = null;
            }

            if (_bases[1] is not null)
            {
                await Score(_bases[1]);

                _bases[1] = null;
            }

            if (_bases[0] is not null)
            {
                _bases[2] = _bases[0];
                _bases[0] = null;

                if (_playByPlay)
                {
                    _events.Add(CreateGameEvent($"{_bases[2].Player.Firstname.Substring(0, 1)} {_bases[2].Player.Lastname} avance en 3e base."));
                }
            }

            _bases[1] = _currentHitter;

            await NextHitter();
        }

        private async Task TripleHit()
        {
            _currentHitter.Triple++;
            _currentPitcher.H++;

            _currentHits++;

            if (_playByPlay)
            {
                _events.Add(CreateGameEvent($"{_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} frappe un triple. Il avance en 3e base."));
            }

            if (_bases[2] is not null)
            {
                await Score(_bases[2]);

                _bases[2] = null;
            }

            if (_bases[1] is not null)
            {
                await Score(_bases[1]);

                _bases[1] = null;
            }

            if (_bases[0] is not null)
            {
                await Score(_bases[0]);

                _bases[0] = null;
            }

            _bases[2] = _currentHitter;

            await NextHitter();
        }

        private async Task FlyOut()
        {
            if (_playByPlay)
            {
                _events.Add(CreateGameEvent($"La balle est attrapée en l'air. {_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} est éliminé."));
            }

            if (_nbOuts < 2)
            {
                // Sac Fly
                if (_bases[2] is not null)
                {
                    if (_playByPlay)
                    {
                        _events.Add(CreateGameEvent($"{_bases[2].Player.Firstname.Substring(0, 1)} {_bases[2].Player.Lastname} tente sa chance et cours vers la marbre ..."));
                    }

                    await Score(_bases[2]);

                    _bases[2] = null;
                }
            }

            _nbOuts++;

            if (_nbOuts < 3)
            {
                await NextHitter();
            }
        }

        private async Task GroundOut()
        {
            bool isDoublePlay = false;

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

                        if (_playByPlay)
                        {
                            _events.Add(CreateGameEvent("Un double jeu est tourné ! Wouaw !!!!"));
                        }

                        isDoublePlay = true;
                    }
                }
            }

            _nbOuts++;

            if(!isDoublePlay && _playByPlay)
            {
                if (_playByPlay)
                {
                    _events.Add(CreateGameEvent($"{_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} est éliminé en 1e base."));
                }
            }

            if (_nbOuts < 3)
            {
                await NextHitter();
            }
        }

        private async Task SingleLine()
        {
            _currentHitter.H++;
            _currentPitcher.H++;

            if (_playByPlay)
            {
                _events.Add(CreateGameEvent($"{_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} frappe un simple. Il avance en 1e base."));
            }

            _currentHits++;

            if (_bases[2] is not null)
            {
                await Score(_bases[2]);

                _bases[2] = null;
            }

            if (_bases[1] is not null)
            {
                await Score(_bases[1]);

                _bases[1] = null;
            }

            if (_bases[0] is not null)
            {
                _bases[2] = _bases[0];

                if (_playByPlay)
                {
                    _events.Add(CreateGameEvent($"{_bases[2].Player.Firstname.Substring(0, 1)} {_bases[2].Player.Lastname} avance en 3e base."));
                }
            }

            _bases[0] = _currentHitter;

            await NextHitter();

        }

        private async Task DoubleLine()
        {
            _currentHitter.Double++;
            _currentPitcher.H++;

            if (_playByPlay)
            {
                _events.Add(CreateGameEvent($"{_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} frappe un double. Il avance en 2e base."));
            }

            _currentHits++;

            if (_bases[2] is not null)
            {
                await Score(_bases[2]);

                _bases[2] = null;
            }

            if (_bases[1] is not null)
            {
                await Score(_bases[1]);

                _bases[1] = null;
            }

            if (_bases[0] is not null)
            {
                await Score(_bases[0]);

                _bases[0] = null;
            }

            _bases[1] = _currentHitter;

            await NextHitter();
        }

        private async Task TripleLine()
        {
            _currentHitter.Triple++;
            _currentPitcher.H++;

            if (_playByPlay)
            {
                _events.Add(CreateGameEvent($"{_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} frappe un triple. Il avance en 3e base."));
            }

            _currentHits++;

            if (_bases[2] is not null)
            {
                await Score(_bases[2]);

                _bases[2] = null;
            }

            if (_bases[1] is not null)
            {
                await Score(_bases[1]);

                _bases[1] = null;
            }

            if (_bases[0] is not null)
            {
                await Score(_bases[0]);

                _bases[0] = null;
            }

            _bases[2] = _currentHitter;

            await NextHitter();
        }

        private async Task LineOut()
        {
            bool isDoublePlay = false;

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
                            isDoublePlay = true;
                        }
                    }
                    else if (_bases[1] is not null)
                    {
                        if (randomDP <= 8)
                        {
                            _bases[1] = null;
                            _nbOuts++;
                            isDoublePlay = true;
                        }
                    }
                    else if (_bases[2] is not null)
                    {
                        if (randomDP <= 8)
                        {
                            _bases[2] = null;
                            _nbOuts++;
                            isDoublePlay = true;
                        }
                    }
                }
            }

            if(_playByPlay)
            {
                if(!isDoublePlay)
                {
                    if (_playByPlay)
                    {
                        _events.Add(CreateGameEvent($"La balle est rattrapée au vol. {_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} est éliminé."));
                    } else
                    {
                        _events.Add(CreateGameEvent($"Un double jeu est tourné ! Wouawww !!!"));
                    }
                }
            }

            _nbOuts++;

            if (_nbOuts < 3)
            {
                await NextHitter();
            }
        }

        private async Task Homerun()
        {
            _currentHitter.HR++;
            _currentHitter.H++;

            _currentPitcher.HR++;

            if(_playByPlay)
            {
                _events.Add(CreateGameEvent($"{_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} frappe un homerun !!!"));
            }

            _currentHits++;

            if (_bases[2] is not null)
            {
                await Score(_bases[2]);

                _bases[2] = null;
            }

            if (_bases[1] is not null)
            {
                await Score(_bases[1]);

                _bases[1] = null;
            }

            if (_bases[0] is not null)
            {
                await Score(_bases[0]);

                _bases[0] = null;
            }

            await Score(_currentHitter);

            await NextHitter();
        }

        private async Task Score(GameOffensiveStats scorer)
        {

            _currentRuns++;
            _currentHitter.RBI++;
            _currentPitcher.R++;
            scorer.R++;

            if (_playByPlay)
            {
                _events.Add(CreateGameEvent($"{_currentHitter.Player.Firstname.Substring(0, 1)} {_currentHitter.Player.Lastname} score !"));
            }

            if (await isWalkOff())
            {
                _gameOver = true;
            }
        }

        private async Task ResetCount()
        {
            _balls = 0;
            _strikes = 0;
        }

        private async Task<bool> isWalkOff()
        {
            // TODO : to be corrected because _runsHome et _runsAway are updated only when changefield()
            if (_offense == _homeLineUp && _halfInnings >= 16 && _runsHome > _runsAway)
            {
                if (_playByPlay)
                {
                    _events.Add(CreateGameEvent($"{_game.Home.Name} remporte la partie : {_runsAway} - {_runsHome}"));
                }

                return true;
            }

            return false;
        }

        private async Task ChangeField()
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
                await ResetCount();

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
                if (await isWalkOff())
                {
                    _gameOver = true;
                }

                if(_playByPlay && !await isWalkOff())
                {
                    _events.Add(CreateGameEvent($"3 éliminés ! Changefield !"));
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

        private GameEvent CreateGameEvent(string message)
        {
            return new GameEvent()
            {
                Message = message,
                HalfInnings = _halfInnings,
                Outs = _nbOuts,
                Strikes = _strikes,
                Balls = _balls,
                RunsAway = (_offense == _awayLineUp ? _runsAway + _currentRuns : _runsAway),
                RunsHome = (_offense == _homeLineUp ? _runsHome + _currentRuns : _runsHome),
                Bases = _bases
            };
        }
    }
}


