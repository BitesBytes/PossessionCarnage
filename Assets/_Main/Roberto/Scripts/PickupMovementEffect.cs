using UnityEngine;

public class PickupMovementEffect : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 15f;

    private float startY;

    private void Awake()
    {
        startY = transform.position.y;
    }

    private void Update()
    {
        float newY = startY + Mathf.Sin(Time.time * moveSpeed);

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
