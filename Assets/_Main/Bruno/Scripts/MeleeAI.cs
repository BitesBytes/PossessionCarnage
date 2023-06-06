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
        speed = 10f;
        damage = 0f;
        index = 0;
        rigidBody = GetComponent<Rigidbody>();
        transform.LookAt(PatrolPoints[index].position);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchBehaviour(STATE.PATROL);
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

}
