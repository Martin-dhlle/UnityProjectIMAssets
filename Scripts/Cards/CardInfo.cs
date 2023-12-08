using TMPro;
using UnityEngine;

public class CardInfo : MonoBehaviour
{
    private int Force { get; set; }
    private string Type { get; set; }

    [SerializeField] private TextMeshPro attackNameText,forceText, typeText;

    public void SetAttackName(string attackName)
    {
        attackNameText.text = attackName;
    }
    
    public void SetForce(int force)
    {
        Force = force;
        forceText.text = Force.ToString();
    }

    public void SetType(string type)
    {
        Type = type;
        typeText.text = Type;
    }
}
