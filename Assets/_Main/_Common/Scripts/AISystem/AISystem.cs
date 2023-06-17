using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(SphereCollider), typeof(Animator))]
public class AISystem : MonoBehaviour
{
    private enum State
    {
        SEARCHING,
        CHASE,
        GO_AWAY,
        STUNNED,
        IDLE,
        ATTACK
    }

    private Character character;
    private NavMeshAgent navMeshAgent;
    private Player player;
    private Rigidbody rigidBody;
    private MeshCollider meshCollider;
    private SphereCollider sphereCollider;

    private State currentState;
    private State previousState;

    private bool destinationReached;
    private Vector3 randomPatrolPosition;
    private float navMeshBorderOffset;
    private float searchPlayerRay;
    private float attackRange;
    private float distanceToKeepFromPlayer;

    private float stunTimer;
    private float impactForce;

    private float idleTimer;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        sphereCollider = GetComponent<SphereCollider>();

        sphereCollider.isTrigger = true;
    }

    private void Start()
    {
        EventManager.OnPossessedCharacterChanged += EventManager_OnPossessedCharacterChanged;
    }

    private void EventManager_OnPossessedCharacterChanged(Character character)
    {
        SwitchBehaviour(State.SEARCHING);
    }

    private void Update()
    {
        if (player == null)
        {
            AnimatorUtils.PlayBoolAnimation(character.GetAnimator(), "SeenPlayer", false);
            SwitchBehaviour(State.SEARCHING);
        }
        else
        {
            AnimatorUtils.PlayBoolAnimation(character.GetAnimator(), "SeenPlayer", true);
        }

        AnimatorUtils.PlayBoolAnimation(character.GetAnimator(), "isRunning", destinationReached);

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
            case State.STUNNED:
                Stunned();
                break;
            case State.IDLE:
                Idle();
                break;
            case State.ATTACK:
                Attack();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<Player>();
    }

    public void Init(Character character)
    {
        this.character = character;

        navMeshBorderOffset = character.GetCharacterType().NavMeshBorderOffset;
        searchPlayerRay = character.GetCharacterType().SearchPlayerRay;
        attackRange = character.GetCharacterType().AttackRange;
        distanceToKeepFromPlayer = character.GetCharacterType().DistanceToKeepFromPlayer;
        meshCollider = character.GetCharacterType().MeshCollider;
        impactForce = character.GetCharacterType().ImpactForce;

        navMeshAgent.speed = character.GetCharacterType().CharacterSpeed;

        character.GetHealthSystem().OnHealthAmountChanged += HealthSystem_OnHealthAmountChanged;

        sphereCollider.radius = searchPlayerRay;

        SwitchBehaviour(State.SEARCHING);
    }

    private void HealthSystem_OnHealthAmountChanged(object sender, HealthSystem.OnHealthAmountChangedEventArgs e)
    {
        if (e.amount < 0f)
        {
            SwitchBehaviour(State.STUNNED);
        }
    }

    private void SwitchBehaviour(State behaviourState)
    {
        switch (behaviourState)
        {
            case State.SEARCHING:
                destinationReached = false;
                AnimatorUtils.PlayBoolAnimation(character.GetAnimator(), "Hurt", false);
                currentState = State.SEARCHING;
                break;
            case State.CHASE:
                destinationReached = false;
                navMeshAgent.isStopped = false;
                AnimatorUtils.PlayBoolAnimation(character.GetAnimator(), "Hurt", false);
                currentState = State.CHASE;
                break;
            case State.GO_AWAY:
                navMeshAgent.isStopped = true;
                AnimatorUtils.PlayBoolAnimation(character.GetAnimator(), "Hurt", false);
                currentState = State.GO_AWAY;
                break;
            case State.STUNNED:
                navMeshAgent.isStopped = true;
                rigidBody.AddForce(transform.forward * impactForce * Time.deltaTime, ForceMode.Force);
                AnimatorUtils.PlayBoolAnimation(character.GetAnimator(), "Hurt", true);
                stunTimer = character.GetCharacterType().StunTimerMax;
                currentState = State.STUNNED;
                break;
            case State.IDLE:
                destinationReached = true;
                navMeshAgent.isStopped = true;
                idleTimer = character.GetCharacterType().IdleTimer;
                currentState = State.IDLE;
                break;
            case State.ATTACK:
                destinationReached = true;
                navMeshAgent.isStopped = true;
                AnimatorUtils.PlayBoolAnimation(character.GetAnimator(), "Hurt", false);
                currentState = State.ATTACK;
                break;
        }
    }

    private void SearchForPlayer()
    {
        previousState = State.SEARCHING;

        if (player != null)
        {
            SwitchBehaviour(State.IDLE);
        }

        destinationReached = navMeshAgent.velocity == Vector3.zero;

        if (destinationReached)
        {
            randomPatrolPosition = new Vector3(UnityEngine.Random.Range(navMeshBorderOffset, meshCollider.bounds.size.x - navMeshBorderOffset), 0f, UnityEngine.Random.Range(navMeshBorderOffset, meshCollider.bounds.size.z - navMeshBorderOffset));
        }

        navMeshAgent.SetDestination(randomPatrolPosition);
    }

    private void Chase()
    {
        previousState = State.CHASE;

        float distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);

        destinationReached = distanceFromPlayer <= attackRange;

        if (distanceToKeepFromPlayer != 0f && distanceFromPlayer <= distanceToKeepFromPlayer)
        {
            SwitchBehaviour(State.GO_AWAY);
        }

        if (destinationReached)
        {
            navMeshAgent.isStopped = true;
            SwitchBehaviour(State.ATTACK);
        }
        else
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(player.transform.position);
        }
    }

    private void GoAway()
    {
        previousState = State.GO_AWAY;

        if (navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = false;
        }

        Vector3 driveAway = (this.transform.position - player.transform.position).normalized;

        navMeshAgent.SetDestination(driveAway);

        float chaseDist = Vector3.Distance(this.transform.position, player.transform.position);

        if (chaseDist >= distanceToKeepFromPlayer)
        {
            SwitchBehaviour(State.CHASE);
        }
    }

    private void Stunned()
    {
        previousState = State.STUNNED;

        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0f)
        {
            SwitchBehaviour(State.IDLE);
        }
    }

    private void Idle()
    {
        idleTimer -= Time.deltaTime;
        if(idleTimer <= 0f)
        {
            switch (previousState)
            {
                case State.SEARCHING:
                    SwitchBehaviour(State.CHASE);
                    break;
                case State.CHASE:
                    SwitchBehaviour(State.CHASE);
                    break;
                case State.GO_AWAY:
                    SwitchBehaviour(State.SEARCHING);
                    break;
                case State.STUNNED:
                    SwitchBehaviour(State.CHASE);
                    break;
                case State.IDLE:
                    SwitchBehaviour(State.SEARCHING);
                    break;
                case State.ATTACK:
                    SwitchBehaviour(State.CHASE);
                    break;
            }
        }
    }

    private void Attack()
    {
        previousState = State.ATTACK;

        character.GetAttackSystem().PerformAttack(AttackType.LIGHT);

        SwitchBehaviour(State.IDLE);
    }

    public void ShowWeapon()
    {
        character.GetWeapon().SetActive(true);
    }
    public void HideWeapon()
    {
        character.GetWeapon().SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.OnPossessedCharacterChanged -= EventManager_OnPossessedCharacterChanged;
        character.GetHealthSystem().OnHealthAmountChanged -= HealthSystem_OnHealthAmountChanged; //MAYBE BUG
    }

}
