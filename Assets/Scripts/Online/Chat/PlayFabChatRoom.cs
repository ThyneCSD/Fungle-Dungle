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

    [Header("JUMPSCARE")]
    [SerializeField] private RawImage jumpscareImage;
    [SerializeField] private AudioSource jumpscareSound;

    [Header("SETTINGS")]
    [SerializeField] private string roomName = "MainRoom";
    [SerializeField] private int maxMessages = 20;

    private bool isConnected;
    private bool isScaring;

    private List<string> messages = new List<string>();

    void Start()
    {
        if (jumpscareImage != null)
            jumpscareImage.gameObject.SetActive(false);

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
        if (!isConnected) return;
        if (string.IsNullOrWhiteSpace(messageInput.text)) return;

        string name = string.IsNullOrWhiteSpace(nameInput.text)
            ? "Anonymous"
            : nameInput.text.Trim();

        string input = messageInput.text.Trim();
        string msg;

        if (input.ToLower() == "!time")
        {
            string time = System.DateTime.Now.ToString("HH:mm:ss");
            msg = $"[{name}] 🕒 {time}";
        }
        else
        {
            msg = $"[{name}] {input}";
        }

        photonView.RPC("ReceiveMessage", RpcTarget.All, msg);

        messageInput.text = "";
    }

    [PunRPC]
    void ReceiveMessage(string msg)
    {
        bool isScaryTree = msg.ToLower().Contains("scary tree");

        if (!isScaryTree)
        {
            messages.Add(msg);

            if (messages.Count > maxMessages)
                messages.RemoveAt(0);
        }

        RefreshChat();

        if (isScaryTree)
        {
            StartCoroutine(Scare());
        }
    }

    void RefreshChat()
    {
        if (chatText != null)
            chatText.text = string.Join("\n", messages);
    }

    IEnumerator Scare()
    {
        if (isScaring) yield break;
        isScaring = true;

        if (jumpscareImage != null)
        {
            jumpscareImage.gameObject.SetActive(true);

            Color c = jumpscareImage.color;
            c.a = 1f;
            jumpscareImage.color = c;
        }

        jumpscareSound?.Play();

        yield return new WaitForSeconds(1f);

        if (jumpscareImage != null)
            jumpscareImage.gameObject.SetActive(false);

        isScaring = false;
    }
}