using UnityEngine;

public class CarMechanics : MonoBehaviour
{
    //Customization
    [SerializeField] Rigidbody2D rb;

    [SerializeField] private float topForwardSpeed = 10;
    [SerializeField] private float topReverseSpeed = 4;
    [SerializeField] private float topForwardAcceleration = 1000;
    [SerializeField] private float topReverseAcceleration = 200;
    [SerializeField] private float accelerationIncrease = 3;
    [SerializeField] private float accelerationDecrease = 2;




    private float rotationalVelocity = 0;
    private float forwardVelocity = 0;
    private float acceleration = 0;
    
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //1 is dan forwardvelocity
        rb.linearVelocity = new Vector2(0, 1);
    }
}
