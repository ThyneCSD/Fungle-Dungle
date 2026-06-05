using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class OnlineLeaderboard : MonoBehaviour
{
    [Header("Player")]
    public string playerName = "Thyne";

    [Header("Debug")]
    public bool autoLoginOnStart = true;

    private bool loggedIn = false;

    void Start()
    {
        PlayFabSettings.staticSettings.TitleId = "1C16ED";

        if (autoLoginOnStart)
        {
            Login();
        }
    }

    public void Login()
    {
#if UNITY_ANDROID
        var request = new LoginWithAndroidDeviceIDRequest
        {
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithAndroidDeviceID(
            request,
            OnLoginSuccess,
            OnError
        );
#else
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(
            request,
            OnLoginSuccess,
            OnError
        );
#endif
    }

    void OnLoginSuccess(LoginResult result)
    {
        loggedIn = true;

        Debug.Log("PlayFab Login Success!");

        var nameRequest = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = playerName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(
            nameRequest,
            r => Debug.Log("Display Name Set: " + playerName),
            OnError
        );
    }

    public void UploadScore(int score)
    {
        if (!loggedIn)
        {
            Debug.LogWarning("Not logged in yet.");
            return;
        }

        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "HighScore",
                    Value = score
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(
            request,
            result =>
            {
                Debug.Log("Score Uploaded: " + score);
            },
            OnError
        );
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(
            request,
            OnLeaderboardReceived,
            OnError
        );
    }

    void OnLeaderboardReceived(GetLeaderboardResult result)
    {
        Debug.Log("===== TOP 10 =====");

        foreach (var player in result.Leaderboard)
        {
            string displayName =
                string.IsNullOrEmpty(player.DisplayName)
                ? "Unknown"
                : player.DisplayName;

            Debug.Log(
                "#" +
                (player.Position + 1) +
                " | " +
                displayName +
                " | " +
                player.StatValue
            );
        }
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    // TEST KNOPPEN IN INSPECTOR
    [ContextMenu("Upload 100")]
    public void TestUpload100()
    {
        UploadScore(100);
    }

    [ContextMenu("Upload 1000")]
    public void TestUpload1000()
    {
        UploadScore(1000);
    }

    [ContextMenu("Get Leaderboard")]
    public void TestLeaderboard()
    {
        GetLeaderboard();
    }
}