using System;
using Cards;

namespace HelperScripts
{
    public static class CompareFunctions
    {
        public static bool CompareType(ICard.TypeEnum cardType, ICard.TypeEnum monsterType)
        {
            switch (monsterType)
            {
                case ICard.TypeEnum.Bash:
                    if (cardType == ICard.TypeEnum.Slash)
                    {
                        return false;
                    }

                    return true;
                case ICard.TypeEnum.Slash:
                    if (cardType == ICard.TypeEnum.Thrust)
                    {
                        return false;
                    }

                    return true;
                case ICard.TypeEnum.Thrust:
                    if (cardType == ICard.TypeEnum.Bash)
                    {
                        return false;
                    }

                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cardType), cardType, null);
            }
        }
    }
}