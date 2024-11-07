using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private List<Card> _removedCards;
    private List<Card> _cards;

    public float angleInterval;
    public float xPosInterval;
    public float yPosInterval;

    private RectTransform _rectTrm;
    private Vector2 _startSize;

    private float _spreadTime;
    private float _spreadDelay = 0.03f;

    private bool _isOpen = false;

    [field: SerializeField] public Transform UseAreaTrm { get; private set; }

    private void Awake()
    {
        _rectTrm = transform as RectTransform;
        _startSize = _rectTrm.sizeDelta;
        _cards = new List<Card>();
        _removedCards = new List<Card>();
        GetComponentsInChildren(_cards);

        int cardCount = _cards.Count;
        for (int i = 0; i < cardCount; i++)
        {
            _cards[i].Init(this);

            float interval = i - (float)cardCount / 2;

            _cards[i].transform.localRotation = Quaternion.Euler(0, 0, -interval * angleInterval);
            _cards[i].VisualTrm.anchoredPosition = new Vector2(interval * xPosInterval, yPosInterval);
        }
    }

    private void Update()
    {
        _removedCards.ForEach(card => _cards.Remove(card));
        _removedCards.Clear();

        int cardCount = _cards.Count;

        for (int i = 0; i < cardCount; i++)
        {
            if (_spreadTime + i * _spreadDelay > Time.time) continue;

            if (_cards[i].IsFront != _isOpen) _cards[i].Turn(_isOpen);

            float interval = i - (cardCount / 2 - (cardCount % 2 == 0 ? 0.5f : 0));

            _cards[i].VisualTrm.anchoredPosition = Vector2.Lerp(_cards[i].VisualTrm.anchoredPosition,
                new Vector2(interval * xPosInterval, yPosInterval), Time.deltaTime * 10);

            if (_cards[i].IsHolded == false)
            {
                _cards[i].transform.localRotation = Quaternion.Lerp(_cards[i].transform.localRotation,
                    Quaternion.Euler(0, 0, -interval * angleInterval), Time.deltaTime * 10);
                _cards[i].transform.localPosition = Vector2.Lerp(_cards[i].transform.localPosition,
                    Vector3.zero, Time.deltaTime * 10); 
            }
            else
            {
                //_cards[i].transform.localRotation = Quaternion.Lerp(_cards[i].transform.localRotation,
                //    Quaternion.identity, Time.deltaTime * 8);
                _cards[i].UpdateUseable();
            }
        }
    }

    public void SetSelectCard(Card card, bool isOn)
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
        angleInterval = 6f;
        yPosInterval = 500f;
        xPosInterval = 250f;
        _rectTrm.sizeDelta = new Vector2(1500 * ((float)_cards.Count / 5), 1300);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isOpen = false;
        _spreadTime = Time.time;
        angleInterval = 0f;
        xPosInterval = 0f;
        yPosInterval = 0f;
        _rectTrm.sizeDelta = _startSize;
    }
}
