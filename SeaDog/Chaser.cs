using System;
using System.Collections.Generic;
using System.Linq;

namespace SeaDog
{
    public abstract class Chaser
    {
        protected readonly string _name;
        private readonly double _fishProximityWeight, _concurrentProximityWeight;

        public Chaser(string name)
        {
            _name = name;
        }

        public string Name => _name;

        /*public Move Move(Board board)
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
            double currentValue = _fishProximityWeight * TotalProximityToFish(board) + _concurrentProximityWeight * TotalProximityToOtherBoats(board);

            for (int i = 0; i < moves.Count; i++)
            {
                Board hypotheticalBoard = board.HypotheticalBoard(moves[i], out int fishImpact, out int crashImpact);

                functionValues[i] = _fishProximityWeight * (TotalProximityToFish(board) + fishImpact)
                    + _concurrentProximityWeight * (TotalProximityToOtherBoats(board) + crashImpact);
            }

            double maxValue = functionValues.Max();
            int index = Array.IndexOf(functionValues, maxValue);
            return moves[index];
        }*/

        protected double TotalProximityToFish(Board board)
        {
            double result = 0;
            List<Boat> playerBoats = board.Boats(_name);
            foreach (Location fish in board.Fish())
                foreach (Boat boat in playerBoats)
                    result += 1 / Location.Distance(fish, boat.Location);
            return result;
        }
        protected double TotalProximityToNearestFish(Board board)
        {
            double result = 0;
            List<Boat> playerBoats = board.Boats(_name);
            foreach (Boat boat in playerBoats)
            {
                double[] fishDistance = board.Fish().Select(f => Location.Distance(f, boat.Location)).ToArray();
                result += 1 / fishDistance.Min();
            }

            return result;
        }
        protected double TotalProximityToOtherBoats(Board board)
        {
            double result = 0;
            List<Boat> playerBoats = board.Boats(_name),
                otherBoats = board.Boats().FindAll(b => b.Player != _name);

            foreach (Boat otherBoat in otherBoats)
                foreach (Boat boat in playerBoats)
                    result += 1 / Location.Distance(otherBoat.Location, boat.Location);
            return result;
        }

    }
}
