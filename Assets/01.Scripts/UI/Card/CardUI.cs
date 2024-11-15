using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Card cardData;

    public bool OnSelected { get; private set; }
    public bool IsSelectable { get; private set; }
    public int LockedLevel { get; private set; } = 0;
    public bool IsHolded { get; private set; }
    public bool IsUseable { get; private set; }
    public bool IsFront { get; private set; }

    [SerializeField] private Sprite _forntSprite;
    [SerializeField] private Sprite _backSprite;

    [Space(25)]
	[SerializeField] private Transform _elementsTrm;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Image _image;
    [SerializeField] private Transform _curseTags;
    [SerializeField] private Transform _blessingTags;

	public RectTransform VisualTrm { get; private set; }
    public Image VisualImage { get; private set; }

    private int _cardIndex;
    private Vector2 _startOffset;
    private Vector3 _onMouseScale = Vector3.one * 1.2f;

    private DeckUI _contoller;

    private Sequence _turnSeq;

    private int _cost;

	private void Awake()
	{
		VisualTrm = transform.Find("Visual") as RectTransform;
		VisualImage = VisualTrm.GetComponent<Image>();
	}

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

    public void Init(DeckUI controller, Card card, int index)
    {
        Debug.Log(controller);

		_contoller = controller;
		_cardIndex = index;
        cardData = card;

        _cost = cardData.cardSO.cost;
        _costText.text = _cost.ToString();
        _nameText.text = cardData.cardSO.name;
        _descriptionText.text = cardData.cardSO.cardDescription;
        _image.sprite = cardData.cardSO.image;

        IsFront = false;
        _elementsTrm.gameObject.SetActive(false);

        UpdateTag();
    }

    public void AfterInit()
    {
        if (cardData.curses.Contains(ECurse.Envy))
        {
            if (_cardIndex - 1 >= 0) _contoller._cardUIList[_cardIndex - 1].Lock(true);
            if (_cardIndex + 1 < _contoller._cardUIList.Count) _contoller._cardUIList[_cardIndex + 1].Lock(true);
        }
    }

    public void UpdateTag()
    {
        //저주가 존재한다면
        if (cardData.curses.Count > 0)
        {
            foreach (ECurse tag in cardData.curses)
            {
                _curseTags.GetChild((int)tag).gameObject.SetActive(true);
            }
        }
        
        //축복이 존재한다면
        if (cardData.blessings.Count > 0)
        {
            foreach (EBlessing tag in cardData.blessings)
            {
                _blessingTags.GetChild((int)tag).gameObject.SetActive(true);
            }
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

    public bool TryUse()
    {
        if (Cost.TryUse(_cost) == false) return false;

        if (cardData.blessings.Contains(EBlessing.Penance)) _contoller.AddCard(cardData, true);
        if (cardData.curses.Contains(ECurse.Envy))
        {
            if (_cardIndex - 1 >= 0) _contoller._cardUIList[_cardIndex - 1].Lock(false);
            if (_cardIndex + 1 < _contoller._cardUIList.Count) _contoller._cardUIList[_cardIndex + 1].Lock(false);
        }
        Debug.Log("Use!");
        cardData.OnUse();
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
