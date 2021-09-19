using System;
using System.Collections.Generic;
using System.Linq;

namespace SeaDog
{
    public class RandomWalk : IPlayer
    {
        private readonly Random _rnd;
        private readonly string _name;

        public RandomWalk(string name)
        {
            _name = name;
            _rnd = new Random(Guid.NewGuid().GetHashCode());
        }

        public string Name => _name;

        public Move Move(Board board)
        {
            List<Boat> playerBoats = board.Boats(_name);
            Boat boat = playerBoats[_rnd.Next(playerBoats.Count)];

            Move move;
            do
            {
                int direction = _rnd.Next(4),
                    rowMove, colMove;

                switch (direction)
                {
                    case 0: rowMove = 1; colMove = 0; break;
                    case 1: rowMove = 0; colMove = 1; break;
                    case 2: rowMove = -1; colMove = 0; break;
                    default: rowMove = 0; colMove = -1; break;
                }
                move = new Move(boat.Id, rowMove, colMove);
            }
            while (!board.IsPossible(_name, move) || playerBoats.Where(b => b.Id != boat.Id).Any(b => b.Colocates(boat.Location.Row + move.RowMove, boat.Location.Column + move.ColumnMove)));
            return move;
        }

        public IPlayer Clone() => new RandomWalk(_name);
    }
}
