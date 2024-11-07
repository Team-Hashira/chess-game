using System.Collections;
using UnityEngine;

public class MainSceneJYC : BaseScene
{
    //private IEnumerator _currentCotoutine = null;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Debug.Log("일단 되겠지");
        //Managers.UI.
        return true;
    }

    public override void Clear()
    {

    }
}
