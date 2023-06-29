using UnityEngine;

[RequireComponent(typeof(HealthSystem), typeof(Animator))]
public class Player : MonoBehaviour
{
    private const string CHARACTER_LAYER = "Character";

    [SerializeField] private float slowmoTimeSpeed = 0.75f;
    [SerializeField] private float speed;
    [SerializeField] private GameObject defaultBodyPrefab;
    [SerializeField] private Transform rayMuz;
    [SerializeField] private float possessionEnergy;
    [SerializeField] private Transform possessedParent;
    [SerializeField] private Transform defaultBodyParent;
    [SerializeField] private float possEnergyDecrementSpeed = 10.0f;

    private CharacterController controller;
    private bool isPossessing;

    private float maxPossEnergy;
    private float maxPossessionDistance;
    private Character possessedBodyComponent;
    private Character defaultBodyComponent;

    private GameObject possessedGameObject;

    private float rotationSens;

    private HealthSystem healthSystem;
    private float healtAmountMax;
    private float healthDecreaseTimerMax;

    private Animator animator;
    private Rigidbody rb;

    private int possessionLayerMask;

    private void Awake()
    {
        maxPossEnergy = 100.0f;
        maxPossessionDistance = 7.5f;
        rotationSens = 10.0f;

        possessionEnergy = maxPossEnergy;

        possessedGameObject = Instantiate(defaultBodyPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, defaultBodyParent);
        possessedBodyComponent = defaultBodyComponent;

        healtAmountMax = 100f;
        healthDecreaseTimerMax = 1.5f;

        healthSystem = GetComponent<HealthSystem>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        possessionLayerMask = 1 << LayerMask.NameToLayer(CHARACTER_LAYER);
    }

    private void Start()
    {
        healthSystem.Init(healtAmountMax, healthDecreaseTimerMax);

        PlayerInputSystem.OnPossessionStarted += PlayerInputSystem_OnPossessionStarted;
        PlayerInputSystem.OnPossessionPerformed += PlayerInputSystem_OnPossessionPerformed;
        PlayerInputSystem.OnExitToMainMenuPerformed += PlayerInputSystem_OnExitToMainMenuPerformed;
    }

    private void Update()
    {
        AnimatorSystem.IsWalking(animator, PlayerInputSystem.GetDirectionNormalized() != Vector2.zero);

        if (isPossessing)
        {
            possessionEnergy -= Time.deltaTime * possEnergyDecrementSpeed;

            if (possessionEnergy <= 0.0f)
            {
                DePossess();
            }
        }
        else
        {
            possessionEnergy = Mathf.Clamp(possessionEnergy + Time.deltaTime * 10.0f, 0, maxPossEnergy);    //regain energy DEBUG
            healthSystem.DecreaseHealthOverTime();
        }
    }

