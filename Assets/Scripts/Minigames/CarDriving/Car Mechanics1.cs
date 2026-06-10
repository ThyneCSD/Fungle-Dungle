using TMPro;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class CarMechanics1 : MonoBehaviour
{
    //Customization
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private ParticleSystem smoke1;
    [SerializeField] private ParticleSystem smoke2;
    [SerializeField] private AudioSource brakeSound;
    [SerializeField] private AudioSource AccelerationSound;
    [SerializeField] private AudioSource nitroSound;
    public TextMeshProUGUI speedTracker;

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

    private bool braking = false;
    private bool accelerating = false;
    private bool boosting = false;
    private bool boostgiven = false;

    public float boostMultiplier = 2f;

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        Vector2 normal = collision.contacts[0].normal;

        float headOnFactor = Mathf.Abs(
            Vector2.Dot(transform.up, normal)
        );

        float impactSpeed = collision.relativeVelocity.magnitude;

        float speedLoss = Mathf.Clamp01(
            headOnFactor * impactSpeed / 50f
        );

        forwardVelocity *= (1f - speedLoss);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Boost"))
        {
            boosting = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"AccelerationValue = {AccelerationValue}");
        Debug.Log(forwardVelocity);


        

        //Forwardmovement
        if (boosting == true)
        {
            //
            //BOOST VFX en SFX
            
            
            if (boostgiven == false)
            {
                forwardVelocity = topForwardSpeed * boostMultiplier;
                boostgiven = true;
                nitroSound.Play();
            }



            if (rotationalVelocity > 0)
            {
                forwardVelocity = Mathf.MoveTowards(forwardVelocity, 0f, rotationalVelocity * 15 * frictionVelocityDecrease * Time.deltaTime);
            }
            else if (rotationalVelocity < 0)
            {
                forwardVelocity = Mathf.MoveTowards(forwardVelocity, 0f, -rotationalVelocity * 15 * frictionVelocityDecrease * Time.deltaTime);
            }
            else
            {
                forwardVelocity = Mathf.MoveTowards(forwardVelocity, 0f, frictionVelocityDecrease * 10 * Time.deltaTime);
            }


            if (forwardVelocity <= topForwardSpeed)
            { 
                boosting = false;
                boostgiven = false;
            }
        }
        else
        { 
        if (AccelerationValue > 0)
        {
            //forwardVelocity = 5;
            //forwardVelocity += accelerationIncrease;
            forwardVelocity += (topForwardSpeed - forwardVelocity) * (AccelerationValue * accelerationIncrease) * Time.deltaTime;
            //SFX
            accelerating = true;
        }
        else if (AccelerationValue < 0)
        {
            //forwardVelocity += (topReverseSpeed - forwardVelocity) * (AccelerationValue * accelerationDecrease) * Time.deltaTime;
            if (forwardVelocity > 0)
            {
                // Brake
                forwardVelocity = Mathf.MoveTowards(
                    forwardVelocity,
                    0f,
                    -AccelerationValue * 10f * Time.deltaTime
                );
                braking = true;

            }
            else
            {
                // Reverse
                forwardVelocity += (topReverseSpeed - forwardVelocity)
                                 * (-AccelerationValue)
                                 * accelerationDecrease
                                 * Time.deltaTime;
                accelerating = true;
                //SFX
            }
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
        }






        //effects
        if (braking == true)
        {
            if (forwardVelocity > 0 && AccelerationValue < 0)
            {
                smoke1.Play(); smoke2.Play();
                smoke1.startSpeed = forwardVelocity / 10; smoke2.startSpeed = forwardVelocity / 10;
                if(brakeSound.isPlaying == false)
                { brakeSound.Play(); }
                brakeSound.volume =  -1 * AccelerationValue * forwardVelocity / 10;
                
            }
            else
            {
                smoke1.Stop(); smoke2.Stop();
                brakeSound.Stop();
                braking = false;

            }
        }
        if (accelerating == true)
        {

            if (braking == true || AccelerationValue == 0)
            {
                accelerating = false;
                AccelerationSound.Stop();
            }
            else
            {
                if (AccelerationSound.isPlaying == false)
                { AccelerationSound.Play(); }

                if (AccelerationValue > 0)
                {
                    AccelerationSound.volume = AccelerationValue;
                }
                else
                {
                    AccelerationSound.volume = -1 * AccelerationValue;
                }
            }
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

        /*
        if (forwardVelocity > topForwardSpeed)
        {
            forwardVelocity = topForwardSpeed;

        }
        else if (forwardVelocity < topReverseSpeed)
        {
            forwardVelocity = topReverseSpeed;
        }
        */



        //put acceleration to actual movement

        

        //forwardVelocity = Vector2.Dot(rb.linearVelocity, transform.up);
        rb.linearVelocity = transform.up * forwardVelocity;
        
        speedTracker.text = (forwardVelocity * 5).ToString("F0")+ "km/h";

        //float speedFactor = Mathf.InverseLerp(0f, 2f, Mathf.Abs(forwardVelocity));





        //rotation
        float speedPercent = Mathf.Abs(forwardVelocity) / topForwardSpeed;
        float steeringMultiplier2 = 1f - 0.5f * speedPercent * speedPercent;
        float steeringMultiplier1 = Mathf.Lerp(1f, 0.6f, speedPercent);

        //float steeringMultiplier = (steeringMultiplier2 + steeringMultiplier1) / 2;

        if (boosting == true)
        { 
            steeringMultiplier1 = Mathf.Lerp(1f, 0.9f, speedPercent);
        }

        if (forwardVelocity > 0)
        {
            // Forwards: Normal steering that fades in at low speeds
            rb.angularVelocity = rotationalVelocity * baseTurnSpeed * steeringMultiplier1;
        }
        else if (forwardVelocity < 0)
        {
            // Backwards: Inverted steering that also fades in smoothly
            rb.angularVelocity = rotationalVelocity * -baseTurnSpeed * steeringMultiplier1;
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

