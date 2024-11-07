using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using static UnityEngine.Rendering.DebugUI;

public class CardController : MonoBehaviour
{
    private List<Card> _removedCards;
    private List<Card> _cards;

    public float angleInterval;
    public float xPosInterval;
    public float yPosInterval;

    [SerializeField] private Transform _useAreaTrm;

    private void Awake()
    {
        _cards = new List<Card>();
        _removedCards = new List<Card>();
        GetComponentsInChildren(_cards);

        int cardCount = _cards.Count;
        for (int i = 0; i < cardCount; i++)
        {
            _cards[i].Init(this, i);

            float interval = i - (float)cardCount / 2;

            _cards[i].transform.localRotation = Quaternion.Euler(0, 0, -interval * angleInterval);
            _cards[i].VisualTrm.anchoredPosition = new Vector2(interval * xPosInterval, yPosInterval);

            _cards[i].SetStartValue();
        }
    }

    private void Update()
    {
        _removedCards.ForEach(card => _cards.Remove(card));
        _removedCards.Clear();

        int cardCount = _cards.Count;

        for (int i = 0; i < cardCount; i++)
        {
            float interval = i - (cardCount / 2 - (cardCount % 2 == 0 ? 0.5f : 0));
            if (_cards[i].onMouse == false)
            {
                _cards[i].VisualTrm.anchoredPosition = Vector2.Lerp(_cards[i].VisualTrm.anchoredPosition,
                    new Vector2(interval * xPosInterval, yPosInterval), Time.deltaTime * 8);
            }
            if (_cards[i].isSelected == false)
            {
                _cards[i].transform.localRotation = Quaternion.Lerp(_cards[i].transform.localRotation,
                    Quaternion.Euler(0, 0, -interval * angleInterval), Time.deltaTime * 8);
                _cards[i].transform.localPosition = Vector2.Lerp(_cards[i].transform.localPosition,
                    Vector3.zero, Time.deltaTime * 8);
            }
            else
            {
                _cards[i].SetUseable(Vector3.Magnitude(_useAreaTrm.position - _cards[i].VisualTrm.position) < 250);
            }
        }
    }

    public void SetSelectCard(Card card, bool isOn)
    {
        //_useAreaTrm.gameObject.SetActive(isOn);

        if (isOn == false && card.isUseable)
        {
            _removedCards.Add(card);
            card.Use();
        }
    }
}
