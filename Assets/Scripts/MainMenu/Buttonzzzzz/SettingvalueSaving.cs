using UnityEngine;

public class SettingvalueSaving : MonoBehaviour
{
    public float volumeValue = 1;
    public float musicSpeedValue = 2;
    public string musicName = "Music1";
    public AudioSource music;


    // Start
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        music = GameObject.Find(musicName).GetComponent<AudioSource>();
        music.Play();
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
    }



    // Update
    void Update()
    {
        
    }
}
