using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine;

public class SubmarineModules : MonoBehaviour
{
    // transforms of other parts of submarine
    public Transform netgun;
    
    public Transform harpoongun;
    public Transform harpoonSpawnPoint;
    public GameObject harpoonPrefab;

    public Transform motor;
    public Transform shield;

    [SerializeField] private float maxRotationSpeed; // guns, shield share a rotation speed
    [SerializeField] private float maxHspeed; // max horizontal travel speed
    [SerializeField] private float hForce;
    [SerializeField] private Vector2 vSpeedRange;
    [SerializeField] private float vForce;

    [SerializeField] private float motorSmoothTime;
    private float motorRefVel = 0;

    // variables related to gun cooldowns
    //public bool netReady;
    public bool harpoonReady;

    // vars storing inputs from controller
    private bool netFireInput;
    public bool harpoonFireInput;
    public float netAimInput;
    public float harpoonAimInput;
    public float shieldInput;
    public float vspeedInput; // this is tracking slider position, not rotation speed, so... special case...?
    public float hspeedInput;

    private PhysicsHelper ph;
    private Rigidbody2D rb;

    private Harpoon harpoon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ph = GetComponent<PhysicsHelper>();
        rb = GetComponent<Rigidbody2D>();

        StatMan.sm.setSubmarine(this);
        newHarpoon();

        //netReady = true;
        //harpoonReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        // rotate things here
        rotateAtSpeed(netgun, netAimInput, maxRotationSpeed, Time.deltaTime);
        rotateAtSpeed(harpoongun, harpoonAimInput, maxRotationSpeed, Time.deltaTime);
        rotateAtSpeed(shield, shieldInput, maxRotationSpeed, Time.deltaTime);

        // rotate motor to face direction (do we want to have a max motor rotation speed? or nah)
        rotateMotor();

        // deal with guns firing
        if (harpoonFireInput && harpoonReady)
        {
            harpoonReady = false;
            harpoon.fire();
            newHarpoon();
        }

    }

    private void FixedUpdate()
    {
        // move submarine body here (using physics?)

        // move horizontally
        ph.ApplyForceToReachVelocity(new Vector2(hspeedInput * maxHspeed, 0), force: hForce, moveX: true);

        // move vertically
        float targetVy = -Mathf.Lerp(vSpeedRange[0], vSpeedRange[1], vspeedInput);
        ph.ApplyForceToReachVelocity(new Vector2(0, targetVy), force: vForce, moveY: true);
    }

    public float getYVelocity()
    {
        return rb.linearVelocityY;
    }

    private void rotateAtSpeed(Transform toRotate, float percent, float maxSpeed, float dt)
    {
        float currRot = toRotate.rotation.eulerAngles.z;
        toRotate.rotation = Quaternion.Euler(0, 0, currRot + maxSpeed * percent * dt);
    }

    public void rotateMotor()
    {
        float targetAngle = Mathf.Atan2(-rb.linearVelocityY, -rb.linearVelocityX) * Mathf.Rad2Deg - 90;
        float angle = Mathf.SmoothDampAngle(motor.eulerAngles.z, targetAngle, ref motorRefVel, motorSmoothTime);
        motor.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void newHarpoon()
    {
        harpoon = Instantiate(harpoonPrefab, harpoonSpawnPoint).GetComponent<Harpoon>();
    }

    public void harpoonReload()
    {
        harpoonReady = true;
    }

}
