using System;
using System.Collections.Generic;
using System.Linq;

namespace SeaDog
{
    public class Board
    {
        #region Variables
        private readonly int _rows, _columns;
        private readonly List<Location> _fish;
        private readonly List<Boat> _boats;
        #endregion

        public Board(int rows, int columns, List<Location> fish, List<Boat> boats)
        {
            _rows = rows;
            _columns = columns;
            _fish = fish.ToList();
            _boats = boats.ToList();
        }

        #region Methods

        public List<Boat> Boats(string player)
        {
            return _boats.FindAll(b => b.Player == player);
        }

        public List<Boat> Boats()
        {
            return _boats.ToList();
        }

        public List<Location> Fish()
        {
            return _fish.ToList();
        }

        public int[,] Content(string player)
        {
            int[,] result = new int[_rows, _columns];

            foreach (Location location in _fish)
                result[location.Row, location.Column] = 1;

            foreach (Boat boat in _boats)
                result[boat.Location.Row, boat.Location.Column] = boat.Player == player ? 10 : 20;

            return result;
        }
        
        public bool IsPossible(string player, Move move)
        {
            if (Math.Abs(move.RowMove) + Math.Abs(move.ColumnMove) > 1)
                return false;

            Boat targetBoat = _boats.Find(b => b.Player == player && b.Id == move.Id);

            if (targetBoat == null)
                return false;

            int targetRow = targetBoat.Location.Row + move.RowMove,
                targetColumn = targetBoat.Location.Column + move.ColumnMove;

            if (targetRow < 0 || targetRow >= _rows || targetColumn < 0 || targetColumn >= _columns)
                return false;

            return true;
        }

        public Board HypotheticalBoard(Move move, out int fishImpact, out int crashImpact)
        {
            Boat targetBoat = _boats.Find(b => b.Id == move.Id);
            Location newLocation = targetBoat.Location.Alter(move);

            #region Check if move fished
            List<Location> fish;
            if (_fish.Any(l => l.Equals(newLocation)))
            {
                fish = _fish.FindAll(l => !l.Equals(newLocation));
                fishImpact = 1;
            }
            else
            {
                fish = _fish;
                fishImpact = 0;
            }
            #endregion

            #region Check if boat collides another boat
            List<Boat> boats;
            if (_boats.Any(b => b.Id != move.Id && b.Location.Equals(newLocation)))
            {
                //Collision
                boats = _boats.FindAll(b => !b.Colocates(newLocation) && b.Id != move.Id).ToList();
                crashImpact = 1;
            }
            else
            {
                //No collision
                boats = _boats.FindAll(b => b.Id != move.Id);
                boats.Add(new Boat(targetBoat.Player, move.Id, newLocation));
                crashImpact = 0;
            }
            #endregion

            return new Board(_rows, _columns, fish, boats);
        }

        #endregion
    }
}
