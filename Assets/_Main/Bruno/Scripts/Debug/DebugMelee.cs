using System.Collections.Generic;
using UnityEngine;

public class DebugMelee : DebugEntity
{
    [SerializeField] private float searchPlayerRay = 10f;
    [SerializeField] private float attackRange = 1.5f;

    private int index;
    private float distanceBeetweenPoints;
    private float patrolRange = 1f;
    private float timer;
    private float maxTimer = 3f;
    private float randomPointMaxDistance;
    private Vector3 randomPatrolPosition;
    private bool destinationReached;
    private float navMeshBorderOffset;

    public List<Transform> PatrolPoints;

    private void Start()
    {
        speed = 10f;
        damage = 0f;
        index = 0;
        rigidBody = GetComponent<Rigidbody>();
        playerDebug = GameObject.FindWithTag("Player").GetComponent<DebugPlayer>();
    }

    private void Update()
    {
        if (!playerSeen)
        {
            timer += Time.deltaTime * 1.0f;

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

    protected override void Patrolling()
    {
        distanceBeetweenPoints = Vector3.Distance(this.transform.position, PatrolPoints[index].position);

        if (distanceBeetweenPoints <= patrolRange)
        {
            ChangeIndex();
        }

        direction = (PatrolPoints[index].position - this.transform.position).normalized;
        rigidBody.MovePosition(rigidBody.position + direction * speed * Time.deltaTime);
    }

    private void ChangeIndex()
    {
        index++;

        if (index >= PatrolPoints.Count)
        {
            index = 0;
        }

        transform.LookAt(PatrolPoints[index].position);
    }

    private void Move()
    {
        direction = (PatrolPoints[index].position - this.transform.position).normalized;
        rigidBody.MovePosition(rigidBody.position + direction * speed * Time.deltaTime);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // il tag in fase definitiva non verrà utilizzato
        {
            playerSeen = true;
        }
    }

    protected override void Chase()
    {
        direction = (playerDebug.transform.position - this.transform.position).normalized;
        rigidBody.MovePosition(rigidBody.position + direction * speed * Time.deltaTime);
        transform.LookAt(playerDebug.transform.position);

        ////calculate the distance beetween AI and player and if is less/equal to 1.5f which is the minimum distance display the debug.log

        float dist = Vector3.Distance(this.transform.position, playerDebug.transform.position);

        if (dist <= attackRange)
        {
            FistsAttack();
        }
    }

    private void FistsAttack()
    {
        Debug.Log("fists attack");
    }
}
