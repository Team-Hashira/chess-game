using UnityEngine;

[CreateAssetMenu(fileName = "CardSO", menuName = "SO/CardSO")]
public class CardSO : ScriptableObject
{
    new public string name;
    public int cost;
    public Sprite image;
    [TextArea]
    public string cardDescription;
}
