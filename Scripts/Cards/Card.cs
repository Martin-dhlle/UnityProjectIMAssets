using TMPro;
using UnityEngine;

namespace Cards
{
    public class Card : MonoBehaviour, ICard
    {
        [SerializeField] private TMP_Text _textForce, _textType;
        public int Force { get; set; }
        public ICard.TypeEnum Type { get; set; }

        private void Start()
        {
            _textForce.text = $"Force : {Force.ToString()}";
            _textType.text = $"Type : {Type.ToString()}";
            Debug.Log($"{Force.ToString()}");
            Debug.Log($"{Type.ToString()}");
        }

        public void AddForce(int force)
        {
            Force = force;
            _textForce.text = $"Force : {Force.ToString()}";
        }

        public void ChangeType(ICard.TypeEnum type)
        {
            Type = type;
            _textType.text = $"Type : {Type.ToString()}";
        }
    }
}
