using NUnit.Framework.Constraints;
using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

// FSM for player movement
[Serializable]
public class Enemy_FSM
{
    public IState currState { get; private set; }

    // references to state objects
    // idle, move, attack, stunned (?) states
    public IdleState idleState;
    public MoveState moveState;
    public AttackState attackState;
    public StunnedState stunnedState;

    // constructor (pass in necessary params)
    public Enemy_FSM(EnemyBehavior enemy)
    {
        idleState = new IdleState(enemy);
        moveState = new MoveState(enemy);
        attackState = new AttackState(enemy);
        stunnedState = new StunnedState(enemy);
    }

    // set starting state
    public void Reset(IState state)
    {
        currState = state;
        state.Enter();
    }

    // take state transition
    public void TransitionTo(IState nextState)
    {
        currState.Exit();
        currState = nextState;
        nextState.Enter();
    }

    // code from the current state which should run in update
    public void Update()
    {
        if (currState != null)
        {
            currState.Update();
        }
    }

    // code from the current state which should run in fixedupdate
    public void FixedUpdate()
    {
        if (currState != null)
        {
            currState.FixedUpdate();
        }
    }
}

//
// The following are various states for enemy behavior
//

public class IdleState : IState
{
    private EnemyBehavior enemy;

    // constructor
    public IdleState(EnemyBehavior enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {

    }

    public void Update()
    {
        // transitions and input checking go here
        if (enemy.stunCondition)
        {
            enemy.fsm.TransitionTo(enemy.fsm.stunnedState);
        }
        else if (enemy.attackCondition)
        {
            enemy.fsm.TransitionTo(enemy.fsm.attackState);
        }
        else if (enemy.moveCondition)
        {
            enemy.fsm.TransitionTo(enemy.fsm.moveState);
        }
    }

    public void FixedUpdate()
    {
        // movement goes here

        // don't move when idle
        enemy.doIdle();
    }

    public void Exit()
    {

    }
}

public class MoveState : IState
{
    private EnemyBehavior enemy;

    // constructor
    public MoveState(EnemyBehavior enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {

    }

    public void Update()
    {
        // transitions and input checking go here
        // transitions and input checking go here
        if (enemy.stunCondition)
        {
            enemy.fsm.TransitionTo(enemy.fsm.stunnedState);
        }
        else if (enemy.attackCondition)
        {
            enemy.fsm.TransitionTo(enemy.fsm.attackState);
        }
        else if (!enemy.moveCondition && enemy.idleCondition)
        {
            enemy.fsm.TransitionTo(enemy.fsm.idleState);
        }
    }

    public void FixedUpdate()
    {
        // movement goes here -- boids stuff...?
        enemy.doMovement();
    }

    public void Exit()
    {

    }
}

public class AttackState : IState
{
    private EnemyBehavior enemy;

    // constructor
    public AttackState(EnemyBehavior enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.attackStart = true;
    }

    public void Update()
    {
        // transitions and input checking go here
        if (enemy.stunCondition)
        {
            enemy.fsm.TransitionTo(enemy.fsm.stunnedState);
        }
        else if (enemy.attackDone && enemy.attackCondition)
        {
            enemy.fsm.TransitionTo(enemy.fsm.attackState);
        }
        else if (enemy.attackDone && enemy.moveCondition)
        {
            enemy.fsm.TransitionTo(enemy.fsm.moveState);
        }
        else if (enemy.idleCondition)
        {
            enemy.fsm.TransitionTo(enemy.fsm.idleState);
        }
    }

    public void FixedUpdate()
    {
        // movement goes here
        // but probably another file will be managing the attack movement ?
        enemy.doAttack();
    }

    public void Exit()
    {
        enemy.attackDone = false;
    }
}

public class StunnedState : IState
{
    private EnemyBehavior enemy;

    private float timer;

    // constructor
    public StunnedState(EnemyBehavior enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.stunCondition = false;
        timer = enemy.stunTime;
    }

    public void Update()
    {
        // transitions and input checking go here
        if (enemy.stunCondition)
        {
            enemy.fsm.TransitionTo(enemy.fsm.stunnedState);
        }
        else if (timer <= 0 && enemy.moveCondition)
        {
            enemy.fsm.TransitionTo(enemy.fsm.moveState);
        }
        else if (timer <= 0)
        {
            enemy.fsm.TransitionTo(enemy.fsm.idleState);
        }

        timer -= Time.deltaTime;
    }

    public void FixedUpdate()
    {
        // movement goes here

        // don't move while stunned
        enemy.doStunned();
    }

    public void Exit()
    {

    }
}