using DG.Tweening;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.WSA;

[System.Serializable]
public enum Curse
{
    Destruction,
    Pride,
    Envy,
    Gluttony,
    Belonging,
    Regret,
}
[System.Serializable]
public enum Blessing
{
    Charity,
    Resection,
    Love,
    Penance,
}

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public List<Curse> curse;
    public List<Blessing> blessing;

    public int cost;
    public int id;
    public bool onMouse;
    public bool isSelected;
    public bool isUseable;

    public RectTransform VisualTrm { get; private set; }
    private RectTransform _rectTrm;

    private Vector2 _startOffset;
    private Vector3 _onMouseScale = Vector3.one * 1.2f;
    private float _visualDefaultYPos;

    private Vector2 _startPos;
    private Quaternion _startRot;

    private CardController _holder;

    public void Init(CardController holder, int id)
    {
        this.id = id;
        _holder = holder;
        _rectTrm = transform as RectTransform;
        VisualTrm = transform.Find("Visual") as RectTransform;
    }

    public void Use()
    {
        Debug.Log("Use!");
        Destroy(gameObject);
    }

    public void UpdateUseable()
    {
        bool newValue = Vector3.SqrMagnitude(_holder.UseAreaTrm.position - VisualTrm.position) < 10000f;
        if (isUseable != newValue)
        {
            isUseable = newValue;
            VisualTrm.GetComponent<Image>().color = isUseable ? Color.yellow : Color.white;
        }
    }

    public void SetStartValue()
    {
        _visualDefaultYPos = VisualTrm.anchoredPosition.y;
        _startPos = _rectTrm.anchoredPosition;
        _startRot = transform.rotation;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position - _startOffset;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onMouse = true;
        VisualTrm.DOScale(_onMouseScale, 0.1f);
        VisualTrm.DOAnchorPosY(_visualDefaultYPos + 100f, 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onMouse = false;
        VisualTrm.DOScale(Vector3.one, 0.1f);
        VisualTrm.DOAnchorPosY(_visualDefaultYPos, 0.1f);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isSelected = true;
        _holder.SetSelectCard(this, true);
        _startOffset = eventData.position - (Vector2)transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isSelected = false;
        _holder.SetSelectCard(this, false);
    }
}
