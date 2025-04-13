using JetBrains.Annotations;
using UnityEngine;

public class JellyfishBehavior : EnemyBehavior
{
    // animation variables
    public float bobAmplitude;
    public float bobPeriod;

    public LineRenderer[] tentacles;
    public int tentResolution=10;
    public float tentLength=3.5f;
    public float tentAmplitude;
    public float tentPeriod;

    private float pointSep;

    private Vector3 restPosition;

    private float timer=0;

    // mechanic variables
    public float playerDamage = 0.1f;

    public override void EnemyStart()
    {
        restPosition = transform.position;
        pointSep = tentLength / tentResolution;

        base.EnemyStart();
    }

// these functions help update state in fsm
    public override bool canIdle()
    {
        return true;
    }

    public override bool canMove()
    {
        return false;
    }

    public override bool canAttack()
    {
        return false;
    }

// these functions are for collision

    // when hitting the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StatMan.sm.damagePlayer(playerDamage);
    }

// movement functions 

    public override void doIdle()
    {
        timer += Time.deltaTime;

        bob();
        animateTentacles();
    }
    
// helper functions
    
    private void bob()
    {
        float offset = bobAmplitude * Mathf.Sin(timer / bobPeriod);
        rb.MovePosition(restPosition + new Vector3(0, offset));
    }

    private void animateTentacles()
    {
        Vector3[] tentPos = new Vector3[tentResolution];

        for (int i = 0; i < tentResolution; i++)
        {
            float offset = Mathf.Sin((timer + pointSep * i) / tentPeriod) * tentAmplitude;
            tentPos[i] = new Vector3(offset, -pointSep * i, 0);
        }

        for (int i = 0; i < tentacles.Length; i++)
        {
            tentacles[i].SetPositions(tentPos);
        }
    }
}
