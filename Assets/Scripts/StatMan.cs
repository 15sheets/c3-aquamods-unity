using UnityEngine;

public class StatMan : MonoBehaviour
{
    public static StatMan sm { get; private set; }

    public SubmarineModules submods { get; private set; }
    public Collider2D subcoll { get; private set; }
    public DestroyWhenOOR rangeCheck { get; private set; }
    public Transform subfollow { get; private set; }

    public float netDamage=1; // not sure if i should be putting these in this file... well, whatever.
    public float harpoonDamage=5;

    private void Awake()
    {
        if (sm != null && sm != this)
        {
            Destroy(this);
        } else
        {
            sm = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

// functions

    public void damagePlayer(float dmgpercent)
    {
        submods.playerhp.damage(dmgpercent);
    }

// setting references
    public void setSubmarine(SubmarineModules sub)
    {
        submods = sub;
        subcoll = sub.GetComponent<Collider2D>();
    }

    public void setRangeCheck(DestroyWhenOOR rc)
    {
        rangeCheck = rc;
        subfollow = rc.transform;
    }

    public void setSubFollow(Transform sf)
    {
        subfollow = sf;
    }
}
