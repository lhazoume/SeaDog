using System;
using System.Collections.Generic;
using System.Linq;

namespace SeaDog
{
    public class Fisherman : Chaser, IPlayer
    {
        public Fisherman(string name)
            : base(name) { }

        public Move Move(Board board)
        {
            List<Boat> playerBoats = board.Boats(_name);

            #region All possible moves
            List<Move> moves = new List<Move>();
            foreach (Boat boat in playerBoats)
            {
                moves.Add(new Move(boat.Id, 0, 1));
                moves.Add(new Move(boat.Id, 1, 0));
                moves.Add(new Move(boat.Id, 0, -1));
                moves.Add(new Move(boat.Id, -1, 0));
            }

            //Remove all impossible moves
            moves.RemoveAll(m => !board.IsPossible(_name, m));

            //Remove all moves that result in crashing in one's boat
            moves.RemoveAll(m =>
            {
                Boat currentBoat = playerBoats.Find(b => b.Id == m.Id);
                Location currentBoatNewLocation = currentBoat.Location.Alter(m);
                return playerBoats.Where(b => b.Id != m.Id).Any(b => currentBoat.Colocates(currentBoatNewLocation));
            });
            #endregion

            double[] functionValues = new double[moves.Count];
            double currentValue = TotalProximityToFish(board);
            for (int i = 0; i < moves.Count; i++)
            {
                Board hypotheticalBoard = board.HypotheticalBoard(moves[i], out int fishImpact, out int crashImpact);
                functionValues[i] = TotalProximityToFish(board) + fishImpact;
            }

            double maxValue = functionValues.Max();
            List<int> indices = Enumerable.Range(0, moves.Count).Where(i => functionValues[i] == maxValue).ToList();
            int index = (new Random(Guid.NewGuid().GetHashCode())).Next(indices.Count); 
            return moves[indices[index]];
        }

        public IPlayer Clone() => new Fisherman(_name);
    }
}
