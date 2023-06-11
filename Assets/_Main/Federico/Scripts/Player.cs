using UnityEngine;

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

    private void Awake()
    {
        maxPossEnergy = 100.0f;
        maxPossessionDistance = 5.0f;
        rotationSens = 2.0f;

        possessionEnergy = maxPossEnergy;

        possessedGameObject = Instantiate(defaultBodyPrefab);
        defaultBodyComponent = defaultBodyPrefab.GetComponent<Character>();
        possessedBodyComponent = defaultBodyComponent;

        possessedGameObject.transform.SetParent(defaultBodyParent);
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
        }

        float mouseX = Input.GetAxis("Mouse X") * rotationSens;

        transform.Rotate(Vector3.up, mouseX);
    }

    private void PlayerInputSystem_OnPossessionPerformed()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayMuz.position, transform.forward, out hit))
        {
            GameObject hitObject = hit.collider.transform.parent.transform.parent.gameObject;

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
        Vector3 movementDirection = Camera.main.transform.TransformDirection(new Vector3(PlayerInputSystem.GetDirectionNormalized().x, 0f, PlayerInputSystem.GetDirectionNormalized().y));
        movementDirection.y = 0f;
        controller.Move(movementDirection * speed * Time.deltaTime);
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
