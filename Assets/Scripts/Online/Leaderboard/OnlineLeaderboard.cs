using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Collections;

public class OnlineLeaderboard : MonoBehaviour
{
    [Header("UI INPUT")]
    [SerializeField] private TMP_InputField nameInput;

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

        StartCoroutine(RefreshBoard());
    }

    private void Update()
    {
        RefreshBoard();
    }

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

        UploadScore(GameState.Money);

        Debug.Log("Login success ez as lah");
    }

    void OnLoginFail(PlayFabError error)
    {
        loginInProgress = false;

        Debug.LogWarning(" Login FOKING FAILED: " + error.ErrorMessage);

        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = false
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    public void SetDisplayNameFromInput()
    {
        if (!loggedIn)
        {
            Debug.LogWarning("Not logged in brah");
            return;
        }

        if (nameInput == null)
        {
            Debug.LogError("NameInput not assigned in Inspector");
            return;
        }

        string newName = nameInput.text.Trim();

        Debug.Log("Trying to set name: " + newName);

        if (string.IsNullOrEmpty(newName))
        {
            Debug.LogWarning("Name is empty non extiasta");
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

    public void UploadScoreFromInput()
    {
        if (!loggedIn)
        {
            Debug.LogWarning("Not logged in brah");
            return;
        }
        UploadScore(GameState.Money);
    }

    public void UploadScore(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "HighScore2",
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

    public void GetLeaderboard()
    {
        if (!loggedIn)
        {
            Debug.LogWarning("Not logged in brah");
            return;
        }

        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScore2",
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

    private IEnumerator RefreshBoard()
    {
        while (true)
        {
            GetLeaderboard();
            yield return new WaitForSeconds(3f);
            GetLeaderboard();
        }
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

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