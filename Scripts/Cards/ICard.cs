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

        public void AddForce(int force);

        public void ChangeType(TypeEnum type);
    }
}