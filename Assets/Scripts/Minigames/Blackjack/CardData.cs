using UnityEngine;

public class CardData : MonoBehaviour
{
    [Header("Card Info")]
    public string cardName;

    [Range(1, 11)]
    public int cardValue = 1;

    public bool isAce;
}