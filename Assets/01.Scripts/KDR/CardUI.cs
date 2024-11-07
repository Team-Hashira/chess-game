using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public List<Curse> curse;
    public List<Blessing> blessing;

    public bool OnSelected { get; private set; }
    public bool IsSelectable { get; private set; }
    public bool IsHolded { get; private set; }
    public bool IsUseable { get; private set; }
    public bool IsFront { get; private set; }

    public RectTransform VisualTrm { get; private set; }
    public Image VisualImage { get; private set; }
    private Transform _elementsTrm;

    public CardSO cardSO;

    [SerializeField] private Sprite _forntSprite;
    [SerializeField] private Sprite _backSprite;

    private TextMeshProUGUI _costText;
    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _descriptionText;
    private Image _image;

    private Vector2 _startOffset;
    private Vector3 _onMouseScale = Vector3.one * 1.2f;

    private CardController _contoller;

    private Sequence _turnSeq;

    public void Init(CardController holder)
    {
        _contoller = holder;
        VisualTrm = transform.Find("Visual") as RectTransform;
        VisualImage = VisualTrm.GetComponent<Image>();
        _elementsTrm = VisualTrm.Find("Elements");
        _costText = _elementsTrm.Find("Cost").GetComponent<TextMeshProUGUI>();
        _nameText = _elementsTrm.Find("Name").GetComponent<TextMeshProUGUI>();
        _descriptionText = _elementsTrm.Find("Description").GetComponent<TextMeshProUGUI>();
        _image = _elementsTrm.Find("Image").GetComponent<Image>();

        _costText.text = cardSO.cost.ToString();
        _nameText.text = cardSO.name;
        _descriptionText.text = cardSO.cardDescription;
        _image.sprite = cardSO.image;

        IsFront = false;
        _elementsTrm.gameObject.SetActive(false);
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
                _elementsTrm.gameObject.SetActive(IsFront);
                VisualImage.sprite = isFront ? _forntSprite : _backSprite;
            })
            .Append(VisualTrm.DOScaleX(1, 0.1f))
            .AppendCallback(() => IsSelectable = true);
    }

    public void SetCost(int cost)
    {
        _costText.text = cost.ToString();
    }

    public void Use()
    {
        if (blessing.Contains(Blessing.Penance)) _contoller.AddCard(cardSO, true);
        Debug.Log("Use!");
        Destroy(gameObject);
    }

    public void UpdateUseable()
    {
        bool newValue = Vector3.SqrMagnitude(_contoller.UseAreaTrm.position - VisualTrm.position) < 10000f;
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
        _contoller.SetSelectCard(this, true);
        _startOffset = eventData.position - (Vector2)transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsHolded = false;
        _contoller.SetSelectCard(this, false);
    }
}
