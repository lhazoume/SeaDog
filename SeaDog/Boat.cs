namespace SeaDog
{
    public class Boat
    {
        private readonly string _id, _player;
        private readonly Location _location;

        public Boat(string player, string id, Location location)
        {
            _player = player;
            _id = id;
            _location = location;
        }

        #region Accessors
        public string Player => _player;
        public string Id => _id;
        public Location Location => _location;
        #endregion

        #region Methods
        public bool Colocates(Location location)
        {
            return _location.Equals(location);
        }
        public bool Colocates(int row, int column)
        {
            return _location.Equals(row, column);
        }
        #endregion
    }
}
