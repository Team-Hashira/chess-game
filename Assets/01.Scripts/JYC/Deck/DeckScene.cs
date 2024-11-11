using UnityEngine;

public class DeckScene : UI_Scene
{
    public enum Images
    {
        Image_InventoryBackGround
    }

    protected override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindImages(typeof(Images));
        //GetImages((int)Images.Image_InventoryBackGround).transform.position = Vector3.zero;

        return true;
    }
}
