using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnScene : MonoBehaviour
{
    public void SinglePlayerOnClick()
    {
        SceneManager.LoadScene("SinglePlayerScene");
    }

    public void MainMenuOnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ChatRoomOnClick()
    {
        SceneManager.LoadScene("ChatRoom");
    }
}
