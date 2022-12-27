using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardObject : MonoBehaviour, IInteractable, IPointerEnterHandler, IPointerExitHandler
{
    private CardData cardData;
    [SerializeField] private Text numberText;
    [SerializeField] private Image cardImage;
    private RectTransform rectTransform;

    public CardData CardData { get => cardData; set => cardData = value; }
    public Text NumberText => numberText;
    public Image CardImage => cardImage;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DOLocalMoveY(100, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOLocalMoveY(0, 0.5f);
    }

    public void SetupCard()
    {
        cardImage.color = cardData.ColorCard;
        
        if (cardData.ActualPower != CardPower.NOTHING)
            numberText.text = GameManager.instance.DictSpecialText[cardData.ActualPower];
        else
            numberText.text = cardData.ActualNumber.ToString();
    }

    public virtual void Interact()
    {
        if (GameManager.instance.IdTurn == 0)
        {
            //Check if its possible
            if (GameManager.instance.CheckPossibleMove(cardData))
            {
                GameManager.instance.ApplyNewCard(cardData);
                Destroy(gameObject);
            }
            else
                Debug.Log("Peupo");
        }
        else
            Debug.Log("Peupo");
    }

}