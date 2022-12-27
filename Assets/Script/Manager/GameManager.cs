using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private Transform playerDeck;
    [SerializeField] private Text turnText;
    [SerializeField] private Text winText;
    [SerializeField] private Image directionImage;
    [SerializeField] private Sprite clockwiseSprite, antiClockwiseSprite;
    public GameObject Canvas => canvas;
    public Transform PlayerDeck => playerDeck;

    [Header("Deck")]
    [SerializeField] private int numberDeck = 1;
    [SerializeField] private int numberCardsPerDeck = 7;
    private CardObject cardPrefab;
    private List<CardData> cardDeckList = new List<CardData>();
    public CardObject CardPrefab => cardPrefab;
    public List<CardData> CardDeckList => cardDeckList;
    
    [Header("Player")]
    public List<Player> playersList = new List<Player>();
    [SerializeField] private GameObject changeColorObject;

    [Header("Game Object")]
    [SerializeField] private Transform gameCardObject;
    private Image actualGameCardImage;
    private Text actualGameCardText;
    private CardData actualGameCard;
    public CardData ActualGameCard => actualGameCard;

    [Header("Game Data")]
    private int idTurn = 0;
    private bool clockwise = true;
    private bool mustPass = false;
    private bool canChangeColor = false;
    private int drawCard = 0;
    private bool mustDraw = false;
    public int IdTurn => idTurn;
    public bool CanChangeColor => canChangeColor;
    public int DrawCard => drawCard;

    [Header("Card")] 
    [SerializeField] private string reverseText;
    [SerializeField] private string passText, changeColorText, drawTwoText, drawFourText;
    private Dictionary<CardPower, string> dictSpecialText = new Dictionary<CardPower, string>();
    public Dictionary<CardPower, string> DictSpecialText => dictSpecialText;

    #region INIT
    private void Awake()
    {
        if (instance == null)
            instance = this;

        dictSpecialText.Add(CardPower.REVERSE, reverseText);
        dictSpecialText.Add(CardPower.PASS, passText);
        dictSpecialText.Add(CardPower.CHANGE_COLOR, changeColorText);
        dictSpecialText.Add(CardPower.DRAW_TWO, drawTwoText);
        dictSpecialText.Add(CardPower.DRAW_FOUR, drawFourText);
    }

    private void Start()
    {
        cardPrefab = Resources.Load<CardObject>("CardObject");

        actualGameCardImage = gameCardObject.GetChild(0).GetComponent<Image>();
        actualGameCardText = gameCardObject.GetChild(2).GetComponent<Text>();

        //Deck Init
        for (int i = 0; i < numberDeck; i++)
        {
            CreateColorCard(CardColor.BLUE);
            CreateColorCard(CardColor.RED);
            CreateColorCard(CardColor.GREEN);
            CreateColorCard(CardColor.YELLOW);
        }
        ShuffleDeck();

        //Distribution
        for (int i = 0; i < playersList.Count; i++)
        {
            for (int j = 0; j < numberCardsPerDeck; j++)
            {
                playersList[i].AddCardToDeck(ChooseFirstCard());
            }
        }

        //Game Set
        actualGameCard = ChooseFirstCard();
        while (actualGameCard.ActualPower != CardPower.NOTHING)
        {
            cardDeckList.Add(actualGameCard);
            actualGameCard = ChooseFirstCard();
        }
        SetupGameCard();
        ShuffleDeck();
    }

    private void CreateColorCard(CardColor cardColorState)
    {
        Color actualColor = ColorManager.instance.DictColors[cardColorState];

        for (int i = 0; i < 10; i++)
        {
            cardDeckList.Add(new CardData(cardColorState, i));
        }
        cardDeckList.Add(new CardData(cardColorState, CardPower.PASS));
        cardDeckList.Add(new CardData(cardColorState, CardPower.REVERSE));
        cardDeckList.Add(new CardData(cardColorState, CardPower.DRAW_TWO));
        cardDeckList.Add(new CardData(CardColor.BLACK, CardPower.CHANGE_COLOR));
        cardDeckList.Add(new CardData(CardColor.BLACK, CardPower.DRAW_FOUR));
    }
    #endregion

    #region GAME

    #region DECK
    public CardData ChooseFirstCard()
    {
        CardData cardChoose = cardDeckList[0];
        cardDeckList.Remove(cardChoose);
        return cardChoose;
    }

    public void PickUpCard()
    {
        playersList[idTurn].AddCardToDeck(ChooseFirstCard());

        NextTurn();
    }

    public void DrawMultipleCard()
    {
        for (int i = 0; i < drawCard; i++)
        {
            playersList[idTurn].AddCardToDeck(ChooseFirstCard());
        }

        drawCard = 0;

        NextTurn();
        return;
    }
    #endregion

    #region PLAY
    public bool CheckPossibleMove(CardData data)
    {
        if (
            ((data.ActualNumber == actualGameCard.ActualNumber || data.ActualColor == actualGameCard.ActualColor) && data.ActualPower == CardPower.NOTHING) 
            || (data.ActualPower != CardPower.NOTHING && drawCard == 0 && (data.ActualPower == actualGameCard.ActualPower || data.ActualColor == actualGameCard.ActualColor))
            || (data.ActualPower == CardPower.DRAW_TWO && data.ActualPower == actualGameCard.ActualPower)
            || data.ActualColor == CardColor.BLACK && drawCard == 0)
        {
            if (data.ActualPower != CardPower.NOTHING)
            {
                switch (data.ActualPower)
                {
                    case CardPower.PASS:
                        mustPass = true;
                        break;
                    case CardPower.REVERSE:
                        clockwise = !clockwise;
                        if (clockwise)
                            directionImage.sprite = clockwiseSprite;
                        else
                            directionImage.sprite = antiClockwiseSprite;
                        break;
                    case CardPower.CHANGE_COLOR:
                        canChangeColor = true;
                        break;
                    case CardPower.DRAW_FOUR:
                        canChangeColor = true;
                        drawCard += 4;
                        mustDraw = true;
                        break;
                }
            }
            return true;
        }

        return false;
    }

    public void ApplyNewCard(CardData cardUse)
    {
        playersList[idTurn].CardList.Remove(cardUse);

        if (actualGameCard.ActualPower == CardPower.CHANGE_COLOR || actualGameCard.ActualPower == CardPower.DRAW_FOUR)
            actualGameCard = new CardData(CardColor.BLACK, actualGameCard.ActualPower);

        cardDeckList.Add(actualGameCard);
        actualGameCard = cardUse;
        SetupGameCard();

        if (actualGameCard.ActualPower == CardPower.DRAW_TWO)
            drawCard += 2;

        if (canChangeColor && playersList[idTurn].GetComponent<Bot>() == null)
            changeColorObject.SetActive(true);
        else
            NextTurn();
    }

    public void ChangeColorButton(int i)
    {
        ChangeColor((CardColor)i);
    }

    public void ChangeColor(CardColor cardColor)
    {
        actualGameCard = new CardData(cardColor, actualGameCard.ActualPower);
        canChangeColor = false;
        SetupGameCard();

        if (playersList[idTurn].GetComponent<Bot>() == null)
        {
            changeColorObject.SetActive(false);
            NextTurn();
        }
    }
    #endregion

    #region TURN
    private void NextTurn()
    {
        if (playersList[idTurn].CardList.Count == 0)
        {
            winText.text = playersList[idTurn].gameObject.name + " Win !";
            winText.gameObject.SetActive(true);
            return;
        }

        if (clockwise)
        {
            Clockwise();
            if (mustPass)
            {
                Clockwise();
                mustPass = false;
            }
        }
        else
        {
            AntiClockwise();
            if (mustPass)
            {
                AntiClockwise();
                mustPass = false;
            }
        }

        if (mustDraw)
        {
            mustDraw = false;
            DrawMultipleCard();
            return;
        }

        turnText.text = playersList[idTurn].gameObject.name;

        if (playersList[idTurn].GetComponent<Bot>() != null)
        {
            StartCoroutine(BotTurn());
        }
        else
        {
            if (drawCard != 0)
            {
                for (int i = 0; i < playersList[idTurn].CardList.Count; i++)
                {
                    if (playersList[idTurn].CardList[i].ActualPower == CardPower.DRAW_TWO)
                        return;
                }
                DrawMultipleCard();
            }
        }
    }

    private IEnumerator BotTurn()
    {
        yield return new WaitForSeconds(2f);
        Bot bot = playersList[idTurn] as Bot;
        bot.PlayTurn();
    }

    private void Clockwise()
    {
        idTurn++;
        if (idTurn >= playersList.Count)
            idTurn = 0;
    }
    
    private void AntiClockwise()
    {
        idTurn--;
        if (idTurn < 0)
            idTurn = playersList.Count - 1;
    }

    #endregion

    #endregion

    #region TOOLS
    private void ShuffleDeck()
    {
        for (int i = 0; i < cardDeckList.Count; i++)
        {
            int random = Random.Range(1, cardDeckList.Count);
            CardData temp = cardDeckList[random];
            cardDeckList[random] = cardDeckList[0];
            cardDeckList[0] = temp;
        }
    }

    private void SetupGameCard()
    {
        actualGameCardImage.color = actualGameCard.ColorCard;

        if (actualGameCard.ActualPower != CardPower.NOTHING)
            actualGameCardText.text = dictSpecialText[actualGameCard.ActualPower];
        else
            actualGameCardText.text = actualGameCard.ActualNumber.ToString();
    }

    #endregion
}
