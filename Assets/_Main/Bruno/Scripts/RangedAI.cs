using System.Collections.Generic;
using UnityEngine;

public class RangedAI : Entity
{
    [SerializeField] private List<Transform> patrolPoints;

    private int index;
    private float distanceBeetweenPoints;
    private float patrolRange = 1f;
    private float rangeToDriveAway = 8f; // this variable is used to set the max range that AI has before driving away from the player
    private float attackRange = 8.7f;

    private float timer;
    private float maxTimer = 5f;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        damage = 0f;
        speed = 13.4f;
        index = 0;
    }

    private void Update()
    {
        if (!playerSeen)
        {

            timer += Time.deltaTime * 2.0f;

            if (timer >= maxTimer) { idle = !idle; timer = 0; }

            if (!idle)
            {
                SwitchBehaviour(State.PATROL);
            }
        }

        if (playerSeen)
        {
            SwitchBehaviour(State.CHASE);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // il tag in fase definitiva non verrà utilizzato
        {
            playerSeen = true;
        }
    }

    protected override void Patrolling()
    {
        distanceBeetweenPoints = Vector3.Distance(this.transform.position, patrolPoints[index].position);

        if (distanceBeetweenPoints <= patrolRange)
        {
            ChangeIndex();
        }

        direction = (patrolPoints[index].position - this.transform.position).normalized;
        rigidBody.MovePosition(rigidBody.position + direction * speed * Time.deltaTime);
    }

    private void ChangeIndex()
    {
        index++;

        if (index >= patrolPoints.Count)
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
        //float distance = Vector3.Distance(this.transform.position, playerDebug.transform.position);

        //direction = (playerDebug.transform.position - this.transform.position).normalized;
        //direction.y = 0;
        //rigidBody.MovePosition(rigidBody.position + direction * speed * Time.deltaTime);

        ////Debug.Log("chasing player");

        //if (distance <= attackRange)
        //{
        //    DebugAttack();
        //}


        //if (distance <= rangeToDriveAway)
        //{
        //    direction = (this.transform.position - playerDebug.transform.position).normalized;
        //    direction.y = 0;
        //    rigidBody.MovePosition(rigidBody.position + direction * speed * Time.deltaTime);
        //    Debug.Log("drivin away from the player");
        //}
    }

    private void DebugAttack()
    {
        Debug.Log("I'm attacking you with a ranged attack");
    }

}
