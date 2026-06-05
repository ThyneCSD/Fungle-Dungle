using UnityEngine;
using UnityEngine.SceneManagement;
public class SettingvalueSaving : MonoBehaviour
{
    public float volumeValue = 1;
    public float musicSpeedValue = 2;
    public string musicName = "Music1";
    public AudioSource music;


    // Start
    void Start()
    {
        //if (!GameObject.Find("ButtonManager"))
        
        DontDestroyOnLoad(gameObject);
        music = GameObject.Find(musicName).GetComponent<AudioSource>();
        music.Play();
        music.volume = volumeValue;
        music.pitch = musicSpeedValue;
    }
    
    void OnEnable()
    {
    SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
    SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded: " + scene.name);
        //DontDestroyOnLoad(gameObject);
        music = GameObject.Find(musicName).GetComponent<AudioSource>();
        music.Play();
        music.volume = volumeValue;
        music.pitch = musicSpeedValue;
    }


//Methods
public void VolumeChanged(float vol)
    {
        volumeValue = vol;
        music.volume = volumeValue;
    }

    public void SpeedChanged(float spd)
    {
        musicSpeedValue = spd;

        music.pitch = musicSpeedValue;
    }

    public void MusicChanged(string name)
    {
        music.Stop();
        musicName = name;
        music = GameObject.Find(musicName).GetComponent<AudioSource>();
        music.Play();
        music.volume = volumeValue;
        music.pitch = musicSpeedValue;
    }



    // Update
    void Update()
    {
        
    }
}
