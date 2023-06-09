using UnityEngine;
using UnityEngine.AI;

public abstract class Entity : MonoBehaviour
{
    protected enum State
    {
        SEARCHING,
        PATROL,
        CHASE
    }

    protected enum AttackType
    {
        SOFT,
        HEAVY,
        SPECIAL
    }

    protected Rigidbody rigidBody;
    protected Vector3 direction;
    protected bool playerSeen;
    protected bool idle;
    protected float damage;
    protected float speed;
    protected State currentState;
    protected DebugMax debugMax;
    protected NavMeshAgent navMeshAgent;

    protected State SwitchBehaviour(State behaviourState)
    {
        switch (behaviourState)
        {
            case State.SEARCHING:
                currentState = State.SEARCHING;
                break;
            case State.PATROL:
                currentState = State.PATROL;
                Patrolling();
                break;
            case State.CHASE:
                currentState = State.CHASE;
                Chase();
                break;
        }

        return behaviourState;
    }

    protected abstract void Patrolling();

    protected abstract void Chase();

}
