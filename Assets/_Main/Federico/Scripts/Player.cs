using System;
using UnityEngine;

[RequireComponent(typeof(HealthSystem), typeof(Animator))]
public class Player : MonoBehaviour
{
    private const string CHARACTER_LAYER = "Character";

    public event EventHandler OnEnergyAmountChanged;

    [SerializeField] private float slowmoTimeSpeed = 0.75f;
    [SerializeField] private float speed;
    [SerializeField] private GameObject defaultBodyPrefab;
    [SerializeField] private Transform rayMuz;
    [SerializeField] private float energyAmountMax = 100f;
    [SerializeField] private Transform possessedParent;
    [SerializeField] private Transform defaultBodyParent;

    private bool isPossessing;

    private float maxPossessionDistance;
    private Character possessedBodyComponent;
    private Character defaultBodyComponent;

    private GameObject possessedGameObject;

    private float rotationSens;

    private HealthSystem healthSystem;
    private float healtAmountMax;
    private float healthDecreaseTimerMax;

    private float energyAmount;

    private Animator animator;
    private Rigidbody rb;

    private int possessionLayerMask;

    private void Awake()
    {
        maxPossessionDistance = 7.5f;
        rotationSens = 10.0f;

        possessedGameObject = Instantiate(defaultBodyPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, defaultBodyParent);
        possessedBodyComponent = defaultBodyComponent;

        healtAmountMax = 100f;
        healthDecreaseTimerMax = 2f;

        energyAmount = energyAmountMax;

        healthSystem = GetComponent<HealthSystem>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        possessionLayerMask = 1 << LayerMask.NameToLayer(CHARACTER_LAYER);
    }

    private void Start()
    {
        healthSystem.Init(healtAmountMax, healthDecreaseTimerMax);

        PlayerInputSystem.OnPossessionStarted += PlayerInputSystem_OnPossessionStarted;
        PlayerInputSystem.OnPossessionPerformed += PlayerInputSystem_OnPossessionPerformed;

        healthSystem.OnDie += HealthSystem_OnDie;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void HealthSystem_OnDie(object sender, System.EventArgs e)
    {
        if (healthSystem.enabled)
        {
            GameManager.MatchWon = false;
            SceneManagementSystem.LoadWonLooseScene();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManagementSystem.ExitToMainMenu();
        }

        if (isPossessing)
        {
            AnimatorSystem.IsWalking(possessedBodyComponent.GetAnimator(), PlayerInputSystem.GetDirectionNormalized() != Vector2.zero);
            possessedBodyComponent.transform.position = this.transform.position;
        }
        else
        {
            AnimatorSystem.IsWalking(animator, PlayerInputSystem.GetDirectionNormalized() != Vector2.zero);
            healthSystem.DecreaseHealthOverTime();
        }
    }

    private void PlayerInputSystem_OnPossessionPerformed()
    {
        if (isPossessing)
        {
            DePossess();
        }
        else
        {
            Ray ray = new Ray(rayMuz.position, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, maxPossessionDistance, possessionLayerMask))
            {
                Character hitObject = hit.collider.gameObject.GetComponent<Character>();

                if (hitObject != null && energyAmount >= hitObject.GetCharacterType().EnergyCostAmount)
                {
                    Possess(hitObject.gameObject);
                }
            }
        }

        Time.timeScale = 1.0f;
    }

    private void PlayerInputSystem_OnPossessionStarted()
    {
        Time.timeScale = slowmoTimeSpeed;
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

        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<CapsuleCollider>().enabled = true;
        animator.enabled = false;
        Destroy(possessedBodyComponent.gameObject.GetComponent<Rigidbody>());
        Destroy(possessedBodyComponent.gameObject.GetComponent<CapsuleCollider>());

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
        possessedBodyComponent.IsPossessed = true;

        possessedBodyComponent.GetHealthSystem().OnPossessedDie += PossessedBodyComponentHealthSystem_OnPossessedDie;

        healthSystem.enabled = false;

        speed = 2f;

        energyAmount -= possessedBodyComponent.GetCharacterType().EnergyCostAmount;
        energyAmount = Mathf.Clamp(energyAmount, 0f, energyAmountMax);
        OnEnergyAmountChanged?.Invoke(this, EventArgs.Empty);

        possessedBodyComponent.ChangeToDefaultMaterial();

        isPossessing = true;
    }

    private void PossessedBodyComponentHealthSystem_OnPossessedDie(object sender, System.EventArgs e)
    {
        DePossess();
    }

    private void DePossess()
    {
        GetComponent<CapsuleCollider>().enabled = false;

        possessedBodyComponent.GetHealthSystem().OnPossessedDie -= PossessedBodyComponentHealthSystem_OnPossessedDie;

        Destroy(possessedGameObject);

        defaultBodyParent.gameObject.SetActive(true);
        isPossessing = false;

        possessedBodyComponent = defaultBodyComponent;
        possessedGameObject.SetActive(false);

        PlayerInputSystem.OnLightAttackPerformed -= PlayerInputSystem_OnLightAttackPerformed;
        PlayerInputSystem.OnHeavyAttackPerformed -= PlayerInputSystem_OnHeavyAttackPerformed;
        PlayerInputSystem.OnSpecialAbilityPerformed -= PlayerInputSystem_OnSpecialAbilityPerformed;

        EventManager.OnPossessedCharacterChangedCall(null);

        speed = 3f;

        healthSystem.enabled = true;
        animator.enabled = true;
    }

    public void GainEnergyAmount(float amount)
    {
        energyAmount += amount;
        energyAmount = Mathf.Clamp(energyAmount, 0, energyAmountMax);
        OnEnergyAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool HasMaxEnergy()
    {
        return energyAmount >= energyAmountMax;
    }

    public float GetEnergyAmountNormalized()
    {
        return energyAmount / energyAmountMax;
    }

    public bool GetIsPossessing()
    {
        return isPossessing;
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
        PlayerInputSystem.OnLightAttackPerformed -= PlayerInputSystem_OnLightAttackPerformed;
        PlayerInputSystem.OnHeavyAttackPerformed -= PlayerInputSystem_OnHeavyAttackPerformed;
        PlayerInputSystem.OnSpecialAbilityPerformed -= PlayerInputSystem_OnSpecialAbilityPerformed;

        healthSystem.OnDie -= HealthSystem_OnDie;
    }
}
