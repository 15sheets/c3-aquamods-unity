using System.Threading;
using UnityEngine;
using UnityEngine.U2D;

public class BarracudaBehavior : EnemyBehavior
{
    // behavior related variables
    public Transform attackCastCenter;
    public float attackColliderRadius=0.6f;
    public float attackCooldown;
    
    public float playerDamage;
    public float knockbackForce;

    private Health hp;
    private float attackTimer;

    // animation related variables
    public float flipBuffer = 0.5f;

    [SerializeField] private Transform sprite;
    [SerializeField] private Animator anim;

    // if the current attack hit the player or not
    private bool hitPlayer;

    public override void EnemyStart()
    {
        hp = GetComponent<Health>();
        attackTimer = attackCooldown;
        attackColliderRadius *= transform.localScale.x;

        base.EnemyStart();
    }

    public override void EnemyUpdate()
    {
        attackTimer = (attackTimer > 0) ? attackTimer - Time.deltaTime : attackTimer;
        base.EnemyUpdate();
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
        return (attackTimer < 0);
    }

// these functions are for collision

    // when hitting a net or a harpoon - TODO subtract from health, stun if necessary...
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if net or if harpoon
        if (collision.gameObject.layer == 10) // net
        {
            // damage
            hp.damageamt(StatMan.sm.netDamage);
            // stun -- reset all attack variables to defaults

            //animateStunned();
        }
        else if (collision.gameObject.layer == 11) // harpoon
        {
            // damage
            hp.damageamt(StatMan.sm.harpoonDamage);
            animateDamaged();
        }
    }


// these functions are for movement and are called by fsm
    public override void doMovement()
    {
        chaseTarget(maxMoveForce, maxMoveSpeed);
    }

    public override void doAttack()
    {
        anim.SetBool("attacking", true);

        chaseTarget(maxMoveForce, maxMoveSpeed);

        int playermask = LayerMask.GetMask("Player");
        int shieldmask = LayerMask.GetMask("shield");
        if (!attackStart && !attackDone && !hitPlayer)
        {
            Collider2D attackHitPlayer = Physics2D.OverlapCircle(attackCastCenter.position, attackColliderRadius, playermask);
            Collider2D attackHitShield = Physics2D.OverlapCircle(attackCastCenter.position, attackColliderRadius, shieldmask);

            // if attackStart is off then raycast for attack hit
            if (attackHitPlayer || attackHitShield)
            {
                // if attack hits and it hasn't already hit this time, knockback fish + damage player
                hitPlayer = true;
                rb.AddForce(targetvector.normalized * -knockbackForce, ForceMode2D.Impulse);

                if (!attackHitShield) StatMan.sm.damagePlayer(playerDamage);
            }
        }

        // DONE:
        // turn off attackStart at end of charge via animation event
        // set attackDone via animation event
    }

    public override void doStunned() // TODO: FINISH STUN TMR
    {
        // slow or freeze fish for a little bit
        // some animation indicator of stun
        // count down a timer until stun finished
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

    // this is called by an animation event
    public void attackIsCharged()
    {
        attackStart = false;
    }
    
    // this is called by an animation event
    public void attackIsDone()
    {
        attackDone = true;
        hitPlayer = false;
        attackTimer = attackCooldown;

        anim.SetBool("attacking", false);
    }

    public void animateDamaged()
    {
        anim.SetTrigger("damaged");
    }

    public void animateStunned()
    {

    }

    public void animateDead()
    {
        maxMoveSpeed = 0;
        
        anim.SetBool("stunned", true);
        anim.SetBool("dying", true);
    }
}
