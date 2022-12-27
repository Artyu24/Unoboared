using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum CardColor
{
    YELLOW,
    BLUE,
    RED,
    GREEN,
    BLACK
}

public enum CardPower
{
    NOTHING,
    REVERSE,
    PASS,
    CHANGE_COLOR,
    DRAW_TWO,
    DRAW_FOUR
}

