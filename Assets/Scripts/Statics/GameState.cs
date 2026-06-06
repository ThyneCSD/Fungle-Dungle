using UnityEngine;

public static class GameState
{
    private const string MoneyKey = "MONEY";

    public static int Money
    {
        get => PlayerPrefs.GetInt(MoneyKey, 100);
        set
        {
            PlayerPrefs.SetInt(MoneyKey, value);
            PlayerPrefs.Save();
        }
    }

    public static void ResetMoney()
    {
        Money = 100;
    }
}