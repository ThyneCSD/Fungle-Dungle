using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Text;
using System.Collections;

public class PlayFabChatRoom : MonoBehaviour
{
    [Header("UI INPUT")]
    [SerializeField] private TMP_InputField messageInput;
    [SerializeField] private TMP_InputField nameInput;

    [Header("UI OUTPUT")]
    [SerializeField] private TextMeshProUGUI chatText;

    [Header("JUMPSCARE")]
    [SerializeField] private RawImage jumpscareImage;
    [SerializeField] private AudioSource jumpscareSound;

    [Header("SETTINGS")]
    [SerializeField] private string sharedGroupId = "GLOBAL_CHAT_ROOM";
    [SerializeField] private float refreshInterval = 3f;
    [SerializeField] private int maxMessages = 20;

    private bool loggedIn;
    private bool joinedGroup;
    private bool isJumpscaring;
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

        SetJumpscareAlpha(0f);
        if (jumpscareImage != null)
            jumpscareImage.gameObject.SetActive(false);
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

    #region GROUP

    void EnsureGroupExists()
    {
        var request = new CreateSharedGroupRequest
        {
            SharedGroupId = sharedGroupId
        };

        PlayFabClientAPI.CreateSharedGroup(request,
            r =>
            {
                AddSelfToGroup();
            },
            e =>
            {
                AddSelfToGroup();
            });
    }

    void AddSelfToGroup()
    {
        var request = new AddSharedGroupMembersRequest
        {
            SharedGroupId = sharedGroupId,
            PlayFabIds = new List<string> { PlayFabSettings.staticPlayer.PlayFabId }
        };

        PlayFabClientAPI.AddSharedGroupMembers(request,
            r =>
            {
                joinedGroup = true;
                RefreshChat();
            },
            OnError);
    }

    #endregion

    #region SEND

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

    #region REFRESH + SECRET

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
            bool triggerJumpscare = false;

            if (result.Data != null && result.Data.ContainsKey("CHAT"))
            {
                var data = JsonUtility.FromJson<ChatWrapper>(result.Data["CHAT"].Value);

                List<string> cleanedMessages = new List<string>();

                foreach (string msg in data.messages)
                {
                    if (msg.ToLower().Contains("scary tree"))
                    {
                        triggerJumpscare = true;
                        continue; // ❌ remove it from chat
                    }

                    cleanedMessages.Add(msg);
                    sb.AppendLine(msg);
                }

                // 🔥 overwrite chat WITHOUT trigger message
                if (triggerJumpscare)
                {
                    string json = JsonUtility.ToJson(new ChatWrapper { messages = cleanedMessages });

                    PlayFabClientAPI.UpdateSharedGroupData(new UpdateSharedGroupDataRequest
                    {
                        SharedGroupId = sharedGroupId,
                        Data = new Dictionary<string, string>
                        {
                        { "CHAT", json }
                        }
                    }, null, OnError);
                }
            }
            else
            {
                sb.AppendLine("No messages yet...");
            }

            if (chatText != null)
                chatText.text = sb.ToString();

            if (triggerJumpscare)
            {
                StartCoroutine(PlayJumpscare());
            }
        },
        OnError);
    }

    #endregion

    #region JUMPSCARE

    IEnumerator PlayJumpscare()
    {
        if (isJumpscaring) yield break;
        isJumpscaring = true;

        jumpscareImage.gameObject.SetActive(true);
        SetJumpscareAlpha(1f);

        if (jumpscareSound != null)
            jumpscareSound.Play();

        yield return new WaitForSeconds(0.5f);

        float t = 0f;
        float duration = 1.2f;

        while (t < duration)
        {
            t += Time.deltaTime;
            SetJumpscareAlpha(Mathf.Lerp(1f, 0f, t / duration));
            yield return null;
        }

        SetJumpscareAlpha(0f);
        jumpscareImage.gameObject.SetActive(false);

        isJumpscaring = false;
    }

    void SetJumpscareAlpha(float a)
    {
        Color c = jumpscareImage.color;
        c.a = a;
        jumpscareImage.color = c;
    }

    #endregion

    #region ERROR

    void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    #endregion

    [System.Serializable]
    public class ChatWrapper
    {
        public List<string> messages = new List<string>();
    }
}