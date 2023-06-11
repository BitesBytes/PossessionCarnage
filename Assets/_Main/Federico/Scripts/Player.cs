using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject defaultBodyPrefab;
    [SerializeField] private Transform rayMuz;
    [SerializeField] private float possessionEnergy;

    private bool isPossessing;
    private Transform possessedParent;
    private Transform defaultBodyParent;

    private float maxPossEnergy = 100.0f;
    private float maxPossessionDistance = 5.0f;
    private PossessableEntity possessedBodyComponent;
    private PossessableEntity defaultBodyComponent;

    private GameObject possessedGameObject;

    private float rotationSens = 2.0f;

    private void Awake()
    {
        possessionEnergy = maxPossEnergy;
        possessedParent = transform.Find("Possessed");
        defaultBodyParent = transform.Find("Default");

        possessedGameObject = Instantiate(defaultBodyPrefab);
        defaultBodyComponent = defaultBodyPrefab.GetComponent<PossessableEntity>();
        possessedBodyComponent = defaultBodyComponent;

        possessedGameObject.transform.SetParent(defaultBodyParent);
    }

    private void Update()
    {
        if (isPossessing)
        {
            possessionEnergy -= Time.deltaTime * 10.0f;

            if (possessionEnergy <= 0)
            {
                DePossess();
            }
        }
        else
        {
            //possessionEnergy += Time.deltaTime * 10.0f;
            possessionEnergy = Mathf.Clamp(possessionEnergy + Time.deltaTime * 10.0f, 0, maxPossEnergy);
        }

        float mouseX = Input.GetAxis("Mouse X") * rotationSens;

        transform.Rotate(Vector3.up, mouseX);

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += speed * Time.deltaTime * new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            possessedBodyComponent.UseAbility();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("SLOWMO");
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(rayMuz.position, transform.forward, out hit))
            {
                GameObject hitObject = hit.collider.transform.parent.transform.parent.gameObject;

                if (Vector3.Distance(hitObject.transform.position, transform.position) <= maxPossessionDistance)
                {
                    Debug.Log("Hai Colpito: " + hitObject);

                    if (hitObject.GetComponent<PossessableEntity>() != null)
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
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManagementSystem.ExitToMainMenu();
        }
    }

    private void Possess(GameObject obj)
    {
        GameObject oldGO = possessedGameObject;
        transform.position = obj.transform.position;
        transform.rotation = obj.transform.rotation;

        isPossessing = true;
        obj.transform.SetParent(possessedParent);
        possessedBodyComponent = obj.GetComponent<PossessableEntity>();
        possessedGameObject = obj;
        defaultBodyParent.gameObject.SetActive(false);


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
    }
}
