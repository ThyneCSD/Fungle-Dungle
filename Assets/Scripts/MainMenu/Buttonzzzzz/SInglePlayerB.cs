using UnityEngine;
using UnityEngine.SceneManagement;

public class SInglePlayerB : MonoBehaviour
{
    public void SinglePlayerOnClick()
    {
        SceneManager.LoadScene("SinglePlayerScene");
    }

    public void BlackJackOnClick()
    {
        SceneManager.LoadScene("BlackJackScene");
    }

    public void RacinOnClick()
    {
        SceneManager.LoadScene("RacingScene");
    }

    public void SettingsOnClick()
    {
        SceneManager.LoadScene("Settings_Menu");
    }

    public void MenuOnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void MultiplayerOnClick()
    {
        SceneManager.LoadScene("Multiplayer_Menu");
    }

    public void SpaceOnClick()
    {
        SceneManager.LoadScene("SpaceScene");
    }

    public void ExitClick()
    {
        Application.Quit();
    }
}


