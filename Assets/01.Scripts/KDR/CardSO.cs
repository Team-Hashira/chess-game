using UnityEngine;

[CreateAssetMenu(fileName = "CardSO", menuName = "SO/CardSO")]
public class CardSO : ScriptableObject
{
    public int cost;
    public Sprite sprite;

    public string cardName;
}
