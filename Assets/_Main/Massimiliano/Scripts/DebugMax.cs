using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class DebugMax : MonoBehaviour
{
    private HealthManager healthManager;
    private AIBehaviourManager aiBehaviourManager;
    private Vector3 direction; //DEBUG
    private CharacterController controller; //DEBUG
    private float speed = 2f; //DEBUG

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        healthManager = HealthManager.Instance;
        aiBehaviourManager = AIBehaviourManager.Instance;

        aiBehaviourManager.SetDebugMax(this);
    }

    private void Update()
    {
        //DEBUG
        if (Input.GetKeyDown(KeyCode.Space))
        {
            healthManager.PossessedCharacterChanged(this.gameObject);
        }

        Kinematics(); //DEBUG
    }

    //DEBUG
    private void Kinematics()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        direction = new Vector3(x, 0, z).normalized;

        controller.Move(direction * Time.deltaTime * speed);
    }
}
