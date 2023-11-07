using TMPro;
using UnityEngine;

namespace Cards
{
    public class Card : MonoBehaviour, ICard
    {
        public TMP_Text textCardName,textForce, textType;
        public string CardName { get; set; } = "default";
        public int Force { get; set; }
        public ICard.TypeEnum Type { get; set; }

        private void Start()
        {
            textCardName.text = $"{CardName}";
            textForce.text = $"Force : {Force.ToString()}";
            textType.text = $"Type : {Type.ToString()}";
        }
    }
}
