using UnityEngine;
using System.Collections.Generic;

public class RangedAI : Entity
{
    [SerializeField] private List<Transform> patrolPoints;
    private int index;
    private float distanceBeetweenPoints;
    private float patrolRange = 1f;

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
        SwitchBehaviour(STATE.PATROL);
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

    }
}
