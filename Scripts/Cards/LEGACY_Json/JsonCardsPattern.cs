using System.Collections.Generic;

namespace Cards.LEGACY_Json
{
    public class JsonCardsPattern
    {
        public readonly Dictionary<string, JsonCard[]> Patterns;

        public JsonCardsPattern(Dictionary<string, JsonCard[]> patterns)
        {
            Patterns = patterns;
        }
    }
    
    public class JsonCard
    {
        public string CardName;
        public int Force;
        public string Type;
    }
}