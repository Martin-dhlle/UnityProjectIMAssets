namespace Cards
{
    public interface LEGACY_ICard
    {

        public string CardName { get; set; }
        
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