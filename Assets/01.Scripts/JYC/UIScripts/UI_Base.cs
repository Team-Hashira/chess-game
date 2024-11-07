using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_Base : InitBase
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    private void Awake()
    {
        Init();
    }

    #region Bind
    // Enum에서 정의한 변수명 과 같은 오브젝트<T>를 가져올거임
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind{names[i]}");
        }
    }

    protected void BindRectTrms(Type type) { Bind<RectTransform>(type); }
    protected void BindTexts(Type type) { Bind<TMP_Text>(type); }
    protected void BindButtons(Type type) { Bind<Button>(type); }
    protected void BindImages(Type type) { Bind<Image>(type); }
    protected void BindSliders(Type type) { Bind<Slider>(type); }
    #endregion

    #region Get

    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[index] as T;
    }

    protected RectTransform GetObjects(int idx) { return Get<RectTransform>(idx); }
    protected TMP_Text GetTexts(int idx) { return Get<TMP_Text>(idx); }
    protected Button GetButtons(int idx) { return Get<Button>(idx); }
    protected Image GetImages(int idx) { return Get<Image>(idx); }
    protected Slider GetSlider(int idx) { return Get<Slider>(idx); }

    #endregion
}
