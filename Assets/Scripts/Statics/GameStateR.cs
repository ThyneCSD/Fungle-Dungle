using UnityEngine;

public static class GameStateR
{
    private const string lapTimeKey = "LAPTIME";

    public static float LapTime
    {
        get => PlayerPrefs.GetFloat(lapTimeKey, Mathf.Infinity);
        set
        {
            PlayerPrefs.SetFloat(lapTimeKey, value);
            PlayerPrefs.Save();
        }
    }

    public static void ResetLapTime()
    {
        PlayerPrefs.DeleteKey(lapTimeKey);
    }
}