using DG.Tweening;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public List<Curse> curse;
    public List<Blessing> blessing;

    public bool OnSelected { get; private set; }
    public bool IsSelectable { get; private set; }
    public int LockedLevel { get; private set; } = 0;
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
    private Transform _curseTags;
    private Transform _blessingTags;

    private int _cardIndex;
    private Vector2 _startOffset;
    private Vector3 _onMouseScale = Vector3.one * 1.2f;

    private CardController _contoller;

    private Sequence _turnSeq;

    private int _cost;

    private void Update()
    {
        if (LockedLevel == 0)
        {
            if (IsHolded)
            {
                transform.position = Input.mousePosition - (Vector3)_startOffset;
                VisualTrm.localRotation = Quaternion.Lerp(VisualTrm.localRotation,
                    Quaternion.Inverse(transform.localRotation), Time.deltaTime * 10);
                UpdateUseable();
            }
        }
    }

    public void Init(CardController controller, int index)
    {
        _cardIndex = index;
        _contoller = controller;
        VisualTrm = transform.Find("Visual") as RectTransform;
        VisualImage = VisualTrm.GetComponent<Image>();
        _elementsTrm = VisualTrm.Find("Elements");
        _costText = _elementsTrm.Find("Cost").GetComponent<TextMeshProUGUI>();
        _nameText = _elementsTrm.Find("Name").GetComponent<TextMeshProUGUI>();
        _descriptionText = _elementsTrm.Find("Description").GetComponent<TextMeshProUGUI>();
        _image = _elementsTrm.Find("Image").GetComponent<Image>();
        _curseTags = _elementsTrm.Find("CurseTag");
        _blessingTags = _elementsTrm.Find("BlessingTag");

        _cost = cardSO.cost;
        _costText.text = _cost.ToString();
        _nameText.text = cardSO.name;
        _descriptionText.text = cardSO.cardDescription;
        _image.sprite = cardSO.image;

        IsFront = false;
        _elementsTrm.gameObject.SetActive(false);

        UpdateTag();
    }

    public void AfterInit()
    {
        if (curse.Contains(Curse.Envy))
        {
            if (_cardIndex - 1 >= 0) _contoller.cards[_cardIndex - 1].Lock(true);
            if (_cardIndex + 1 < _contoller.cards.Count) _contoller.cards[_cardIndex + 1].Lock(true);
        }
    }

    public void UpdateTag()
    {
        foreach (Curse tag in curse)
        {
            _curseTags.GetChild((int)tag).gameObject.SetActive(true);
        }
        foreach (Blessing tag in blessing)
        {
            _blessingTags.GetChild((int)tag).gameObject.SetActive(true);
        }
    }
    public void UpdateArray(float interval, float targetXPosInterval, float targetYPosInterval, float targetAngleInterval)
    {
        if (IsHolded) return;
        VisualTrm.anchoredPosition = Vector2.Lerp(VisualTrm.anchoredPosition,
            new Vector2(interval * targetXPosInterval, targetYPosInterval), Time.deltaTime * 10);
        transform.localRotation = Quaternion.Lerp(transform.localRotation,
            Quaternion.Euler(0, 0, -interval * targetAngleInterval), Time.deltaTime * 10);
        transform.localPosition = Vector2.Lerp(transform.localPosition,
            Vector3.zero, Time.deltaTime * 10);
        VisualTrm.localRotation = Quaternion.Lerp(VisualTrm.localRotation,
            Quaternion.identity, Time.deltaTime * 10);
    }

    public void Lock(bool isLock)
    {
        LockedLevel += isLock ? 1 : -1;
        VisualImage.color = LockedLevel != 0 ? Color.gray : Color.white;
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
                _elementsTrm.gameObject.SetActive(isFront);
                VisualImage.sprite = isFront ? _forntSprite : _backSprite;
            })
            .Append(VisualTrm.DOScaleX(1, 0.1f))
            .AppendCallback(() => IsSelectable = true);
    }

    public void CostModify(int cost, Color color = default)
    {
        _cost = Mathf.Clamp(_cost + cost, 0, 10);
        _costText.text = _cost.ToString();
        if (color != default)
            _costText.color = color;
    }

    public bool Use()
    {
        if (_contoller.TryUseCost(_cost) == false) return false;

        if (blessing.Contains(Blessing.Penance)) _contoller.AddCard(cardSO, true);
        if (curse.Contains(Curse.Envy))
        {
            if (_cardIndex - 1 >= 0) _contoller.cards[_cardIndex - 1].Lock(false);
            if (_cardIndex + 1 < _contoller.cards.Count) _contoller.cards[_cardIndex + 1].Lock(false);
        }
        Debug.Log("Use!");
        Destroy(gameObject);
        return true;
    }

    public void UpdateUseable()
    {
        bool newValue = Vector3.SqrMagnitude(_contoller.UseAreaTrm.position - VisualTrm.position) < 10000f;
        if (IsUseable != newValue)
        {
            IsUseable = newValue;
            VisualImage.color = IsUseable ? Color.blue : (LockedLevel != 0 ? Color.gray : Color.white);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (LockedLevel != 0) return;
        if (IsFront == false || IsSelectable == false || IsHolded) return;
        OnSelected = true;
        VisualTrm.DOScale(_onMouseScale, 0.1f);
        //VisualTrm.DOAnchorPosY(_visualDefaultYPos + 100f, 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (LockedLevel != 0) return;
        if (IsFront == false || IsSelectable == false || IsHolded) return;
        OnSelected = false;
        VisualTrm.DOScale(Vector3.one, 0.1f);
        //VisualTrm.DOAnchorPosY(_visualDefaultYPos, 0.1f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (LockedLevel != 0) return;
        IsHolded = true;
        _contoller.SetSelectCard(this, true);
        _startOffset = eventData.position - (Vector2)transform.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (LockedLevel != 0) return;
        IsHolded = false;
        VisualImage.color = LockedLevel != 0 ? Color.gray : Color.white;
        _contoller.SetSelectCard(this, false);
    }
}
