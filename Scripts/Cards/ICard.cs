namespace Cards
{
    public interface ICard
    {
        public int Force { get; set; }
        
        public TypeEnum Type { get; set; }

        public enum TypeEnum
        {
            Thrust,
            Slash,
            Bash,
        }
    }
}