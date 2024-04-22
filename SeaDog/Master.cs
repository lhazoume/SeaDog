using System;
using System.Collections.Generic;
using System.Linq;

namespace SeaDog
{
    public class Master
    {
        #region Variables
        private const int _rows = 100, _columns = 100;
        private readonly IPlayer[] _players;
        private readonly int[] _scores;
        private readonly int _fishCount, _boatCount;
        #endregion

        public Master(IPlayer firstPlayer, IPlayer secondPlayer, int fishCount, int boatCount)
        {
            _players = new IPlayer[2] { firstPlayer, secondPlayer };
            _scores = new int[2];
            _fishCount = fishCount;
            _boatCount = boatCount;
        }

        #region Methods
        private Board InitializeBoard()
        {
            _scores[0] = 0;
            _scores[1] = 0;

            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            #region Fish
            List<Location> fish = new List<Location>();
            for (int i = 0; i < _fishCount; i++)
            {
                Location newFish = new Location(rnd.Next(_rows), rnd.Next(_columns));
                while (fish.Count > 0 && fish.Any(f => f.Row == newFish.Row && f.Column == newFish.Column))
                    newFish = new Location(rnd.Next(_rows), rnd.Next(_columns));
                fish.Add(newFish);
            }
            #endregion

            #region Boats
            List<Boat> boats = new List<Boat>();
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < _boatCount; j++)
                {
                    Location newBoatLocation = new Location(rnd.Next(_rows), rnd.Next(_columns));
                    List<Location> usedLocations = fish.Concat(boats.Select(b => b.Location)).ToList();

                    while (usedLocations.Any(f => f.Row == newBoatLocation.Row && f.Column == newBoatLocation.Column))
                        newBoatLocation = new Location(rnd.Next(_rows), rnd.Next(_columns));
                    boats.Add(new Boat(_players[i].Name, Guid.NewGuid().ToString(), newBoatLocation));
                }
            #endregion

            return new Board(_rows, _columns, fish, boats);
        }
        private Board UpdateFromMove(Board previousBoard, Move move, Log log)
        {
            Boat targetBoat = previousBoard.Boats().Find(b => b.Id == move.Id);

            Board newBoard = previousBoard.HypotheticalBoard(move, out int fishImpact, out int crashImpact);

            if (fishImpact == 1)
            {
                int playerIndex = _players[0].Name == targetBoat.Player ? 0 : 1;
                _scores[playerIndex] += 1;
                log.Add($"Fish by {_players[playerIndex].Name}");
            }

            if (crashImpact == 1)
                log.Add($"Crash by {targetBoat.Player}");

            return newBoard;
        }
        private bool IsGameOver(Board board)
        {
            if (board.Fish().Count == 0)
                return true;
            if (board.Boats(_players[0].Name).Count == 0)
                return true;
            if (board.Boats(_players[1].Name).Count == 0)
                return true;
            return false;
        }
        public void Run(Log log)
        {
            Board board = InitializeBoard();
            log.Add("Board initialization");
            bool gameOver = false;
            int rounds = 0;

            while (!gameOver)
            {
                //log.Add($"Round #{rounds}");

                for (int i = 0; i < 2 && !gameOver; i++)
                {
                    Move proposition = _players[i].Move(board);

                    if (board.IsPossible(_players[i].Name, proposition))
                    {
                        board = UpdateFromMove(board, proposition, log);
                        
                    }
                    //else
                    //    log.Add($"Impossible move by {_players[i].Name}");
                    gameOver = IsGameOver(board);
                }
                rounds++;
            }
            log.Add($"Final score {_players[0].Name}:{_scores[0]}-{_scores[1]}:{_players[1].Name}");
            log.Add($"Final boats {_players[0].Name}:{board.Boats(_players[0].Name).Count}-{board.Boats(_players[1].Name).Count}:{_players[1].Name}");

            string winner = Winner;
            log.Add(winner == "Draw" ? "Draw" : $"Win by {winner}");
        }
        #endregion

        #region Accessors
        public string Winner
        {
            get
            {
                if (_scores[0] == _scores[1]) return "Draw";
                return _scores[0] > _scores[1] ? _players[0].Name : _players[1].Name;
            }
        }
        #endregion
    }
}
