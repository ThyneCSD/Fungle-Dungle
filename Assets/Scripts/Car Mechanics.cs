using UnityEngine;

public class CarMechanics : MonoBehaviour
{
    //Customization
    [SerializeField] Rigidbody2D rb;

    [SerializeField] private float topForwardSpeed = 10f;
    [SerializeField] private float topReverseSpeed = -4f;
    //[SerializeField] private float topForwardAcceleration = 1000;
    //[SerializeField] private float topReverseAcceleration = 200;
    [SerializeField] private float accelerationIncrease = 0.09f;
    [SerializeField] private float accelerationDecrease = 0.07f;
    [SerializeField] private float frictionVelocityDecrease = 0.02f;
    [SerializeField] private float baseTurnSpeed = 45f;


    private float rotationalVelocity = 0;
    private float forwardVelocity = 0;
    private float acceleration = 0;
    private float rotationSpeedMultiplier = 1;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(forwardVelocity);
        //Forwardmovement
        if (Input.GetKey(KeyCode.W))
        {
            //forwardVelocity = 5;
            //forwardVelocity += accelerationIncrease;
            forwardVelocity += (topForwardSpeed - forwardVelocity) * accelerationIncrease * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //forwardVelocity = -5;
            //forwardVelocity -= accelerationDecrease;
            //forwardVelocity -= (topReverseSpeed + forwardVelocity) * accelerationIncrease * Time.deltaTime;
            forwardVelocity += (topReverseSpeed - forwardVelocity) * accelerationIncrease * Time.deltaTime;
        }
        else
        {
            // CORRECTIE: Remmen met Time.deltaTime zodat het frame-onafhankelijk is.
            // 0.001f was te zacht, we gebruiken nu een rem-factor (bijv. 3f). 
            // Mathf.MoveTowards zorgt dat hij netjes bij exact 0 stopt en niet doorschiet.
            
            forwardVelocity = Mathf.MoveTowards(forwardVelocity, 0f, frictionVelocityDecrease * Time.deltaTime);
        }
        /*
        else if (forwardVelocity > 0)
        {

            forwardVelocity -= 0.001f;
        }
        else if (forwardVelocity < 0)
        {
            forwardVelocity += 0.001f;
        }
        else { forwardVelocity = 0; } 
        */

        //Rotation
        if (Input.GetKey(KeyCode.A))
        {
            rotationalVelocity = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationalVelocity = -1;
        }
        else
        {
            rotationalVelocity = 0;
        }



        //1 is dan forwardvelocity
        //rb.linearVelocity = new Vector2(0, forwardVelocity);

        if (forwardVelocity > topForwardSpeed)
        {
            forwardVelocity = topForwardSpeed;

        }
        else if (forwardVelocity < topReverseSpeed)
        { 
            forwardVelocity = topReverseSpeed;
        }



        rb.linearVelocity = transform.up * forwardVelocity;


        float speedFactor = Mathf.InverseLerp(0f, 2f, Mathf.Abs(forwardVelocity));

        if (forwardVelocity > 0)
        {
            // Forwards: Normal steering that fades in at low speeds
            rb.angularVelocity = rotationalVelocity * baseTurnSpeed * speedFactor;
        }
        else if (forwardVelocity < 0)
        {
            // Backwards: Inverted steering that also fades in smoothly
            rb.angularVelocity = rotationalVelocity * -baseTurnSpeed * speedFactor;
        }
        else
        {
            // Completely stationary
            rb.angularVelocity = 0f;
        }






        /*
        if (forwardVelocity > 0)
        {
            rotationSpeedMultiplier = 10 - forwardVelocity * 6;
            rb.angularVelocity = rotationalVelocity * 45;
        }
        else if (forwardVelocity < 0)
        {
            rotationSpeedMultiplier = 4 + forwardVelocity * 6;
            rb.angularVelocity = rotationalVelocity * -45;
        }
        else
        {
            rb.angularVelocity = 0; 
        }
        */
    }
}
