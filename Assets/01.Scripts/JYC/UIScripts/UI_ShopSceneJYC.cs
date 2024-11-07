using UnityEngine;

public class UI_ShopSceneJYC : UI_Scene
{
    public enum Images
    {
        Image_ShopBackGround
    }

    public enum Texts
    {
        Text_Coin
    }

    protected override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindImages(typeof(Images));
        BindTexts(typeof(Texts));

        GetImages((int)Images.Image_ShopBackGround).transform.position = new Vector3(0,0,0);
        GetTexts((int)Texts.Text_Coin).text = Managers.Coin.SetCoin().ToString();

        return true;
    }
}
