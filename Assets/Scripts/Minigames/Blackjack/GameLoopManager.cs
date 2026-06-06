using TMPro;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private Dealer dealer;
    [SerializeField] private Betting betting;
    [SerializeField] private TextMeshProUGUI handValueText;
    [SerializeField] private TextMeshProUGUI dealerValueText;
    [SerializeField] private TextMeshProUGUI gameloopText;
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private GameObject ReplayButton;

    [SerializeField] private GameObject canvas;

    public bool gameEnded = false;

    void Start()
    {
        if (deckManager == null)
            deckManager = FindFirstObjectByType<DeckManager>();
    }

    void Update()
    {
        moneyText.text = "Money: $" + GameState.Money;
        loopCheck();
        Debug.Log(GameState.Money);
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
            gameEnded = true;
        }
        else if (handValue == 21)
        {
            gameloopText.text = "Blackjack! You win!";
            ReplayButton.SetActive(true);
            canvas.GetComponent<CanvasGroup>().interactable = false;

            if (!gameEnded)
            {
                GameState.Money += betting.moneyBetted * 2;
                gameEnded = true;
            }
        }

        if (dealer.CalculateDealerValue() > 21)
        {
            gameloopText.text = "Dealer busts! You win!";
            ReplayButton.SetActive(true);
            canvas.GetComponent<CanvasGroup>().interactable = false;

            if (!gameEnded)
            {
                GameState.Money += betting.moneyBetted * 2;
                gameEnded = true;
            }
        }

        if (dealer.CalculateDealerValue() == 21)
        {
            gameloopText.text = "Dealer has Blackjack! You lose!";
            ReplayButton.SetActive(true);
            canvas.GetComponent<CanvasGroup>().interactable = false;
            gameEnded = true;
        }
    }

    public void OnStandButton()
    {
        if (dealer.CalculateDealerValue() < 17)
        {
            dealer.DrawDealerCard();
        }
        else
        {
            int handValue = deckManager.CalculateHandValue();
            int dealerValue = dealer.CalculateDealerValue();

            if (handValue > dealerValue)
            {
                gameloopText.text = "You win!";
                if (!gameEnded)
                {
                    GameState.Money += betting.moneyBetted * 2;
                    gameEnded = true;
                }
            }
            else if (handValue < dealerValue)
            {
                gameloopText.text = "You lose!";
                gameEnded = true;
            }
            else
            {
                gameloopText.text = "It's a tie!";
                if (!gameEnded)
                {
                    GameState.Money += betting.moneyBetted;
                    gameEnded = true;
                }
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