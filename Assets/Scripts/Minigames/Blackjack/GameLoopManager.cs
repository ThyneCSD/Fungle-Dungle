using TMPro;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private TextMeshProUGUI handValueText;
    [SerializeField] private TextMeshProUGUI gameloopText;

    void Start()
    {
        if (deckManager == null)
            deckManager = FindFirstObjectByType<DeckManager>();
    }

    void Update()
    {
        int handValue = deckManager.CalculateHandValue();
        handValueText.text = "Hand value: " + handValue;

        if (handValue > 21)
        {
            gameloopText.text = "Bust! You lose.";
        } else if (handValue == 21)
        {
            gameloopText.text = "Blackjack! You win!";
        }
    }
}