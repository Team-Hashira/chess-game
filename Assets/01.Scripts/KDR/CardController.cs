using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private CardUI _cardPrefab;

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

            cards[i].VisualTrm.anchoredPosition = Vector2.Lerp(cards[i].VisualTrm.anchoredPosition,
                new Vector2(interval * _targetXPosInterval, _targetYPosInterval), Time.deltaTime * 10);

            if (cards[i].IsHolded == false)
            {
                cards[i].transform.localRotation = Quaternion.Lerp(cards[i].transform.localRotation,
                    Quaternion.Euler(0, 0, -interval * _targetAngleInterval), Time.deltaTime * 10);
                cards[i].transform.localPosition = Vector2.Lerp(cards[i].transform.localPosition,
                    Vector3.zero, Time.deltaTime * 10);
            }
            else
            {
                //_cards[i].transform.localRotation = Quaternion.Lerp(_cards[i].transform.localRotation,
                //    Quaternion.identity, Time.deltaTime * 8);
                cards[i].UpdateUseable();
            }
        }
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
            _removedCards.Add(card);
            card.Use();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isOpen = true;
        _spreadTime = Time.time;
        _targetAngleInterval = angleInterval;
        _targetYPosInterval = yPosInterval;
        _targetXPosInterval = xPosInterval;
        _rectTrm.sizeDelta = new Vector2(1500 * ((float)cards.Count / 5), 1300);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isOpen = false;
        _spreadTime = Time.time;
        _targetAngleInterval = 0f;
        _targetXPosInterval = 0f;
        _targetYPosInterval = 0f;
        _rectTrm.sizeDelta = _startSize;
    }
}
