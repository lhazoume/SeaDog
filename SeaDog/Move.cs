namespace SeaDog
{
    public struct Move
    {
        private readonly string _id;
        private readonly int _rowMove, _columnMove;

        public Move(string id, int rowMove, int columnMove)
        {
            _id = id;
            _rowMove = rowMove;
            _columnMove = columnMove;
        }

        public string Id => _id;
        public int RowMove => _rowMove;
        public int ColumnMove => _columnMove;
    }
}
