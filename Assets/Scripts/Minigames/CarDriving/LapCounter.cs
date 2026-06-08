using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LapCounter : MonoBehaviour
{
    /*
    public int laps = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            laps++;
        }
        
    }
    */
    private bool checkpointPassed = false;
    private float lapTime = 0f;
    private float bestLapTime = Mathf.Infinity;
    private bool raceStarted = false;
    public TextMeshProUGUI timeTracker;
    

    void Update()
    {
        if (raceStarted)
        {
            lapTime += Time.deltaTime;
        }
        timeTracker.text = lapTime.ToString("F2");

    }

    public void CheckpointPassed()
    {
        checkpointPassed = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        
        if (!raceStarted)
        {
            raceStarted = true;
            lapTime = 0f;
            checkpointPassed = false;
            return;
        }

        
        if (checkpointPassed)
        {
            Debug.Log($"Lap Time: {lapTime:F2}");

            if (lapTime < bestLapTime)
            {
                bestLapTime = lapTime;
                Debug.Log($"Best Lap: {bestLapTime:F2}");
            }

            lapTime = 0f;
            checkpointPassed = false;
        }
    }


}
