using TMPro;
using UnityEngine;

public class Betting : MonoBehaviour
{
    [SerializeField] private GameObject canvasGameUI;
    [SerializeField] private GameObject canvasBettingUI;
    [SerializeField] private TMP_InputField moneyInput;

    public int moneyBetted;
    public void StartGame()
    {
        canvasGameUI.SetActive(true);
        canvasBettingUI.SetActive(false);
    }


    private void Update()
    {
        if (moneyInput.text == "Hentai")
        {
            PlayerPrefs.SetInt("Money", 100);
        }
    }

    public void Bet10()
    {
        if (PlayerPrefs.GetInt("Money", 0) >= 10)
        {
            moneyBetted = 10;
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 0) - 10);
            StartGame();
        }
    }

    public void Bet20()
    {
        if (PlayerPrefs.GetInt("Money", 0) >= 20)
        {
            moneyBetted = 20;
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 0) - 20);
            StartGame();
        }
    }

    public void Bet50()
    {
        if (PlayerPrefs.GetInt("Money", 0) >= 50)
        {
            moneyBetted = 50;
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 0) - 50);
            StartGame();
        }
    }

    public void AllIn()
    {
        int money = PlayerPrefs.GetInt("Money", 0);
        if (money > 0)
        {
            moneyBetted = money;
            PlayerPrefs.SetInt("Money", 0);
            StartGame();
        }
    }
}
