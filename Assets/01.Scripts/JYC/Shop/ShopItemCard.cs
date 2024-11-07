using UnityEngine;
using TMPro;

public class ShopItemCard : MonoBehaviour
{
    public ShopItemCardSO itemCardSO;
    private TextMeshProUGUI _coinText;

    private void Awake()
    {
        _coinText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        _coinText.text = itemCardSO.price.ToString();
    }

    public void BuyCard()
    {
        if (itemCardSO.price > Managers.Coin.SetCoin())
            return;

        Managers.Coin.LostCoin(itemCardSO.price);
        Debug.Log(Managers.Coin.SetCoin());
    }
}
