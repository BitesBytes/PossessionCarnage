using UnityEngine;

public class DebugPlayer : MonoBehaviour
{
    
    [SerializeField] private Vector3 cameraOffset;
    private float speed = 2f;
    private CharacterController controller;
    private Camera mainCamera;
    private Vector3 direction;

    void Start()
    {
        direction = Vector3.zero;
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }


    void Update()
    {
        mainCamera.transform.position = this.transform.position + cameraOffset;

        Kinematics();
    }

    private void Kinematics()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        direction = new Vector3(x, 0, z).normalized;

        controller.Move(direction * Time.deltaTime * speed);
    }
}
