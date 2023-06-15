using UnityEngine;

public class IsoCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float cameraSpeed;

    [SerializeField] private Vector3 offsetPosition;
    [SerializeField] private Vector3 offsetAngle;

    private void Awake()
    {
        transform.position = target.position + offsetPosition;
        transform.rotation = Quaternion.Euler(offsetAngle);
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offsetPosition, Time.fixedDeltaTime * cameraSpeed);
    }
}
