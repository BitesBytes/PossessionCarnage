using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody), typeof(Animator))]
public class AISystem : MonoBehaviour
{
    private enum State
    {
        SEARCHING,
        CHASE,
        GO_AWAY,
        STUNNED
    }

    private Character character;
    private NavMeshAgent navMeshAgent;
    private Character actualPlayer;
    private Rigidbody rigidBody;
    private MeshCollider meshCollider;

    private State currentState;

    private bool destinationReached;
    private Vector3 randomPatrolPosition;
    private float navMeshBorderOffset;
    private float searchPlayerRay;
    private float attackRange;
    private float distanceToKeepFromPlayer;

    private float stunTimer;
    private float impactForce;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        navMeshAgent = GetComponent<NavMeshAgent>();

        SwitchBehaviour(State.SEARCHING);
    }

    private void Start()
    {
        EventManager.OnPossessedCharacterChanged += EventManager_OnPossessedCharacterChanged;
    }

    private void EventManager_OnPossessedCharacterChanged(Character character)
    {
        actualPlayer = character;

        SwitchBehaviour(State.SEARCHING);
    }

    private void Update()
    {
        if (actualPlayer != null)
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
                case State.STUNNED:
                    Stunned();
                    break;
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
        impactForce = character.GetCharacterType().ImpactForce;

        navMeshAgent.speed = character.GetCharacterType().CharacterSpeed;

        character.GetHealthSystem().OnHealthAmountChanged += HealthSystem_OnHealthAmountChanged;
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
                currentState = State.GO_AWAY;
                break;
            case State.STUNNED:
                navMeshAgent.isStopped = true;
                rigidBody.AddForce(transform.forward * impactForce * Time.deltaTime, ForceMode.Force);
                AnimatorUtils.PlayBoolAnimation(character.GetAnimator(), "Hurt", true);
                stunTimer = character.GetCharacterType().StunTimerMax;
                currentState = State.STUNNED;
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

        if (Vector3.Distance(actualPlayer.transform.position, transform.position) <= searchPlayerRay)
        {
            AnimatorUtils.PlayBoolAnimation(character.GetAnimator(), "SeenPlayer", true);
            SwitchBehaviour(State.CHASE);
        }

        if (navMeshAgent.velocity == Vector3.zero)
        {
            AnimatorUtils.PlayBoolAnimation(character.GetAnimator(), "isRunning", false);
        }

        if (navMeshAgent.velocity != Vector3.zero)
        {
            AnimatorUtils.PlayBoolAnimation(character.GetAnimator(), "isRunning", true);
        }

        navMeshAgent.SetDestination(randomPatrolPosition);
    }

    private void Chase()
    {
        float distanceFromPlayer = Vector3.Distance(this.transform.position, actualPlayer.transform.position);

        destinationReached = distanceFromPlayer <= attackRange;

        if (navMeshAgent.velocity != Vector3.zero)
        {
            AnimatorUtils.PlayBoolAnimation(character.GetAnimator(), "SeenPlayer", false);
            AnimatorUtils.PlayBoolAnimation(character.GetAnimator(), "isRunning", true);
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
            navMeshAgent.SetDestination(actualPlayer.transform.position);
        }
    }

    private void GoAway()
    {
        if (navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = false;
        }

        Vector3 driveAway = (this.transform.position - actualPlayer.transform.position).normalized;

        navMeshAgent.SetDestination(driveAway);

        float chaseDist = Vector3.Distance(this.transform.position, actualPlayer.transform.position);

        if (chaseDist >= distanceToKeepFromPlayer)
        {
            SwitchBehaviour(State.CHASE);
        }
    }

    private void Stunned()
    {
        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0f)
        {
            SwitchBehaviour(State.CHASE);
        }
    }

    public void ShowWeapon()
    {
        character.GetWeapon().SetActive(true);
    }

    private void OnDestroy()
    {
        EventManager.OnPossessedCharacterChanged -= EventManager_OnPossessedCharacterChanged;
        character.GetHealthSystem().OnHealthAmountChanged -= HealthSystem_OnHealthAmountChanged; //MAYBE BUG
    }

}
