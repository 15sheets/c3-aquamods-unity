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
    [SerializeField] private float hForce;
    [SerializeField] private Vector2 vSpeedRange;
    [SerializeField] private float vForce;

    [SerializeField] private float motorSmoothTime;
    private float motorRefVel = 0;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ph = GetComponent<PhysicsHelper>();
        rb = GetComponent<Rigidbody2D>();
        //playerhp = battery.GetComponent<Health>();

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

    }

    private void FixedUpdate()
    {
        // move horizontally
        ph.ApplyForceToReachVelocity(new Vector2(hspeedInput * maxHspeed, 0), force: hForce, moveX: true);

        // move vertically
        float targetVy = -Mathf.Lerp(vSpeedRange[0], vSpeedRange[1], vspeedInput);
        ph.ApplyForceToReachVelocity(new Vector2(0, targetVy), force: vForce, moveY: true);
    }

// private functions
    private void rotateAtSpeed(Transform toRotate, float percent, float maxSpeed, float dt)
    {
        float currRot = toRotate.rotation.eulerAngles.z;
        toRotate.rotation = Quaternion.Euler(0, 0, currRot - maxSpeed * percent * dt);
    }

    private void rotateMotor()
    {
        float targetAngle = Mathf.Atan2(-rb.linearVelocityY, -rb.linearVelocityX) * Mathf.Rad2Deg - 90;
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
        hspeedInput = input;
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
        netAimInput = input;
    }

    public void setHarpoonAim(float input) // input range from [-1, 1]
    {
        harpoonAimInput = input;
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
        shieldInput = input;
    }

    public void setBattery(bool input)
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
        return rb.linearVelocity;
    }
}
