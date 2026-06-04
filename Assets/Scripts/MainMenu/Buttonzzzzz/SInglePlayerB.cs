using UnityEngine;
using UnityEngine.SceneManagement;

public class SInglePlayerB : MonoBehaviour
{
    public void SinglePlayerOnClick()
    {
        SceneManager.LoadScene("SinglePlayerScene");
    }

    public void BlackJackOnCLick()
    {
        SceneManager.LoadScene("BlackJackScene");
    }

    public void RacinOnCLick()
    {
        SceneManager.LoadScene("RacingScene");
    }
}


