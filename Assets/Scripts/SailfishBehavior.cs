using UnityEngine;

public class SailfishBehavior : EnemyBehavior
{
    // behavior-related variables
    public float attackRadius;
    public float attackSpeed;
    public float attackForce;

    public float attackTime = 1.5f;
    private float attackTimer;

    public float chargeTime = 0.5f;
    private float chargeTimer;

    public float playerDamage = 0.05f;
    public float knockbackForce;

    public bool trigHitShield;
    public bool trigHitPlayer;

    private Health hp;

    // animation related variables
    public float flipBuffer = 0.5f;

    [SerializeField] private Transform sprite;
    [SerializeField] private Animator anim;

    [SerializeField] private Collider2D coll;
    [SerializeField] private Collider2D trig;

    // if the current attack hit the player or not
    private bool hitPlayer;

    public override void EnemyStart()
    {
        hp = GetComponent<Health>();

        base.EnemyStart();
    }

// these functions help update state in fsm
    public override bool canIdle()
    {
        return false;
    }

    public override bool canMove()
    {
        return true;
    }

    public override bool canAttack()
    {
        return (targetvector.magnitude < attackRadius);
    }

    // these functions are for collision

    // when hitting a net or a harpoon
    // todo: add shield case...
    // the other collider (trigger) is only on while the sailfish is attacking
    // the collider on this gameobject is only off if the sailfish is attacking and it hasn't hit the shield
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if net or if harpoon
        if (collision.gameObject.layer == 10) // net
        {
            // damage
            hp.damageamt(StatMan.sm.netDamage);
            // stun -- reset all attack variables to defaults
        }
        else if (collision.gameObject.layer == 11) // harpoon
        {
            // damage
            hp.damageamt(StatMan.sm.harpoonDamage);
        }
        /*
        else if (collision.gameObject.layer == 6) // player
        {
            hitPlayer = true;
            StatMan.sm.damagePlayer(playerDamage);
        }
        */
    }

    // when hitting the shield ? add later
    private void OnCollisionEnter2D(Collision2D collision) // todo: change later
    {
        if (collision.gameObject.layer == 12)
        {
            rb.AddForce(targetvector.normalized * -knockbackForce, ForceMode2D.Impulse);
        }
    }

// these functions are for movement and are called by fsm

    public override void doMovement()
    {
        anim.SetBool("moving", true);
        //coll.excludeLayers = 0; // don't exclude any extra layers
        //trig.excludeLayers = -1; // exclude all layers
        
        //coll.enabled = true;
        //trig.enabled = false;

        chaseTarget(maxMoveForce, maxMoveSpeed);

        chargeTimer = chargeTime;
    }

    public override void doAttack()
    {
        // charge attack
        if (attackStart && chargeTimer > 0)
        {
            anim.SetBool("moving", false);
            anim.SetBool("charging", true);

            chargeTimer -= Time.deltaTime;

            chaseTarget(attackForce, 0);
        } // finish charging, start attacking
        else if (attackStart && chargeTimer <= 0)
        {
            hitPlayer = false;
            attackStart = false;
            anim.SetBool("charging", false);

            //coll.enabled = false;
            //trig.enabled = true;
            //trig.excludeLayers = 0;
            Physics2D.IgnoreCollision(coll, StatMan.sm.subcoll, true);

            chargeTimer = chargeTime;
            attackTimer = attackTime;
        }
        else // attack
        {
            // update timer
            attackTimer -= Time.deltaTime;
            attackDone = (attackTimer < 0);

            // check if attack has hit the player
            if (!hitPlayer && trigHitShield)
            {
                Physics2D.IgnoreCollision(coll, StatMan.sm.subcoll, false);
                

                hp.damageamt(StatMan.sm.harpoonDamage);
                hitPlayer = true;

            } else if (!hitPlayer && trigHitPlayer)
            {
                hitPlayer = true;
                StatMan.sm.damagePlayer(playerDamage);
            }

            // movement behaviors
            if (!hitPlayer) // follow player until hit
            {
                chaseTarget(attackForce, attackSpeed);
            }
            else // continue in direction of travel once player hit
            {
                continueTravel(attackForce, rb.linearVelocity);
            }

            // end of attack miscellany
            if (attackDone)
            {
                trigHitPlayer = false;
                trigHitShield = false;
                Physics2D.IgnoreCollision(coll, StatMan.sm.subcoll, false);
            }

        }

    }

// these functions are useful and will only be used in this file
    private void continueTravel(float maxforce, Vector3 currvelocity)
    {
        currvelocity.z = 0;
        //transform.right = currvelocity;

        Vector2 force = ph.ForceToReachVelocity(currvelocity, maxforce, true, true);
        rb.AddForce(force);
    }
    
    private void chaseTarget(float maxforce, float maxvelocity)
    {
        faceTarget();

        //Vector3 targetvelocity = (targetvector.magnitude < maxvelocity) ? targetvector : targetvector.normalized * maxvelocity;
        Vector3 targetvelocity = targetvector.normalized * maxvelocity;

        Vector2 force = ph.ForceToReachVelocity(targetvelocity, maxforce, true, true);
        rb.AddForce(force);
    }

    private void faceTarget()
    {
        // consider changing this to a slerp with a max rotation speed...
        transform.right = targetvector.normalized;
        if ((sprite.localScale.y > 0 && targetvector.x < -flipBuffer) ||
            (sprite.localScale.y < 0 && targetvector.x > flipBuffer))
        {
            sprite.localScale = Vector3.Scale(sprite.localScale, new Vector3(1, -1, 1));
        }
    }

    public void die()
    {
        // add a particle system here later?
        Destroy(gameObject);
    }

    public void animateDeath()
    {
        maxMoveSpeed = 0;

        anim.SetBool("dying", true);
    }
}
