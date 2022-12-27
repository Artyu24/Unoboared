using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CardData
{
    private CardColor actualColor;
    private CardPower actualPower;
    private int actualNumber;
    private Color colorCard;

    public CardColor ActualColor => actualColor;
    public CardPower ActualPower => actualPower;
    public int ActualNumber => actualNumber;
    public Color ColorCard => colorCard;

    public CardData (CardColor actualColor, CardPower actualPower)
    {
        this.actualColor = actualColor;
        this.actualPower = actualPower;
        this.actualNumber = -1;
        this.colorCard = ColorManager.instance.DictColors[actualColor];
    }

    public CardData(CardColor actualColor, int actualNumber)
    {
        this.actualColor = actualColor;
        this.actualPower = CardPower.NOTHING;
        this.actualNumber = actualNumber;
        this.colorCard = ColorManager.instance.DictColors[actualColor];
    }
}
