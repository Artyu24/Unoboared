using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected List<CardData> cardList = new List<CardData>();
    public List<CardData> CardList => cardList;

    public virtual void AddCardToDeck(CardData data)
    {
        cardList.Add(data);
        GameObject objectInstance = Instantiate(GameManager.instance.CardPrefab.gameObject, GameManager.instance.PlayerDeck);
        CardObject cardInstance = objectInstance.GetComponent<CardObject>();
        cardInstance.CardData = data;
        cardInstance.SetupCard();
    }
}
