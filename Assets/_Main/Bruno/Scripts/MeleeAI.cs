using System.Collections.Generic;
using UnityEngine;

public class MeleeAI : Entity
{

    public List<Transform> PatrolPoints;
    private int index;
    private float distanceBeetweenPoints;
    private float patrolRange = 1f;


    // Start is called before the first frame update
    void Start()
    {
        playerDebug = GameObject.FindWithTag("Player").GetComponent<DebugPlayer>();
        speed = 10f;
        damage = 0f;
        index = 0;
        rigidBody = GetComponent<Rigidbody>();
        transform.LookAt(PatrolPoints[index].position);
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerSeen)
        {
            SwitchBehaviour(STATE.PATROL);
        }

        if(playerSeen)
        {
            SwitchBehaviour(STATE.CHASE);
        }
    }

    protected override void Patroling()
    {
        distanceBeetweenPoints = Vector3.Distance(this.transform.position, PatrolPoints[index].position);

        if(distanceBeetweenPoints <= patrolRange)
        {
            SwitchPoints();
        }

        Move();
    }

    private void Move()
    {
        direction = (PatrolPoints[index].position - this.transform.position).normalized;
        rigidBody.MovePosition(rigidBody.position + direction * speed * Time.deltaTime);
    }

    private void SwitchPoints()
    {
        index++;

        if(index >= PatrolPoints.Count)
        {
            index = 0;
        }

        transform.LookAt(PatrolPoints[index].position);
    }


    protected override void Chase()
    {
        direction = (playerDebug.transform.position - this.transform.position).normalized;
        rigidBody.MovePosition(rigidBody.position + direction * speed * Time.deltaTime);
        transform.LookAt(playerDebug.transform.position);

        //calculate the distance beetween AI and player and if is less/equal to 1.5f which is the minimum distance display the debug.log

        float dist = Vector3.Distance(this.transform.position, playerDebug.transform.position);

        if(dist <= 1.5f) // I just set it for test purpose
        {
            FistsAttack();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Attack Mode on");
            playerSeen = true;
        }
    }

    private void FistsAttack()
    {
        Debug.Log("fists attack");
    }

}
