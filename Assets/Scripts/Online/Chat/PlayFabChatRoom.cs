using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Text;

public class PlayFabChatRoom : MonoBehaviour
{
    [Header("UI INPUT")]
    [SerializeField] private TMP_InputField messageInput;
    [SerializeField] private TMP_InputField nameInput;

    [Header("UI OUTPUT")]
    [SerializeField] private TextMeshProUGUI chatText;

    [Header("Settings")]
    [SerializeField] private string sharedGroupId = "GLOBAL_CHAT_ROOM";
    [SerializeField] private float refreshInterval = 3f;
    [SerializeField] private int maxMessages = 20;

    private bool loggedIn;
    private bool joinedGroup;
    private float timer;
    private string customId;

    void Awake()
    {
#if UNITY_EDITOR
        customId = "EDITOR_USER_" + SystemInfo.deviceUniqueIdentifier;
#else
        customId = SystemInfo.deviceUniqueIdentifier;
#endif
    }

    void Start()
    {
        PlayFabSettings.staticSettings.TitleId = "1C16ED";
        Login();
    }

    void Update()
    {
        if (!loggedIn || !joinedGroup) return;

        timer += Time.deltaTime;
        if (timer >= refreshInterval)
        {
            timer = 0;
            RefreshChat();
        }
    }

    #region LOGIN

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request,
            result =>
            {
                loggedIn = true;
                Debug.Log("Login success");

                EnsureGroupExists();
            },
            OnError);
    }

    #endregion

    #region GROUP SETUP

    void EnsureGroupExists()
    {
        var createRequest = new CreateSharedGroupRequest
        {
            SharedGroupId = sharedGroupId
        };

        PlayFabClientAPI.CreateSharedGroup(createRequest,
            result =>
            {
                Debug.Log("Group created");
                AddSelfToGroup();
            },
            error =>
            {
                // If it already exists, we still continue
                Debug.Log("Group already exists or cannot be created, continuing...");
                AddSelfToGroup();
            });
    }

    void AddSelfToGroup()
    {
        var request = new AddSharedGroupMembersRequest
        {
            SharedGroupId = sharedGroupId,
            PlayFabIds = new List<string>
            {
                PlayFabSettings.staticPlayer.PlayFabId
            }
        };

        PlayFabClientAPI.AddSharedGroupMembers(request,
            result =>
            {
                joinedGroup = true;
                Debug.Log("Joined shared group");

                RefreshChat();
            },
            OnError);
    }

    #endregion

    #region SEND MESSAGE

    public void SendMessage()
    {
        if (!loggedIn || !joinedGroup) return;

        string playerName = string.IsNullOrWhiteSpace(nameInput.text)
            ? "Anonymous"
            : nameInput.text.Trim();

        string message = messageInput.text.Trim();

        if (string.IsNullOrEmpty(message)) return;

        string formatted = $"[{playerName}] {message}";

        PlayFabClientAPI.GetSharedGroupData(new GetSharedGroupDataRequest
        {
            SharedGroupId = sharedGroupId
        },
        result =>
        {
            List<string> messages = new List<string>();

            if (result.Data != null && result.Data.ContainsKey("CHAT"))
            {
                messages = JsonUtility.FromJson<ChatWrapper>(result.Data["CHAT"].Value).messages;
            }

            messages.Add(formatted);

            if (messages.Count > maxMessages)
                messages.RemoveAt(0);

            string json = JsonUtility.ToJson(new ChatWrapper { messages = messages });

            PlayFabClientAPI.UpdateSharedGroupData(new UpdateSharedGroupDataRequest
            {
                SharedGroupId = sharedGroupId,
                Data = new Dictionary<string, string>
                {
                    { "CHAT", json }
                }
            },
            r =>
            {
                messageInput.text = "";
                RefreshChat();
            },
            OnError);
        },
        OnError);
    }

    #endregion

    #region REFRESH CHAT

    public void RefreshChat()
    {
        if (!loggedIn || !joinedGroup) return;

        PlayFabClientAPI.GetSharedGroupData(new GetSharedGroupDataRequest
        {
            SharedGroupId = sharedGroupId
        },
        result =>
        {
            StringBuilder sb = new StringBuilder();

            if (result.Data != null && result.Data.ContainsKey("CHAT"))
            {
                var data = JsonUtility.FromJson<ChatWrapper>(result.Data["CHAT"].Value);

                foreach (var msg in data.messages)
                    sb.AppendLine(msg);
            }
            else
            {
                sb.AppendLine("No messages yet...");
            }

            if (chatText != null)
                chatText.text = sb.ToString();
        },
        OnError);
    }

    #endregion

    #region ERROR

    void OnError(PlayFabError error)
    {
        Debug.LogError("PlayFab Error: " + error.GenerateErrorReport());
    }

    #endregion

    [System.Serializable]
    public class ChatWrapper
    {
        public List<string> messages = new List<string>();
    }
}