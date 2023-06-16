using UnityEngine;

public class PickupMovementEffect : MonoBehaviour
{
    [SerializeField] private PickupSO pickup;

    private float moveSpeed;
    private float rotationSpeed;

    private float startY;

    private void Awake()
    {
        startY = transform.position.y;
        moveSpeed = pickup.MoveSpeed;
        rotationSpeed = pickup.RotationSpeed;
    }

    private void Update()
    {
        float newY = startY + Mathf.Sin(Time.time * moveSpeed);

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
