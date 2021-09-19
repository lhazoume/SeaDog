namespace SeaDog
{
    public interface IPlayer
    {
        string Name { get; }
        Move Move(Board board);

        IPlayer Clone();
    }
}
