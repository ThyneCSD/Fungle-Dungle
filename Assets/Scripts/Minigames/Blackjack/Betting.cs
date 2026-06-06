using TMPro;
using UnityEngine;

public class Betting : MonoBehaviour
{
    [SerializeField] private GameObject canvasGameUI;
    [SerializeField] private GameObject canvasBettingUI;
    public void StartGame()
    {
        canvasGameUI.SetActive(true);
        canvasBettingUI.SetActive(false);
    }

    public void Bet10()
    {
        if (PlayerPrefs.GetInt("Money", 0) >= 10)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 0) - 10);
            StartGame();
        }
    }

    public void Bet20()
    {
        if (PlayerPrefs.GetInt("Money", 0) >= 20)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 0) - 20);
            StartGame();
        }
    }

    public void Bet50()
    {
        if (PlayerPrefs.GetInt("Money", 0) >= 50)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 0) - 50);
            StartGame();
        }
    }

    public void AllIn()
    {
        int money = PlayerPrefs.GetInt("Money", 0);
        if (money > 0)
        {
            PlayerPrefs.SetInt("Money", 0);
            StartGame();
        }
    }

}
