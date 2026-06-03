using TMPro;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private Dealer dealer;
    [SerializeField] private TextMeshProUGUI handValueText;
    [SerializeField] private TextMeshProUGUI dealerValueText;
    [SerializeField] private TextMeshProUGUI gameloopText;

    [SerializeField] private GameObject ReplayButton;

    [SerializeField] private GameObject canvas;

    void Start()
    {
        if (deckManager == null)
            deckManager = FindFirstObjectByType<DeckManager>();
    }

    void Update()
    {
        loopCheck();
    }

    private void loopCheck()
    {
        int handValue = deckManager.CalculateHandValue();
        handValueText.text = "Hand value: " + handValue;

        int dealerValue = dealer.CalculateDealerValue();
        dealerValueText.text = "Dealer value: " + dealerValue;

        if (handValue > 21)
        {
            gameloopText.text = "Bust! You lose.";
            ReplayButton.SetActive(true);
            canvas.GetComponent<CanvasGroup>().interactable = false;
        }
        else if (handValue == 21)
        {
            gameloopText.text = "Blackjack! You win!";
            ReplayButton.SetActive(true);
            canvas.GetComponent<CanvasGroup>().interactable = false;
        }

        if (dealer.CalculateDealerValue() > 21)
        {
            gameloopText.text = "Dealer busts! You win!";
            ReplayButton.SetActive(true);
            canvas.GetComponent<CanvasGroup>().interactable = false;
        }
        if (dealer.CalculateDealerValue() == 21)
        {
            gameloopText.text = "Dealer has Blackjack! You lose!";
            ReplayButton.SetActive(true);
            canvas.GetComponent<CanvasGroup>().interactable = false;
        }
    }

    public void OnStandButton()
    {
        if (dealer.CalculateDealerValue() < 17)
        {
            dealer.DrawDealerCard();
        } else
        {
            int handValue = deckManager.CalculateHandValue();
            int dealerValue = dealer.CalculateDealerValue();
            if (handValue > dealerValue)
            {
                gameloopText.text = "You win!";
            }
            else if (handValue < dealerValue)
            {
                gameloopText.text = "You lose!";
            }
            else
            {
                gameloopText.text = "It's a tie!";
            }
            ReplayButton.SetActive(true);
            canvas.GetComponent<CanvasGroup>().interactable = false;
        }
    }

    public void OnReplayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}