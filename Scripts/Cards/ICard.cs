namespace Interfaces
{
    public interface ICard
    {

        public string CardName { get; set; }
        
        public int Force { get; set; }
        
        public enum Type
        {
            Thrust,
            Slash,
            Bash
        }

    }
}