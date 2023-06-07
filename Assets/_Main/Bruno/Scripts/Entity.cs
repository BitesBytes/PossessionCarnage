using UnityEngine;

public class Entity : MonoBehaviour
{
    protected enum STATE {PATROL, CHASE} // enum to add the behaiour state, chase STATE will come in the beta rel
    protected enum ATTACKTYPE {SOFT, HEAVY, SPECIAL} // the type of attack depending of the AI-type

    protected DebugPlayer playerDebug = null;
    protected Rigidbody rigidBody;
    protected Vector3 direction; // the movement direction
    protected bool playerSeen;
    protected bool idle;
    protected float damage; // to add in late development
    protected float speed;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected STATE SwitchBehaviour(STATE behaviourState)
    {
        switch(behaviourState)
        {
            case STATE.PATROL:
            Patroling();
            break;
            case STATE.CHASE:
            Chase();
            break;
        }

        return behaviourState;
    }

    protected virtual void Patroling(){}

    protected virtual void Chase(){}

}
