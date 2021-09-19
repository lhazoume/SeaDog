using System;

namespace SeaDog
{
    public struct Location
    {
        private readonly int _row, _column;

        public Location(int row, int column)
        {
            _row = row;
            _column = column;
        }

        public int Row => _row;
        public int Column => _column;

        #region Methods
        public bool Equals(Location location)
        {
            return _row == location.Row && _column == location.Column;
        }
        public bool Equals(int row, int column)
        {
            return _row == row && _column == column;
        }
        public Location Clone()
        {
            return new Location(_row, _column);
        }
        public Location Alter(int rowMove, int colMove)
        {
            return new Location(_row + rowMove, _column + colMove);
        }
        public Location Alter(Move move)
        {
            return Alter(move.RowMove, move.ColumnMove);
        }
        #endregion

        #region Operator
        public static double Distance(Location x, Location y)
        {
            return Math.Abs(x.Row - y.Row) + Math.Abs(x.Column - y.Column);
        }
        #endregion
    }
}
