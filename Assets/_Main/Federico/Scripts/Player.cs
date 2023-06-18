using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(HealthSystem), typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
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

    private CapsuleCollider capsuleCollider;

    private void Awake()
    {
        maxPossEnergy = 100.0f;
        maxPossessionDistance = 7.5f;
        rotationSens = 10.0f;

        possessionEnergy = maxPossEnergy;

        possessedGameObject = Instantiate(defaultBodyPrefab, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z),transform.rotation, defaultBodyParent);
        defaultBodyComponent = defaultBodyPrefab.GetComponent<Character>();
        possessedBodyComponent = defaultBodyComponent;

        healtAmountMax = 100f;
        healthDecreaseTimerMax = 1.5f;

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.Init(healtAmountMax, healthDecreaseTimerMax);
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        PlayerInputSystem.OnPossessionStarted += PlayerInputSystem_OnPossessionStarted;
        PlayerInputSystem.OnPossessionPerformed += PlayerInputSystem_OnPossessionPerformed;
        PlayerInputSystem.OnExitToMainMenuPerformed += PlayerInputSystem_OnExitToMainMenuPerformed;
    }

    private void Update()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        Character otherCharacter = other.gameObject.GetComponent<Character>();

        if (otherCharacter != null)
        {
            healthSystem.ChangeHealthAmount(-otherCharacter.GetAttackSystem().GetActualDamage());
        }
    }

    private void PlayerInputSystem_OnPossessionPerformed()
    {
        if (Physics.Raycast(rayMuz.position, transform.forward, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (Vector3.Distance(hitObject.transform.position, transform.position) <= maxPossessionDistance)
            {
                Debug.Log("Hai Colpito: " + hitObject);

                if (hitObject.GetComponent<Character>() != null)
                {
                    Possess(hitObject);
                }
            }
            else
            {
                Debug.Log("Outrange");
                if (isPossessing)
                {
                    DePossess();
                }
            }
        }
        else
        {
            Debug.Log("Non Hai colpito nulla");

            if (isPossessing)
            {
                DePossess();
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

        controller.Move(movementDirection * speed * Time.fixedDeltaTime);

        Vector2 mousePosition = PlayerInputSystem.GetMousePosition();

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y));

        Vector3 directionToMouse = mouseWorldPosition - transform.position;
        directionToMouse.y = 0f; // Assicurati che la direzione sia piatta (sul piano XZ)

        if (directionToMouse != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToMouse, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSens * Time.deltaTime);
        }
    }

    private void Possess(GameObject obj)
    {
        GameObject oldGO = possessedGameObject;
        transform.position = obj.transform.position;
        transform.rotation = obj.transform.rotation;

        isPossessing = true;
        obj.transform.SetParent(possessedParent);
        possessedBodyComponent = obj.GetComponent<Character>();
        possessedGameObject = obj;
        defaultBodyParent.gameObject.SetActive(false);

        PlayerInputSystem.OnLightAttackPerformed += PlayerInputSystem_OnLightAttackPerformed;
        PlayerInputSystem.OnHeavyAttackPerformed += PlayerInputSystem_OnHeavyAttackPerformed;
        PlayerInputSystem.OnSpecialAbilityPerformed += PlayerInputSystem_OnSpecialAbilityPerformed;

        EventManager.OnPossessedCharacterChangedCall(possessedBodyComponent);

        healthSystem.enabled = false;

        if (oldGO.transform.parent == possessedParent)
        {
            Destroy(oldGO);
        }
    }

    private void DePossess()
    {
        defaultBodyParent.gameObject.SetActive(true);
        isPossessing = false;

        possessedBodyComponent = defaultBodyComponent;
        possessedGameObject.SetActive(false);

        PlayerInputSystem.OnLightAttackPerformed -= PlayerInputSystem_OnLightAttackPerformed;
        PlayerInputSystem.OnHeavyAttackPerformed -= PlayerInputSystem_OnHeavyAttackPerformed;
        PlayerInputSystem.OnSpecialAbilityPerformed -= PlayerInputSystem_OnSpecialAbilityPerformed;

        EventManager.OnPossessedCharacterChangedCall(null);

        healthSystem.enabled = true;
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
