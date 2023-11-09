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

        public void AddForce(int force)
        {
            Force = force;
            textForce.text = $"Force : {Force.ToString()}";
        }

        public void ChangeType(ICard.TypeEnum type)
        {
            Type = type;
            textType.text = $"Type : {Type.ToString()}";
        }
    }
}
