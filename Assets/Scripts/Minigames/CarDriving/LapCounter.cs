using UnityEngine;

public class LapCounter : MonoBehaviour
{
    public int laps = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            laps++;
        }
        
    }
    
}
