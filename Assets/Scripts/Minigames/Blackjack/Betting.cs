using TMPro;
using UnityEngine;

public class Betting : MonoBehaviour
{
    [SerializeField] private GameObject canvasGameUI;
    [SerializeField] private GameObject canvasBettingUI;
    [SerializeField] private TMP_InputField codeInput;
    [SerializeField] private TMP_InputField moneyInput;

    public int moneyBetted;
    public void StartGame()
    {
        canvasGameUI.SetActive(true);
        canvasBettingUI.SetActive(false);
    }


    private void Update()
    {
        if (codeInput.text == "Hentai")
        {
            GameState.ResetMoney();
        }
    }

    public void Bet10()
    {
        if (GameState.Money >= 10)
        {
            moneyBetted = 10;
            GameState.Money -= 10;
            StartGame();
        }
    }

    public void Bet20()
    {
        if (GameState.Money >= 20)
        {
            moneyBetted = 20;
            GameState.Money -= 20;
            StartGame();
        }
    }

    public void Bet50()
    {
        if (GameState.Money >= 50)
        {
            moneyBetted = 50;
            GameState.Money -= 50;
            StartGame();
        }
    }

    public void AllIn()
    {
        int money = GameState.Money;
        if (money > 0)
        {
            moneyBetted = money;
            GameState.Money = 0;
            StartGame();
        }
    }

    public void BetCustom()
    {
        int bet;

        if (int.TryParse(moneyInput.text, out bet))
        {
            if (bet > 0 && GameState.Money >= bet)
            {
                moneyBetted = bet;
                GameState.Money -= bet;
                StartGame();
            }
        }
    }
}
