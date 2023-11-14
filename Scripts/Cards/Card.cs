using TMPro;
using UnityEngine;

namespace Cards
{
    public class Card : MonoBehaviour, ICard
    {
        [SerializeField] private TMP_Text textForce, textType;
        public int Force { get; private set; }
        public ICard.TypeEnum Type { get; private set; }

        private void Start()
        {
            textForce.text = $"Force : {Force.ToString()}";
            textType.text = $"Type : {Type.ToString()}";
        }

        /// <summary>
        /// Add force to the card
        /// </summary>
        /// <param name="force"></param>
        public void AddForce(int force)
        {
            Force += force;
        }

        /// <summary>
        /// Change the type of the card and update instantly the text
        /// </summary>
        /// <param name="type"></param>
        public void ChangeType(ICard.TypeEnum type)
        {
            Type = type;
            textType.text = $"Type : {Type.ToString()}";
        }

        /// <summary>
        /// Update the force value text on the card
        /// </summary>
        public void UpdateText()
        {
            textForce.text = $"Force : {Force.ToString()}";
        }
    }
}
