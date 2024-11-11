using UnityEngine;
using UnityEngine.UI;

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

        return true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            GetImages((int)Images.Image_ShopBackGround).transform.position = new Vector3(-2000, 0, 0);

        GetTexts((int)Texts.Text_Coin).text = Managers.Coin.SetCoin().ToString(); // 이건 수정할거임
    }
}
