using UnityEngine;

public class SardineBehavior : EnemyBehavior
{
    // behavior-related variables
    public float attackRadius;
    public float attackSpeed;

    public float attackTime=1.5f;
    private float attackTimer;

    public float chargeTime = 0.5f;
    private float chargeTimer;

    public float playerDamage = 0.05f;
    public float knockbackForce;

    // animation related variables
    public float flipBuffer = 0.5f;

    [SerializeField]
    private Transform sprite;
    [SerializeField]
    private Animator anim;

    // if the current attack hit the player or not
    private bool hitPlayer;

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if net or if harpoon
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 11) // net || harpoon
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("dying", true);
        } 
    }
    
    // when hitting the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // check if player
        if (collision.gameObject.layer == 6)
        {
            hitPlayer = true;
            StatMan.sm.damagePlayer(playerDamage);
            rb.AddForce(targetvector.normalized * -knockbackForce, ForceMode2D.Impulse);
        }
        // damage player
        // small backwards impulse
        // stop moving forwards at attackspeed
    }


// these functions are for movement and are called by fsm
    public override void doMovement()
    {
        anim.SetBool("moving", true);

        chaseTarget(maxMoveForce, maxMoveSpeed);

        chargeTimer = chargeTime;
    }

    public override void doAttack()
    {
        if (attackStart && chargeTimer > 0)
        {
            anim.SetBool("moving", false);
            anim.SetBool("charging", true);

            chargeTimer -= Time.deltaTime;

            faceTarget();
        } 
        else if (attackStart && chargeTimer <= 0) {
            hitPlayer = false;
            attackStart = false;
            anim.SetBool("charging", false);

            attackTimer = attackTime;
        }
        else
        {
            // update timer
            attackTimer -= Time.deltaTime;
            attackDone = (attackTimer < 0);

            chargeTimer = chargeTime;
            //faceTarget();

            if (!hitPlayer) {
                chaseTarget(maxMoveForce * 5, attackSpeed);
            } else
            {
                chaseTarget(maxMoveForce, maxMoveSpeed);
            }

        }

    }

// these functions are useful and will only be used in this file
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

}
