using UnityEngine;

public class Net : MonoBehaviour
{
    [SerializeField] private float fireSpeed;
    [SerializeField] private float maxRange;

    private bool fired;
    private Vector2 velOffset;
    private float distTraveled;

    private bool fireAnimDone;

    private Animator anim;
    private Collider2D coll;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rb.simulated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (fired && distTraveled < maxRange)
        {
            rb.linearVelocity = transform.up * fireSpeed + new Vector3(velOffset.x, velOffset.y);
            distTraveled += rb.linearVelocity.magnitude * Time.deltaTime;
        } else
        {
            rb.linearVelocity = Vector3.zero;
        }

        if (StatMan.sm.rangeCheck.isOutOfRange(transform.position) || fireAnimDone)
        {
            Destroy(gameObject);
        }
    }

    public void fire()
    {
        // turn on collider
        rb.simulated = true;

        fired = true;
        anim.SetBool("fired", fired);
        velOffset = StatMan.sm.submods.getVelocity();

        // unparent net
        transform.SetParent(null, true);

        velOffset = StatMan.sm.submods.getVelocity();
    }

    public void readyToFire()
    {
        StatMan.sm.submods.netReload();
    }

    public void fireAnimFinished()
    {
        fireAnimDone = true;
    }
}
