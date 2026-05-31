using TMPro;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private TextMeshProUGUI GameloopText;

    void Start()
    {
        deckManager = GetComponent<DeckManager>();
        GameloopText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        GameloopText.text = "Hand value: " + deckManager.CalculateHandValue();

        if (deckManager.CalculateHandValue() > 21)
        {
            GameloopText.text = "Bust! You lose.";
        }
    }
}
