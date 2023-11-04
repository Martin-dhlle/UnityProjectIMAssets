using Interfaces;
using UnityEngine;

public class CardController : MonoBehaviour, ICard
{
    public string CardName { get; set; } = "default";
    public int Force { get; set; }
    public ICard.Type Type { get; set; }
}