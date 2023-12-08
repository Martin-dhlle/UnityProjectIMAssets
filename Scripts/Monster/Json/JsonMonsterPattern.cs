using System.Collections.Generic;

namespace Monster.Json
{
    public class JsonMonsterPattern
    {
        public readonly Dictionary<int, JsonMonsterData> Patterns;

        public JsonMonsterPattern(Dictionary<int, JsonMonsterData> patterns)
        {
            Patterns = patterns;
        }
    }
    
    public class JsonMonsterData
    {
        public string AttackName;
        public string ImagePath;    
        public string Type;
        public int Force;
        public int Qte;
        public int Fame;
    }
}