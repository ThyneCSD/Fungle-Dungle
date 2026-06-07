using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonChatRoom : MonoBehaviourPunCallbacks
{
    [Header("UI INPUT")]
    [SerializeField] private TMP_InputField messageInput;
    [SerializeField] private TMP_InputField nameInput;

    [Header("UI OUTPUT")]
    [SerializeField] private TextMeshProUGUI chatText;

    [Header("SPECIAL IMAGES")]
    [SerializeField] private RawImage scaryTreeImage;
    [SerializeField] private RawImage danielImage;
    [SerializeField] private RawImage fortniteImage;

    [Header("SPECIAL SOUNDS")]
    [SerializeField] private AudioSource scaryTreeSound;
    [SerializeField] private AudioSource danielSound;
    [SerializeField] private AudioSource fortniteSound;

    [Header("EFFECT SETTINGS")]
    [SerializeField] private float effectDuration = 1f;
    [SerializeField] private float fortniteDelay = 1f;

    [Header("SETTINGS")]
    [SerializeField] private string roomName = "MainRoom";
    [SerializeField] private int maxMessages = 20;

    private bool isConnected;
    private bool isShowingEffect;

    private List<string> messages = new List<string>();

    void Start()
    {
        if (scaryTreeImage != null)
            scaryTreeImage.gameObject.SetActive(false);

        if (danielImage != null)
            danielImage.gameObject.SetActive(false);

        if (fortniteImage != null)
            fortniteImage.gameObject.SetActive(false);

        Connect();
    }

    void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom(
            roomName,
            new RoomOptions { MaxPlayers = 20 },
            TypedLobby.Default
        );

        Debug.Log("Connected to Photon Master");
    }

    public override void OnJoinedRoom()
    {
        isConnected = true;
        RefreshChat();
        Debug.Log("Joined Room: " + roomName);
    }

    public void SendMessage()
    {
        if (!isConnected)
            return;

        if (string.IsNullOrWhiteSpace(messageInput.text))
            return;

        string playerName = string.IsNullOrWhiteSpace(nameInput.text)
            ? "Anonymous"
            : nameInput.text.Trim();

        string input = messageInput.text.Trim();
        string msg;

        if (input.ToLower() == "!time")
        {
            string time = System.DateTime.Now.ToString("HH:mm:ss");
            msg = $"[{playerName}] 🕒 {time}";
        }
        else
        {
            msg = $"[{playerName}] {input}";
        }

        photonView.RPC("ReceiveMessage", RpcTarget.All, msg);

        messageInput.text = "";
    }

    [PunRPC]
    void ReceiveMessage(string msg)
    {
        string lower = msg.ToLower();

        bool special =
            lower.Contains("scary tree") ||
            lower.Contains("daniel") ||
            lower.Contains("fortnite");

        // Alleen normale berichten in de chat tonen
        if (!special)
        {
            messages.Add(msg);

            if (messages.Count > maxMessages)
                messages.RemoveAt(0);
        }

        RefreshChat();

        if (lower.Contains("scary tree"))
        {
            StartCoroutine(ShowEffect(scaryTreeImage, scaryTreeSound, 0f));
        }
        else if (lower.Contains("daniel"))
        {
            StartCoroutine(ShowEffect(danielImage, danielSound, 0f));
        }
        else if (lower.Contains("fortnite"))
        {
            StartCoroutine(ShowEffect(fortniteImage, fortniteSound, fortniteDelay));
        }
    }

    void RefreshChat()
    {
        if (chatText != null)
            chatText.text = string.Join("\n", messages);
    }

    IEnumerator ShowEffect(RawImage image, AudioSource sound, float delay)
    {
        if (isShowingEffect)
            yield break;

        isShowingEffect = true;

        // Geluid direct afspelen
        if (sound != null)
            sound.Play();

        // Alleen de afbeelding uitstellen
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        if (image != null)
        {
            image.gameObject.SetActive(true);

            Color c = image.color;
            c.a = 1f;
            image.color = c;
        }

        yield return new WaitForSeconds(effectDuration);

        if (image != null)
            image.gameObject.SetActive(false);

        isShowingEffect = false;
    }
}