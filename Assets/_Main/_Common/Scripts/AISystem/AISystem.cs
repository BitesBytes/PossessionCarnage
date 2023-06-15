using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class AISystem : MonoBehaviour
{

    private enum State
    {
        SEARCHING,
        CHASE,
        GO_AWAY
    }

    [SerializeField] private GameObject cube;

    private Character character;
    private NavMeshAgent navMeshAgent;
    private Character actualPlayer;

    private State currentState;

    private bool destinationReached;
    private Vector3 randomPatrolPosition;
    private float navMeshBorderOffset;
    private float searchPlayerRay;
    private float attackRange;
    private float distanceToKeepFromPlayer;
    private MeshCollider meshCollider;

    // stun system
    private Rigidbody rigidBody;
    private float stunTimer; // timer to move again after stun
    private float stunTimerLimit = 1.5f;
    private float impactForce;
    private bool isStunned;

    //Animations System (DEBUG PURPOSES)
    [SerializeField] private Animator animatorController;  // can't put it inside SO because it wants the all prefab in order to see the animator component
    [SerializeField] private GameObject weapon; //test


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        navMeshAgent = GetComponent<NavMeshAgent>();

        SwitchBehaviour(State.SEARCHING);
    }

    private void Start()
    {
        // to enabled
        // EventManager.OnPossessedCharacterChanged += EventManager_OnPossessedCharacterChanged;
    }

    private void EventManager_OnPossessedCharacterChanged(Character character)
    {
        actualPlayer = character;

        SwitchBehaviour(State.SEARCHING);
    }

    private void Update()
    {
        if (cube != null) //to replace in to actual player
        {
            switch (currentState)
            {
                case State.SEARCHING:
                    SearchForPlayer();
                    break;
                case State.CHASE:
                    Chase();
                    break;
                case State.GO_AWAY:
                    GoAway();
                    break;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            character.GetHealthSystem().ChangeHealthAmount(10f);
        }

        if(isStunned)
        {
            stunTimer += Time.deltaTime * 1.0f;

            navMeshAgent.isStopped = true;

            Debug.Log("stun");

            rigidBody.AddForce(transform.forward * impactForce * Time.deltaTime, ForceMode.Force);

            if(stunTimer >= stunTimerLimit)
            {
                isStunned = false;
                stunTimer = 0;
                navMeshAgent.isStopped = false;
            }
        }
    }

    public void Init(Character character)
    {
        this.character = character;

        navMeshBorderOffset = character.GetCharacterType().NavMeshBorderOffset;
        searchPlayerRay = character.GetCharacterType().SearchPlayerRay;
        attackRange = character.GetCharacterType().AttackRange;
        distanceToKeepFromPlayer = character.GetCharacterType().DistanceToKeepFromPlayer;
        meshCollider = character.GetCharacterType().MeshCollider;
        stunTimer = character.GetCharacterType().StunTimer;
        impactForce = character.GetCharacterType().ImpactForce;

        navMeshAgent.speed = character.GetCharacterType().CharacterSpeed;

        //character.GetHealthSystem().OnHealthAmountChanged += HealthSystem_OnHealthAmountChanged;
        HealthSystem.OnHealthAmountChanged += HealthSystem_OnHealthAmountChanged;
    }

    private void HealthSystem_OnHealthAmountChanged(object sender, EventArgs e)
    {
        isStunned = true;
        Debug.Log("stun");
    }


    private void SwitchBehaviour(State behaviourState)
    {
        switch (behaviourState)
        {
            case State.SEARCHING:
                destinationReached = false;
                currentState = State.SEARCHING;
                break;
            case State.CHASE:
                destinationReached = false;
                navMeshAgent.isStopped = false;
                currentState = State.CHASE;
                break;
            case State.GO_AWAY:
                navMeshAgent.isStopped = true;
                currentState = State.GO_AWAY;
                break;
        }
    }

    private void SearchForPlayer()
    {
        destinationReached = navMeshAgent.velocity == Vector3.zero;

        if (destinationReached)
        {
            randomPatrolPosition = new Vector3(UnityEngine.Random.Range(navMeshBorderOffset, meshCollider.bounds.size.x - navMeshBorderOffset), 0f, UnityEngine.Random.Range(navMeshBorderOffset, meshCollider.bounds.size.z - navMeshBorderOffset));
        }

        if (Vector3.Distance(cube.transform.position, transform.position) <= searchPlayerRay) // cube to replace actual player
        {
            character.GetCharacterType().PlayBoolAnimation(animatorController, "SeenPlayer", true);
            SwitchBehaviour(State.CHASE);
        }

        if(navMeshAgent.velocity == Vector3.zero) // go idle (no animations yet TO-DO later) i don't think we need that since it's always in movement
        {
            character.GetCharacterType().PlayBoolAnimation(animatorController, "isRunning", false);
        }

        if(navMeshAgent.velocity != Vector3.zero)
        {
            character.GetCharacterType().PlayBoolAnimation(animatorController, "isRunning", true);
        }

        navMeshAgent.SetDestination(randomPatrolPosition);
    }

    private void Chase()
    {
        float distanceFromPlayer = Vector3.Distance(this.transform.position, cube.transform.position); //cube to replace actualplayer

        destinationReached = distanceFromPlayer <= attackRange;

        if(navMeshAgent.velocity != Vector3.zero)
        {
            character.GetCharacterType().PlayBoolAnimation(animatorController, "SeenPlayer", false);
            character.GetCharacterType().PlayBoolAnimation(animatorController, "isRunning", true);
        }

        if (distanceToKeepFromPlayer != 0f && distanceFromPlayer <= distanceToKeepFromPlayer)
        {
            SwitchBehaviour(State.GO_AWAY);
        }

        if (destinationReached)
        {
            navMeshAgent.isStopped = true;
            character.GetAttackSystem().PerformAttack(AttackType.LIGHT);
        }
        else
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(cube.transform.position); // replace with actual player
        }
    }

    private void GoAway()
    {
        if(navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = false;
        }

        Vector3 driveAway = (this.transform.position - cube.transform.position).normalized;

        character.GetCharacterType().PlayBoolAnimation(animatorController, "isRunningBackward", true);

        navMeshAgent.SetDestination(driveAway);

        float chaseDist = Vector3.Distance(this.transform.position, cube.transform.position); // actual player will replace cube

        if(chaseDist >= distanceToKeepFromPlayer)
        {
            SwitchBehaviour(State.CHASE);
            character.GetCharacterType().PlayBoolAnimation(animatorController, "isRunningBackward", false);
        }
    }

    private void OnDestroy()
    {
        EventManager.OnPossessedCharacterChanged -= EventManager_OnPossessedCharacterChanged;
    }


    //Animation event

    public void ShowWeapon()
    {
        weapon.SetActive(true);
    }

}
