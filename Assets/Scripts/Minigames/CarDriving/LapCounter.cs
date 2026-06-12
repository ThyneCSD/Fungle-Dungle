using UnityEngine;
using TMPro;

public class LapCounter : MonoBehaviour
{
    private bool checkpointPassed = false;
    private bool raceStarted = false;

    private float lapTime = 0f;
    private float bestLapTime;

    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI bestTimeText;

    void Start()
    {
        bestLapTime = GameStateR.LapTime;
    }

    void Update()
    {
        if (raceStarted)
        {
            lapTime += Time.deltaTime;
        }

        currentTimeText.text = lapTime.ToString("F2");

        if (bestLapTime == Mathf.Infinity)
        {
            bestTimeText.text = "--.--";
        }
        else
        {
            bestTimeText.text = bestLapTime.ToString("F2");
        }
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
                GameStateR.LapTime = bestLapTime;

                Debug.Log($"New Best Lap: {bestLapTime:F2}");
            }

            lapTime = 0f;
            checkpointPassed = false;
        }
    }
}