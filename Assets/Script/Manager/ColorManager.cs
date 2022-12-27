using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;

    [SerializeField] private Color blueColor, redColor, greenColor, yellowColor, blackColor;

    private Dictionary<CardColor, Color> dictColors = new Dictionary<CardColor, Color>();
    public Dictionary<CardColor, Color> DictColors => dictColors;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        #region Color Init
        dictColors.Add(CardColor.BLUE, blueColor);
        dictColors.Add(CardColor.RED, redColor);
        dictColors.Add(CardColor.GREEN, greenColor);
        dictColors.Add(CardColor.YELLOW, yellowColor);
        dictColors.Add(CardColor.BLACK, blackColor);
        #endregion
    }
}
