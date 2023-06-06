using UnityEngine;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{

    [SerializeField] private List<Transform> patrolingPoints;

    protected enum STATE {PATROL, CHASE} // enum to add the behaiour state, more to add in the late development
    protected enum ATTACKTYPE {MELEE, RANGED} // the type of attack depending of the AI-type

    protected float damage; // to add in late development
    protected Vector3 direction; // the movement direction
    protected STATE behaviourState;
    protected ATTACKTYPE attackType;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected STATE SwitchBehaviour()
    {
        switch(behaviourState)
        {
            case STATE.PATROL:
            break;
            case STATE.CHASE:
            break;
        }

        return behaviourState;
    }


    private void Patroling()
    {

    }
}
