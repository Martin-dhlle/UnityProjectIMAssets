namespace Cards
{
    public interface ICard
    {
        public int Force { get; }
        
        public TypeEnum Type { get; }

        public enum TypeEnum
        {
            Thrust,
            Slash,
            Bash,
        }

        public enum ModeEnum
        {
            Battle,
            Edit
        }
    }
}