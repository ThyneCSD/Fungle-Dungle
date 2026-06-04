using UnityEngine;

public class CarMechanics1 : MonoBehaviour
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

    public float AccelerationValue = 0;
    
    private float rotationalVelocity = 0;
    private float forwardVelocity = 0;
    private float acceleration = 0;
    private float rotationSpeedMultiplier = 1;


    void Start()
    {

    }
    public void AccelerationChanged(float acc)
    {
        AccelerationValue = acc;

    }

    public void SteeringChanged(float str)
    {
        rotationalVelocity = str;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"AccelerationValue = {AccelerationValue}");
        Debug.Log(forwardVelocity);
        //Forwardmovement
        if (AccelerationValue > 0)
        {
            //forwardVelocity = 5;
            //forwardVelocity += accelerationIncrease;
            forwardVelocity += (topForwardSpeed - forwardVelocity) * (AccelerationValue * accelerationIncrease) * Time.deltaTime;


        }
        else if (AccelerationValue < -1)
        {
            forwardVelocity += (topReverseSpeed - forwardVelocity) * (AccelerationValue * accelerationDecrease) * Time.deltaTime;
        }
        else
        {
            if (rotationalVelocity > 0)
            {
                forwardVelocity = Mathf.MoveTowards(forwardVelocity, 0f, rotationalVelocity * 3 * frictionVelocityDecrease * Time.deltaTime);
            }
            else if (rotationalVelocity < 0)
            {
                forwardVelocity = Mathf.MoveTowards(forwardVelocity, 0f, -rotationalVelocity * 3 * frictionVelocityDecrease * Time.deltaTime);
            }
            else
            {
                forwardVelocity = Mathf.MoveTowards(forwardVelocity, 0f, frictionVelocityDecrease * Time.deltaTime);
            
            }
            /*
             *
             *if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                forwardVelocity = Mathf.MoveTowards(forwardVelocity, 0f, 3 * frictionVelocityDecrease * Time.deltaTime);
            }
             */

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

        /*
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
        */


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

