using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ChatJumpscareSystem : MonoBehaviour
{
    [Header("Chat")]
    public TMP_InputField messageInput;
    public TextMeshProUGUI chatText;

    [Header("Jumpscare UI")]
    public RawImage jumpscareImage;
    public AudioSource jumpscareSound;

    private bool isJumpscaring;

    void Start()
    {
        SetAlpha(0f);
        jumpscareImage.gameObject.SetActive(false);
    }

    public void SendMessage()
    {
        string msg = messageInput.text.Trim();

        if (string.IsNullOrEmpty(msg))
            return;

        if (msg.ToLower() == "scary tree")
        {
            StartCoroutine(PlayJumpscare());
            messageInput.text = "";
            return;
        }

        chatText.text += "\n" + msg;
        messageInput.text = "";
    }

    IEnumerator PlayJumpscare()
    {
        if (isJumpscaring) yield break;
        isJumpscaring = true;

        jumpscareImage.gameObject.SetActive(true);
        SetAlpha(1f);

        if (jumpscareSound != null)
            jumpscareSound.Play();

        yield return new WaitForSeconds(0.4f);

        float t = 0f;
        float duration = 3f;

        while (t < duration)
        {
            t += Time.deltaTime;

            float a = Mathf.Lerp(1f, 0f, t / duration);
            SetAlpha(a);

            yield return null;
        }

        SetAlpha(0f);
        jumpscareImage.gameObject.SetActive(false);

        isJumpscaring = false;
    }

    void SetAlpha(float a)
    {
        Color c = jumpscareImage.color;
        c.a = a;
        jumpscareImage.color = c;
    }
}