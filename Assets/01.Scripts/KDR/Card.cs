using DG.Tweening;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEditor.Experimental.GraphView;
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

    public bool OnSelected { get; private set; }
    public bool IsSelectable { get; private set; }
    public bool IsHolded { get; private set; }
    public bool IsUseable { get; private set; }
    public bool IsFront {  get; private set; }

    public RectTransform VisualTrm { get; private set; }
    public Image VisualImage { get; private set; }
    private GameObject _elementsObj;

    public CardSO cardSO;

    [SerializeField] private Sprite _forntSprite;
    [SerializeField] private Sprite _backSprite;

    private Vector2 _startOffset;
    private Vector3 _onMouseScale = Vector3.one * 1.2f;

    private CardController _holder;

    private Sequence _turnSeq;

    public void Init(CardController holder)
    {
        _holder = holder;
        VisualTrm = transform.Find("Visual") as RectTransform;
        VisualImage = VisualTrm.GetComponent<Image>();
        _elementsObj = VisualTrm.Find("Elements").gameObject;
    }

    public void Turn(bool isFront)
    {
        if (_turnSeq != null && _turnSeq.IsActive()) _turnSeq.Kill();
        _turnSeq = DOTween.Sequence();

        IsSelectable = false;
        IsFront = isFront;

        _turnSeq.Append(VisualTrm.DOScaleX(0, 0.1f))
            .AppendCallback(() =>
            {
                _elementsObj.SetActive(IsFront);
                VisualImage.sprite = isFront ? _forntSprite : _backSprite;
            })
            .Append(VisualTrm.DOScaleX(1, 0.1f))
            .AppendCallback(() => IsSelectable = true);
    }

    public void Use()
    {
        Debug.Log("Use!");
        Destroy(gameObject);
    }

    public void UpdateUseable()
    {
        bool newValue = Vector3.SqrMagnitude(_holder.UseAreaTrm.position - VisualTrm.position) < 10000f;
        if (IsUseable != newValue)
        {
            IsUseable = newValue;
            VisualTrm.GetComponent<Image>().color = IsUseable ? Color.yellow : Color.white;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position - _startOffset;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsSelectable == false) return;
        OnSelected = true;
        VisualTrm.DOScale(_onMouseScale, 0.1f);
        //VisualTrm.DOAnchorPosY(_visualDefaultYPos + 100f, 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsSelectable == false) return;
        OnSelected = false;
        VisualTrm.DOScale(Vector3.one, 0.1f);
        //VisualTrm.DOAnchorPosY(_visualDefaultYPos, 0.1f);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        IsHolded = true;
        _holder.SetSelectCard(this, true);
        _startOffset = eventData.position - (Vector2)transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsHolded = false;
        _holder.SetSelectCard(this, false);
    }
}
