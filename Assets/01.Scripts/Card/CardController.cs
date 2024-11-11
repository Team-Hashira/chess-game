using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private List<CardUI> _removedCards;
    public List<CardUI> cards;

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

    private int _currentCost = 10;
    [SerializeField] private int _targetCost = 10;

    [SerializeField] private CardUI _cardPrefab;
    [SerializeField] private TextMeshProUGUI _costText;

    [field: SerializeField] public Transform UseAreaTrm { get; private set; }

    private void Awake()
    {
        _rectTrm = transform as RectTransform;
        _startSize = _rectTrm.sizeDelta;
        cards = new List<CardUI>();
        _removedCards = new List<CardUI>();
        GetComponentsInChildren(cards);

        int cardCount = cards.Count;
        for (int i = 0; i < cardCount; i++)
        {
            cards[i].Init(this, i);

            float interval = i - (float)cardCount / 2;

            cards[i].transform.localRotation = Quaternion.Euler(0, 0, -interval * _targetAngleInterval);
            cards[i].VisualTrm.anchoredPosition = new Vector2(interval * _targetXPosInterval, _targetYPosInterval);
        }
        for (int i = 0; i < cardCount; i++)
        {
            cards[i].AfterInit();
        }
    }

    private void Update()
    {
        _removedCards.ForEach(card => cards.Remove(card));
        _removedCards.Clear();

        int cardCount = cards.Count;

        for (int i = 0; i < cardCount; i++)
        {
            if (_spreadTime + i * _spreadDelay > Time.time) continue;

            if (cards[i].IsFront != _isOpen) cards[i].Turn(_isOpen);

            float interval = i - (cardCount / 2 - (cardCount % 2 == 0 ? 0.5f : 0));
            cards[i].UpdateArray(interval, _targetXPosInterval, _targetYPosInterval, _targetAngleInterval);
        }


        UpdateCost();
    }

    public bool TryUseCost(int amount)
    {
        if (_targetCost < amount) return false;

        _targetCost -= amount;
        return true;
    }

    public void UpdateCost()
    {
        if (_currentCost < _targetCost) _currentCost++;
        else if (_currentCost > _targetCost) _currentCost--;
        _costText.text = $"Cost : {_currentCost.ToString()}";
    }

    public void AddCard(CardSO cardSO, bool isPenance = false)
    {
        CardUI card = Instantiate(_cardPrefab, transform);
        card.transform.localPosition = Vector3.zero;
        card.cardSO = cardSO;
        card.Init(this, cards.Count);
        if (isPenance) card.CostModify(-10, Color.yellow);
        cards.Add(card);
    }

    public void SetSelectCard(CardUI card, bool isOn)
    {
        UseAreaTrm.gameObject.SetActive(isOn);

        if (isOn == false && card.IsUseable)
        {
            if (card.Use())
            {
                _removedCards.Add(card);
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
        _rectTrm.sizeDelta = new Vector2(1500 * ((float)cards.Count / 5), 1300);
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
