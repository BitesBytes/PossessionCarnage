using UnityEngine;

public abstract class DebugEntity : MonoBehaviour
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
    protected DebugPlayer playerDebug;

    protected State SwitchBehaviour(State behaviourState)
    {
        switch (behaviourState)
        {
            case State.SEARCHING:
                break;
            case State.PATROL:
                Patrolling();
                break;
            case State.CHASE:

                Chase();
                break;
        }

        return behaviourState;
    }

    protected abstract void Patrolling();

    protected abstract void Chase();

}
