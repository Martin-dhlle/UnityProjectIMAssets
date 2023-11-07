using System.Collections.Generic;

namespace Monster.Json
{
    public class JsonMonsterPattern
    {
        public readonly Dictionary<string, JsonMonster> Patterns;

        public JsonMonsterPattern(Dictionary<string, JsonMonster> patterns)
        {
            Patterns = patterns;
        }
    }
    
    public class JsonMonster
    {
        public string Type;
        public int Force;
    }
}