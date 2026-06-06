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

        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 100));
    }

    void Update()
    {
        moneyText.text = "Money: $" + PlayerPrefs.GetInt("Money");
        loopCheck();
        Debug.Log(PlayerPrefs.GetInt("Money", 0));
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
            if (gameEnded == false)
            {
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 0) + betting.moneyBetted * 2);
                gameEnded = true;
            }
        }

        if (dealer.CalculateDealerValue() > 21)
        {
            gameloopText.text = "Dealer busts! You win!";
            ReplayButton.SetActive(true);
            canvas.GetComponent<CanvasGroup>().interactable = false;
            if (gameEnded == false)
            {
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 0) + betting.moneyBetted * 2);
                gameEnded = true;
            }
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
                if (gameEnded == false)
                {
                    PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 0) + betting.moneyBetted * 2);
                    gameEnded = true;
                }
            }
            else if (handValue < dealerValue)
            {
                gameloopText.text = "You lose!";
            }
            else
            {
                gameloopText.text = "It's a tie!";
                if (gameEnded == false)
                {
                    PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 0) + betting.moneyBetted);
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