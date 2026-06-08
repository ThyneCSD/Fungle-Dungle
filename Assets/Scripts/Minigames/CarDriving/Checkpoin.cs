using UnityEngine;

public class Checkpoin : MonoBehaviour
{
    [SerializeField] private LapCounter finishLine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            finishLine.CheckpointPassed();
        }
    }

}
