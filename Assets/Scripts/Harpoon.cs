using UnityEngine;

public class Harpoon : MonoBehaviour
{
    [SerializeField] private float fireSpeed;

    private bool fired;
    private Vector2 velOffset;

    private Collider2D coll;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        rb.simulated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // don't forget to deal with collisions...
    // move harpoon in dxn if fired
    private void FixedUpdate()
    {
        if (fired)
        {
            // may need to adjust position when sprites are switched
            rb.linearVelocity = - transform.right * fireSpeed + new Vector3(velOffset.x, velOffset.y);
            //Vector2 nextpos = transform.position - transform.right * fireSpeed * Time.fixedDeltaTime;
            //rb.MovePosition(nextpos);

        }
        
        if (StatMan.sm.rangeCheck.isOutOfRange(transform.position))
        {
            Destroy(gameObject);
        }
        
    }

    public void fire()
    {
        // turn on collider
        rb.simulated = true;

        // unparent harpoon
        transform.SetParent(null, true);

        velOffset = StatMan.sm.submods.getVelocity();
        fired = true;
    }

    public void readyToFire()
    {
        StatMan.sm.submods.harpoonReload();
        
        
    }
}
