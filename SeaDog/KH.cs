using System.Collections.Generic;

namespace SeaDog
{
    public class KH : IPlayer
    {
        private readonly string _name;

        public KH(string name)
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

            Move move = new Move(boat.Id, 0, 0);
            if ((nearestFish.Row > boat.Location.Row) && (nearestFish.Column > boat.Location.Column))
            {
                move = new Move(boat.Id, 1, 1);
            }
            else if ((nearestFish.Row < boat.Location.Row) && (nearestFish.Column < boat.Location.Column))
            {
                move = new Move(boat.Id, -1, -1);
            }
            else if ((nearestFish.Row < boat.Location.Row) && (nearestFish.Column > boat.Location.Column))
            {
                move = new Move(boat.Id, -1, 1);
            }
            else if ((nearestFish.Row > boat.Location.Row) && (nearestFish.Column < boat.Location.Column))
            {
                move = new Move(boat.Id, 1, -1);
            }
            else if (nearestFish.Row == boat.Location.Row)
            {
                if (nearestFish.Column > boat.Location.Column)
                {
                    move = new Move(boat.Id, 0, 1);
                }
                else
                {
                    move = new Move(boat.Id, 0, -1);
                }
            }
            else if (nearestFish.Column == boat.Location.Column)
            {
                if (nearestFish.Row > boat.Location.Row)
                {
                    move = new Move(boat.Id, 1, 0);
                }
                else
                {
                    move = new Move(boat.Id, -1, 0);
                }
            }


            return move;
        }

        public IPlayer Clone() => new KH(_name);
    }
}
