using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Enemy_FSM fsm { get; private set; }

    // to set in editor
    public float maxMoveSpeed;
    public float maxMoveForce;
    public float stunTime;

    // for fsm to read
    public bool attackStart;
    public bool attackDone;

    // for fsm transitions
    public bool idleCondition;
    public bool moveCondition;
    public bool attackCondition;
    public bool stunCondition;

    // private variables
    [HideInInspector] public Vector3 targetvector;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public PhysicsHelper ph;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        EnemyStart();
    }

    // Update is called once per frame
    private void Update()
    {
        EnemyUpdate();
    }

    private void FixedUpdate()
    {
        EnemyFixedUpdate();
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // check if net or shield
        stunCondition = true;

        // also, damage enemy here
    }
    */

// these functions are overrideable and are called in start, update, fixedupdate
    public virtual void EnemyStart()
    {
        fsm = new Enemy_FSM(this);
        fsm.Reset(fsm.idleState);

        //target = StatMan.sm.submods.transform;
        rb = GetComponent<Rigidbody2D>();
        ph = GetComponent<PhysicsHelper>();
    }

    public virtual void EnemyUpdate()
    {
        targetvector = StatMan.sm.submods.transform.position - transform.position;

        // idle, move, and attack conditions are calculated
        // stunned is triggered by collisions with shield or net
        idleCondition = canIdle();
        moveCondition = canMove();
        attackCondition = canAttack();

        fsm.Update();

        if (StatMan.sm.rangeCheck.isOutOfRange(transform.position))
        {
            Destroy(gameObject);
        }
    }

    public virtual void EnemyFixedUpdate()
    {
        fsm.FixedUpdate();
    }

 // these functions help update state in fsm
    public virtual bool canIdle()
    {
        return idleCondition;
    }

    public virtual bool canMove()
    {
        return moveCondition;
    }

    public virtual bool canAttack()
    {
        return attackCondition;
    }

 // these functions are for movement and are called by fsm
    public virtual void doMovement()
    {
        faceTarget();

        Vector3 targetvelocity = (targetvector.magnitude < maxMoveSpeed) ? targetvector : targetvector.normalized * maxMoveSpeed;

        Vector2 force = ph.ForceToReachVelocity(targetvelocity, maxMoveForce, true, true);
        rb.AddForce(force);
    }

    public virtual void doAttack()
    {
        attackStart = false;
        attackDone = true;
    }

    public virtual void doIdle()
    {
        rb.linearVelocity = Vector3.zero;
    }

    public virtual void doStunned()
    {
        rb.linearVelocity = Vector3.zero; 
    }

// these functions are useful and will only be used in this file
    private void faceTarget()
    {
        transform.right = targetvector.normalized;
    }

    private void faceTravel()
    {
        //transform.right = rb.linearVelocity.normalized;
    }

}