    private void PlayerInputSystem_OnPossessionPerformed()
    {
        if(isPossessing)
        {
            DePossess();
        }
        else
        {
            Ray ray = new Ray(rayMuz.position, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, maxPossessionDistance, possessionLayerMask))
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject.GetComponent<Character>() != null)
                {
                    Possess(hitObject);
                }
            }
        }

        Time.timeScale = 1.0f;
    }

    private void PlayerInputSystem_OnPossessionStarted()
    {
        Time.timeScale = slowmoTimeSpeed;
    }

    private void PlayerInputSystem_OnExitToMainMenuPerformed()
    {
        SceneManagementSystem.ExitToMainMenu();
    }

    private void PlayerInputSystem_OnHeavyAttackPerformed()
    {
        possessedBodyComponent.GetAttackSystem().PerformAttack(AttackType.HEAVY);
    }

    private void PlayerInputSystem_OnLightAttackPerformed()
    {
        possessedBodyComponent.GetAttackSystem().PerformAttack(AttackType.LIGHT);
    }

    private void PlayerInputSystem_OnSpecialAbilityPerformed()
    {
        possessedBodyComponent.GetAttackSystem().PerformAttack(AttackType.SPECIAL);
    }

    private void FixedUpdate()
    {
        float inputHorizontal = PlayerInputSystem.GetDirectionNormalized().x;
        float inputVertical = PlayerInputSystem.GetDirectionNormalized().y;

        Vector3 movementDirection = new Vector3(inputHorizontal, 0f, inputVertical);
        movementDirection.y = 0f;
        movementDirection.Normalize();

        rb.MovePosition(rb.position + movementDirection * speed * Time.fixedDeltaTime);
        //controller.Move(movementDirection * speed * Time.fixedDeltaTime);

        Vector2 mousePosition = PlayerInputSystem.GetMousePosition();

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y));

        Vector3 directionToMouse = mouseWorldPosition - transform.position;
        directionToMouse.y = 0f; // Assicurati che la direzione sia piatta (sul piano XZ)
        directionToMouse.Normalize();

        if (directionToMouse != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToMouse, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSens * Time.fixedDeltaTime);
        }
    }

    private void Possess(GameObject obj)
    {
        GameObject oldGO = possessedGameObject;

        possessedBodyComponent = obj.GetComponent<Character>();

        //GetComponent<CapsuleCollider>().enabled = false;                  //commented line cause we need this collider to make enemies find player
        GetComponent<Rigidbody>().useGravity = false;
        animator.enabled = false;
        Destroy(possessedBodyComponent.gameObject.GetComponent<Rigidbody>());
        possessedBodyComponent.SetAnimator(animator);

        transform.position = obj.transform.position;
        transform.rotation = obj.transform.rotation;

        obj.transform.SetParent(possessedParent);
        possessedGameObject = obj;
        defaultBodyParent.gameObject.SetActive(false);

        PlayerInputSystem.OnLightAttackPerformed += PlayerInputSystem_OnLightAttackPerformed;
        PlayerInputSystem.OnHeavyAttackPerformed += PlayerInputSystem_OnHeavyAttackPerformed;
        PlayerInputSystem.OnSpecialAbilityPerformed += PlayerInputSystem_OnSpecialAbilityPerformed;

        EventManager.OnPossessedCharacterChangedCall(possessedBodyComponent);

        possessedBodyComponent.GetAISystem().DisalbeAI();

        healthSystem.enabled = false;

        isPossessing = true;

        //if (oldGO.transform.parent == possessedParent)
        //{
        //    Destroy(oldGO);
        //}
    }

    private void DePossess()
    {
        //GetComponent<CapsuleCollider>().enabled = true;           //commented line cause we need this collider to make enemies find player
        GetComponent<Rigidbody>().useGravity = true;

        Destroy(possessedGameObject);

        defaultBodyParent.gameObject.SetActive(true);
        isPossessing = false;

        possessedBodyComponent = defaultBodyComponent;
        possessedGameObject.SetActive(false);

        PlayerInputSystem.OnLightAttackPerformed -= PlayerInputSystem_OnLightAttackPerformed;
        PlayerInputSystem.OnHeavyAttackPerformed -= PlayerInputSystem_OnHeavyAttackPerformed;
        PlayerInputSystem.OnSpecialAbilityPerformed -= PlayerInputSystem_OnSpecialAbilityPerformed;

        EventManager.OnPossessedCharacterChangedCall(null);

        healthSystem.enabled = true;
        animator.enabled = true;
    }

    public Character GetPossessedBodyComponent()
    {
        return possessedBodyComponent;
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }

    private void OnDestroy()
    {
        PlayerInputSystem.OnPossessionStarted -= PlayerInputSystem_OnPossessionStarted;
        PlayerInputSystem.OnPossessionPerformed -= PlayerInputSystem_OnPossessionPerformed;
        PlayerInputSystem.OnExitToMainMenuPerformed -= PlayerInputSystem_OnExitToMainMenuPerformed;
        PlayerInputSystem.OnLightAttackPerformed -= PlayerInputSystem_OnLightAttackPerformed;       //MAYBE BUG: maybe we are unsubscribing from an event and we are not subscribed
        PlayerInputSystem.OnHeavyAttackPerformed -= PlayerInputSystem_OnHeavyAttackPerformed;
        PlayerInputSystem.OnSpecialAbilityPerformed -= PlayerInputSystem_OnSpecialAbilityPerformed;
    }
}
