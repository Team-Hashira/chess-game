using System.Net.NetworkInformation;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private int _coin = 110;
    private int _currentCoin;

    public void GetCoin(int value)
    {
        _coin += value;
    }

    public void LostCoin(int value)
    {
        _coin -= value;
    }

    public int SetCoin()
    {
        return _coin;
    }
}
