using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<CardUI> _cardUIList;

    private float _targetAngleInterval;
    private float _targetXPosInterval;
    private float _targetYPosInterval;
    public float angleInterval;
    public float xPosInterval;
    public float yPosInterval;

    private RectTransform _rectTrm;
    private Vector2 _startSize;

    private float _spreadTime;
    private float _spreadDelay = 0.03f;

    private bool _isOpen = false;

    [SerializeField] private CardUI _cardPrefab;
    [SerializeField] private TextMeshProUGUI _costText;

    [field: SerializeField] public Transform UseAreaTrm { get; private set; }

    private List<Card> CardDataList => Deck.GetCurrentCards();

	private void Start()
	{
		_rectTrm = transform as RectTransform;
		_startSize = _rectTrm.sizeDelta;
		_cardUIList = new List<CardUI>();
        GetComponentsInChildren(_cardUIList);
        CardUIInit();
	}

	private void CardUIInit()
    {
        int cardCount = Mathf.Clamp(CardDataList.Count, 0, 5);
        Debug.Log(cardCount);
		for (int i = 0; i < cardCount; i++)
        {
			_cardUIList[i].Init(this, CardDataList[i], i);

			float interval = i - (float)cardCount / 2;

            _cardUIList[i].transform.localRotation = Quaternion.Euler(0, 0, -interval * _targetAngleInterval);
            _cardUIList[i].VisualTrm.anchoredPosition = new Vector2(interval * _targetXPosInterval, _targetYPosInterval);
        }
        for (int i = 0; i < cardCount; i++)
        {
            _cardUIList[i].AfterInit();
        }
    }

    private void Update()
    {
        //_removedCardUIList.ForEach(card => _cardUIList.Remove(card));
        //_removedCardUIList.Clear();

        int cardCount = _cardUIList.Count;

        for (int i = 0; i < cardCount; i++)
        {
            if (_spreadTime + i * _spreadDelay > Time.time) continue;
            if (_cardUIList[i].IsFront != _isOpen) _cardUIList[i].Turn(_isOpen);

            float interval = i - (cardCount / 2 - (cardCount % 2 == 0 ? 0.5f : 0));
            _cardUIList[i].UpdateArray(interval, _targetXPosInterval, _targetYPosInterval, _targetAngleInterval);
        }

		_costText.text = $"Cost : {Cost.Get().ToString()}";
	}

    public void AddCard(Card cardData, bool isPenance = false)
    {
        CardUI card = Instantiate(_cardPrefab, transform);
        card.transform.localPosition = Vector3.zero;

        card.Init(this, cardData, _cardUIList.Count);

        if (isPenance) card.CostModify(-10, Color.yellow);
        _cardUIList.Add(card);
    }

    public void SetSelectCard(CardUI card, bool isOn)
    {
        UseAreaTrm.gameObject.SetActive(isOn);

        if (isOn == false && card.IsUseable)
        {
            if (card.TryUse())
            {
                //_removedCardUIList.Add(card);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UseAreaTrm.gameObject.activeSelf) return;
        _isOpen = true;
        _spreadTime = Time.time;
        _targetAngleInterval = angleInterval;
        _targetYPosInterval = yPosInterval;
        _targetXPosInterval = xPosInterval;
        _rectTrm.sizeDelta = new Vector2(1500 * ((float)_cardUIList.Count / 5), 1300);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		if (UseAreaTrm.gameObject.activeSelf) return;
        _isOpen = false;
        _spreadTime = Time.time;
        _targetAngleInterval = 0f;
        _targetXPosInterval = 0f;
        _targetYPosInterval = 0f;
        _rectTrm.sizeDelta = _startSize;
    }
}
