using UnityEngine;
using System.Collections.Generic;

public class RangedAI : Entity
{
    [SerializeField] private List<Transform> patrolPoints;
    private int index;
    private float distanceBeetweenPoints;
    private float patrolRange = 1f;
    private float rangeToDriveAway = 8f; // this variable is used to set the max range that AI has before driving away from the player

    void Start()
    {
        playerDebug = GameObject.FindWithTag("Player").GetComponent<DebugPlayer>();
        rigidBody = GetComponent<Rigidbody>();
        damage = 0f;
        speed = 13.4f;
        index = 0;
    }


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


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerSeen = true;
        }
    }

    protected override void Patroling()
    {
        distanceBeetweenPoints = Vector3.Distance(this.transform.position, patrolPoints[index].position);

        if(distanceBeetweenPoints <= patrolRange)
        {
            ChangeIndex();
        }

        direction = (patrolPoints[index].position - this.transform.position).normalized;
        rigidBody.MovePosition(rigidBody.position + direction * speed * Time.deltaTime);
    }


    private void ChangeIndex()
    {
        index++;

        if(index >= patrolPoints.Count)
        {
            index = 0;
        }

        transform.LookAt(patrolPoints[index].position);
    }


    protected override void Chase()
    {
        DriveAway();
    }

    private void DriveAway()
    {
        float distance = Vector3.Distance(this.transform.position, playerDebug.transform.position);

        direction = (playerDebug.transform.position - this.transform.position).normalized;
        direction.y = 0;
        rigidBody.MovePosition(rigidBody.position + direction * speed * Time.deltaTime);

        if(distance <= rangeToDriveAway)
        {
            direction = (this.transform.position - playerDebug.transform.position).normalized;
            direction.y = 0;
            rigidBody.MovePosition(rigidBody.position + direction * speed * Time.deltaTime);
        }
    }
}
