using System.Collections;
using UnityEngine;

public class MainSceneJYC : BaseScene
{
    //private IEnumerator _currentCotoutine = null;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Debug.Log("�ϴ� �ǰ���");
        //Managers.UI.
        return true;
    }

    public override void Clear()
    {

    }
}
