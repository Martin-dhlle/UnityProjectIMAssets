using System.Collections.Generic;

namespace Cards.Json
{
    public class JsonPattern
    {
        public readonly Dictionary<string, JsonCard[]> Patterns;

        public JsonPattern(Dictionary<string, JsonCard[]> patterns)
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