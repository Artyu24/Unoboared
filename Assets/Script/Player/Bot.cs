using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bot : Player
{
    [SerializeField] private Text nbrCardText;

    public override void AddCardToDeck(CardData data)
    {
        cardList.Add(data);
        nbrCardText.text = cardList.Count.ToString();
    }

    public void PlayTurn()
    {
        if (GameManager.instance.DrawCard == 0)
        {
            for (int i = 0; i < cardList.Count; i++)
            {
                if (GameManager.instance.CheckPossibleMove(cardList[i]))
                {
                    GameManager.instance.ApplyNewCard(cardList[i]);
                    
                    if(GameManager.instance.CanChangeColor)
                        ChangeColorBot();

                    nbrCardText.text = cardList.Count.ToString();
                    return;
                }
            }

            GameManager.instance.PickUpCard();
        }
        else
        {
            for (int i = 0; i < cardList.Count; i++)
            {
                if (cardList[i].ActualPower == CardPower.DRAW_TWO)
                {
                    GameManager.instance.ApplyNewCard(cardList[i]);
                    nbrCardText.text = cardList.Count.ToString();
                    return;
                }
            }
            GameManager.instance.DrawMultipleCard();
        }

        nbrCardText.text = cardList.Count.ToString();
    }

    private void ChangeColorBot()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i].ActualColor != CardColor.BLACK)
            {
                GameManager.instance.ChangeColor(cardList[i].ActualColor);
                return;
            }
        }

        int random = Random.Range(0, 4);
        GameManager.instance.ChangeColor((CardColor)random);
    }
}
