using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Collections;

public class OnlineLeaderboardRacing : MonoBehaviour
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

    private const int MAX_SCORE = 100000000;

    void Awake()
    {
#if UNITY_EDITOR
        customId = "EDITOR_TEST_USER_" + SystemInfo.deviceUniqueIdentifier;
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

        PlayFabClientAPI.LoginWithCustomID(
            request,
            OnLoginSuccess,
            OnLoginFail
        );
    }

    void OnLoginSuccess(LoginResult result)
    {
        loginInProgress = false;
        loggedIn = true;

        UploadLapTime();

        Debug.Log("Logged into PlayFab!");
    }

    void OnLoginFail(PlayFabError error)
    {
        loginInProgress = false;

        Debug.LogError(error.GenerateErrorReport());

        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = false
        };

        PlayFabClientAPI.LoginWithCustomID(
            request,
            OnLoginSuccess,
            OnError
        );
    }

    public void SetDisplayNameFromInput()
    {
        if (!loggedIn)
            return;

        string newName = nameInput.text.Trim();

        if (string.IsNullOrEmpty(newName))
            return;

        PlayFabClientAPI.UpdateUserTitleDisplayName(
            new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = newName
            },
            result =>
            {
                Debug.Log("Name updated!");
            },
            OnError
        );
    }

    public void UploadLapTime()
    {
        if (!loggedIn)
            return;

        if (GameStateR.LapTime == Mathf.Infinity)
            return;

        int score = MAX_SCORE - Mathf.RoundToInt(GameStateR.LapTime * 1000f);

        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "HighScoreRacing",
                    Value = score
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(
            request,
            result => Debug.Log("Lap uploaded!"),
            OnError
        );
    }

    public void GetLeaderboard()
    {
        if (!loggedIn)
            return;

        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScoreRacing",
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
        string output = "===== FASTEST LAPS =====\n\n";

        foreach (var player in result.Leaderboard)
        {
            string name = string.IsNullOrEmpty(player.DisplayName)
                ? "Unknown"
                : player.DisplayName;

            float lapTime =
                (MAX_SCORE - player.StatValue) / 1000f;

            output += "#" + (player.Position + 1)
                + " | "
                + name
                + " | "
                + lapTime.ToString("F2")
                + "s\n";
        }

        leaderboardText.text = output;
    }

    IEnumerator RefreshBoard()
    {
        while (true)
        {
            if (loggedIn)
                GetLeaderboard();

            yield return new WaitForSeconds(3f);
        }
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    [ContextMenu("Upload Lap")]
    public void TestUpload()
    {
        UploadLapTime();
    }

    [ContextMenu("Get Leaderboard")]
    public void TestLeaderboard()
    {
        GetLeaderboard();
    }

    [ContextMenu("Set Name")]
    public void TestName()
    {
        SetDisplayNameFromInput();
    }
}
