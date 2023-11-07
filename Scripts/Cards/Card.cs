using TMPro;
using UnityEngine;

namespace Cards
{
    public class Card : MonoBehaviour, ICard
    {
        public TMP_Text textForce, textType;
        public int Force { get; set; }
        public ICard.TypeEnum Type { get; set; }

        private void Start()
        {
            textForce.text = $"Force : {Force.ToString()}";
            textType.text = $"Type : {Type.ToString()}";
        }
    }
}
