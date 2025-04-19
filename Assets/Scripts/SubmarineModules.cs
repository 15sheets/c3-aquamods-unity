using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine;

public class SubmarineModules : MonoBehaviour
{
    // transforms of other parts of submarine
    public Transform netgun;
    public Transform netSpawnPoint;
    public GameObject netPrefab;
    
    public Transform harpoongun;
    public Transform harpoonSpawnPoint;
    public GameObject harpoonPrefab;

    public Transform motor;
    public Transform shield;

    public Transform battery;
    public Health playerhp;

    [SerializeField] private float maxRotationSpeed; // guns, shield share a rotation speed
    [SerializeField] private float maxHspeed; // max horizontal travel speed
    [SerializeField] private Vector2 vSpeedRange;

    [SerializeField] private float motorSmoothTime;
    private float motorRefVel = 0;

    [SerializeField] private ParticleSystem motorParticles;
    private ParticleSystem.MainModule mp_main;
    private ParticleSystem.EmissionModule mp_emission;

    [SerializeField] private Vector2 mp_speed;
    [SerializeField] private Vector2 mp_rate;
    [SerializeField] private Vector2 mp_minsizes;
    [SerializeField] private Vector2 mp_maxsizes;

    // variables related to gun cooldowns
    public bool netReady;
    public bool harpoonReady;

    // vars storing inputs from controller
    // if needed, make these public to debug
    private bool netFireInput;
    private bool harpoonFireInput;
    private bool batteryInput;
    private float netAimInput;
    private float harpoonAimInput;
    private float shieldInput;
    private float vspeedInput; // this is tracking slider position, not rotation speed, so... special case...?
    private float hspeedInput;

    private PhysicsHelper ph;
    private Rigidbody2D rb;

    private Harpoon harpoon;
    private Net net;

    // rotation values of battery that correspond to max and min hp
    private static float maxhpRotation = 0;
    private static float minhpRotation = -120;

    // current x,y velocities, for smoothdamp purposes
    private float smoothTime = 0.1f;
    private Vector3 currVel = Vector3.zero;
    private float refvelx;
    private float refvely;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ph = GetComponent<PhysicsHelper>();
        rb = GetComponent<Rigidbody2D>();

        mp_main = motorParticles.main;
        mp_emission = motorParticles.emission;

        StatMan.sm.setSubmarine(this);
        newHarpoon();
        newNet();
    }

    // Update is called once per frame
    void Update()
    {
        // rotate things here
        rotateAtSpeed(netgun, netAimInput, maxRotationSpeed, Time.deltaTime);
        rotateAtSpeed(harpoongun, harpoonAimInput, maxRotationSpeed, Time.deltaTime);
        rotateAtSpeed(shield, shieldInput, maxRotationSpeed, Time.deltaTime);

        // rotate motor to face direction (do we want to have a separate max motor rotation speed? or nah)
        rotateMotor();

        // rotate battery to match hp and animate as necessary
        rotateBattery();
        playerhp.animateHealing(batteryInput);

        // deal with guns firing
        // btw if you're looking through the scripts in search of the rest of the gun logic, it's in the animator
        // reload time and net scaling is done through animation and animation events
        // i'm not explaining that go google it
        if (harpoonFireInput && harpoonReady)
        {
            harpoonReady = false;
            harpoon.fire();
            newHarpoon();
        }
        if (netFireInput && netReady)
        {
            netReady = false;
            net.fire();
            newNet();
        }

        // update motor particle system stats
        float vspeedpercent = getVSpeedPercent();

        mp_main.startSpeed = Mathf.Lerp(mp_speed[0], mp_speed[1], vspeedpercent);
        mp_emission.rateOverTime = Mathf.Lerp(mp_rate[0], mp_rate[1], vspeedpercent);
        mp_main.startSize = new ParticleSystem.MinMaxCurve(Mathf.Lerp(mp_minsizes[0], mp_minsizes[1], vspeedpercent), 
                                                           Mathf.Lerp(mp_maxsizes[0], mp_maxsizes[1], vspeedpercent));
    }

    private void FixedUpdate()
    {
        // move horizontally
        currVel.x = Mathf.SmoothDamp(currVel.x, hspeedInput * maxHspeed, ref refvelx, smoothTime);

        // move vertically
        float targetVy = -Mathf.Lerp(vSpeedRange[0], vSpeedRange[1], vspeedInput);
        currVel.y = Mathf.SmoothDamp(currVel.y, targetVy, ref refvely, smoothTime);

        rb.MovePosition(transform.position + currVel * Time.deltaTime);
    }

// private functions
    private void rotateAtSpeed(Transform toRotate, float percent, float maxSpeed, float dt)
    {
        float currRot = toRotate.rotation.eulerAngles.z;
        toRotate.rotation = Quaternion.Euler(0, 0, currRot - maxSpeed * percent * dt);
    }

    private void rotateMotor()
    {
        float targetAngle = Mathf.Atan2(-currVel.y, -currVel.x) * Mathf.Rad2Deg - 90;
        float angle = Mathf.SmoothDampAngle(motor.eulerAngles.z, targetAngle, ref motorRefVel, motorSmoothTime);
        motor.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void rotateBattery()
    {
        if (batteryInput)
        {
            playerhp.heal(Time.deltaTime);
        }

        float targetAngle = Mathf.LerpAngle(minhpRotation, maxhpRotation, playerhp.getHealthPercent());
        battery.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    private void newHarpoon()
    {
        harpoon = Instantiate(harpoonPrefab, harpoonSpawnPoint).GetComponent<Harpoon>();
    }

    private void newNet() 
    {
        net = Instantiate(netPrefab, netSpawnPoint).GetComponent<Net>();
    }

// public functions for controller interface
    public void setSteer(float input) // input range from [-1, 1]
    {
        hspeedInput = 3 * input;
    }

    public void setSpeed(float input, bool keycon) // speed setting; keycon is true if not using custom controllers
    {
        if (keycon) // increase/decrease speed by amount "input" for keyboard or standard controller
        {
            vspeedInput = Mathf.Clamp(vspeedInput + input, 0.0f, 1.0f);
        }
        else { // only set speed if input > 0 (< 0 signifies no input)
            vspeedInput = (input >= 0) ? input : vspeedInput;
        }
    }

    public void setNetAim(float input) // input range from [-1, 1]
    {
        netAimInput = 5 * input;
    }

    public void setHarpoonAim(float input) // input range from [-1, 1]
    {
        harpoonAimInput = 5 * input;
    }

    public void setNetFire(bool input) // button
    {
        netFireInput = input;
    }

    public void setHarpoonFire(bool input) // button
    {
        harpoonFireInput = input;
    }

    public void setShield(float input) // input range from [-1, 1]
    {
        shieldInput = 5 * input;
    }

    public void setBattery(bool input) // button
    {
        batteryInput = input;
    }

// public functions that aren't for controller interface
    public void harpoonReload()
    {
        harpoonReady = true;
    }

    public void netReload()
    {
        netReady = true;
    }

    public Vector2 getVelocity()
    {
        return currVel;
    }

    public float getVSpeedPercent()
    {
        return (-currVel.y - vSpeedRange[0]) / (vSpeedRange[1] - vSpeedRange[0]);  
    }
}
