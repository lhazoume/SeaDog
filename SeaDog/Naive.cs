using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;

namespace SeaDog
{
    public class Naive : IPlayer
    {
        private readonly string _name;

        public Naive(string name)
        {
            _name = name;
        }

        public string Name => _name;

        public Move Move(Board board)
        {
            List<Boat> playerBoats = board.Boats(_name);
            Boat boat = playerBoats[0];
            List<Location> fish = board.Fish();

            int nearestFishIndex = 0;
            double nearestDistance = Location.Distance(boat.Location, fish[0]);

            for (int i = 1; i < fish.Count; i++)
                if (Location.Distance(boat.Location, fish[i]) < nearestDistance)
                {
                    nearestFishIndex = i;
                    nearestDistance = Location.Distance(boat.Location, fish[i]);
                }

            Location nearestFish = fish[nearestFishIndex];

            int columnDirection = Math.Sign(nearestFish.Column - boat.Location.Column);
            int rowDirection = Math.Sign(nearestFish.Row - boat.Location.Row);
            Move move = new Move(boat.Id, rowDirection, columnDirection);

            return move;
        }

        public IPlayer Clone() => new Naive(_name);
    }
}
