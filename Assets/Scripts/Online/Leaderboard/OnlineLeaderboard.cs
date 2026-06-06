using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class OnlineLeaderboard : MonoBehaviour
{
    [Header("UI INPUT")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField scoreInput;

    [Header("UI OUTPUT")]
    [SerializeField] private TextMeshProUGUI leaderboardText;

    [Header("Settings")]
    public bool autoLoginOnStart = true;

    private bool loggedIn = false;
    private bool loginInProgress = false;
    private string customId;

    void Awake()
    {
#if UNITY_EDITOR
        customId = "EDITOR_TEST_USER" + SystemInfo.deviceUniqueIdentifier;
#else
        customId = SystemInfo.deviceUniqueIdentifier;
#endif
    }

    void Start()
    {
        PlayFabSettings.staticSettings.TitleId = "1C16ED";

        if (autoLoginOnStart)
            Login();
    }

    // ---------------- LOGIN ----------------
    public void Login()
    {
        if (loginInProgress || loggedIn)
            return;

        loginInProgress = true;

        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFail);
    }

    void OnLoginSuccess(LoginResult result)
    {
        loginInProgress = false;
        loggedIn = true;

        Debug.Log("✅ Login success");
    }

    void OnLoginFail(PlayFabError error)
    {
        loginInProgress = false;

        Debug.LogWarning("❌ Login failed: " + error.ErrorMessage);

        // fallback login
        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = false
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    // ---------------- NAME SYSTEM ----------------
    public void SetDisplayNameFromInput()
    {
        if (!loggedIn)
        {
            Debug.LogWarning("❌ Not logged in yet");
            return;
        }

        if (nameInput == null)
        {
            Debug.LogError("❌ NameInput not assigned in Inspector");
            return;
        }

        string newName = nameInput.text.Trim();

        Debug.Log("➡ Trying to set name: " + newName);

        if (string.IsNullOrEmpty(newName))
        {
            Debug.LogWarning("❌ Name is empty");
            return;
        }

        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = newName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(
            request,
            result =>
            {
                Debug.Log("🔥 NAME SUCCESS: " + newName);
            },
            error =>
            {
                Debug.LogError("🔥 NAME ERROR: " + error.GenerateErrorReport());
            }
        );
    }

    // ---------------- SCORE ----------------
    public void UploadScoreFromInput()
    {
        if (!loggedIn)
        {
            Debug.LogWarning("❌ Not logged in yet");
            return;
        }

        if (scoreInput == null)
        {
            Debug.LogError("❌ ScoreInput not assigned");
            return;
        }

        if (!int.TryParse(scoreInput.text, out int score))
        {
            Debug.LogWarning("❌ Invalid score");
            return;
        }

        UploadScore(score);
    }

    public void UploadScore(int score)
    {
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
            result => Debug.Log("🔥 Score uploaded: " + score),
            OnError
        );
    }

    // ---------------- LEADERBOARD ----------------
    public void GetLeaderboard()
    {
        if (!loggedIn)
        {
            Debug.LogWarning("❌ Not logged in yet");
            return;
        }

        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardReceived, OnError);
    }

    void OnLeaderboardReceived(GetLeaderboardResult result)
    {
        string output = "===== TOP 10 =====\n\n";

        foreach (var player in result.Leaderboard)
        {
            string name = string.IsNullOrEmpty(player.DisplayName)
                ? "Unknown"
                : player.DisplayName;

            output += "#" + (player.Position + 1)
                + " | " + name
                + " | " + player.StatValue + "\n";
        }

        if (leaderboardText != null)
            leaderboardText.text = output;

        Debug.Log(output);
    }

    // ---------------- ERROR ----------------
    void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    // ---------------- DEBUG BUTTONS ----------------
    [ContextMenu("Set Name")]
    public void TestSetName()
    {
        SetDisplayNameFromInput();
    }

    [ContextMenu("Upload Score")]
    public void TestScore()
    {
        UploadScoreFromInput();
    }

    [ContextMenu("Get Leaderboard")]
    public void TestLeaderboard()
    {
        GetLeaderboard();
    }
}