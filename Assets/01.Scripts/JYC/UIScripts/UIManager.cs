using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    [SerializeField] private Image _shopPanel;
    //private int _order = 10;

    //private UI_Scene _sceneUI = null;

    //private UI_Scene UIScene
    //{
    //    get { return _sceneUI; }
    //    set { _sceneUI = value; }
    //}

    public void ShopOpen()
    {
        _shopPanel.gameObject.SetActive(true);
    }
}
