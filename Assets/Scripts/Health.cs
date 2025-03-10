using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    //public bool isPlayer;

    public Animator anim;

    public float maxhp;
    public float hp;

    public float healingPerSecond = .05f;

    public UnityEvent death;
    public UnityEvent damaged;
    //public UnityEvent healed;

    //public bool healing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hp = maxhp;

        if (death == null)
        {
            death = new UnityEvent();
        }
        if (damaged == null)
        {
            damaged = new UnityEvent();
        }
        /*
        if (healed == null)
        {
            healed = new UnityEvent();
        }
        */
    }

    /// <summary>
    /// returns normalized value of hp percentage
    /// </summary>
    /// <returns></returns>
    public float getHealthPercent()
    {
        return hp / maxhp;
    }

    /// <summary>
    /// do dmgpct of damage to object
    /// </summary>
    /// <param name="dmgamt"></param>
    public void damage(float dmgamt)
    {
        hp -= dmgamt * maxhp;
        damaged.Invoke();
        // todo: play an animation (flash main player body? or just health? play a sound effect?)
        // rotate hp shape..

        if (hp <= 0f)
        {
            death.Invoke();
        }
    }

    /// <summary>
    /// pass in deltaTime
    /// </summary>
    /// <param name="healamt"></param>
    public void heal(float healamt)
    {
        if (hp < maxhp)
        {
            //healed.Invoke();
            //healing = true;
            hp = Mathf.Min(hp + maxhp * healamt * healingPerSecond, maxhp);
        }

        // todo: play an animation (pulse while healing)
        // rotate hp shape
    }

// animator functions
    public void animateDamaged()
    {
        anim.SetTrigger("damaged");
    }

    public void animateHealing(bool healing)
    {
        anim.SetBool("healing", healing);
    }

}
