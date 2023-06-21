using UnityEngine;

public class IsoCamera : MonoBehaviour
{
    //[SerializeField] private Transform target;
    //[SerializeField] private float cameraSpeed;

    //[SerializeField] private Vector3 offsetPosition;
    //[SerializeField] private Vector3 offsetAngle;

    //private void Awake()
    //{
    //    transform.position = target.position + offsetPosition;
    //    transform.rotation = Quaternion.Euler(offsetAngle);
    //}

    //private void FixedUpdate()
    //{
    //    transform.position = Vector3.Lerp(transform.position, target.position + offsetPosition, Time.fixedDeltaTime * cameraSpeed);
    //}

    [SerializeField] private Transform target;
    [SerializeField] private float positionSmoothness = 1.5f; // Valore che controlla la morbidezza del movimento della telecamera
    [SerializeField] private float rotationSmoothness = 1.5f; // Valore che controlla la morbidezza della rotazione della telecamera

    [SerializeField] private Vector3 rotationOffset; // Offset per la rotazione della telecamera
    [SerializeField] private Vector3 positionOffset; // Offset tra la telecamera e il giocatore

    private void Start()
    {
        // Imposta la posizione iniziale della telecamera
        transform.position = target.position + positionOffset;

        // Imposta la rotazione iniziale della telecamera
        Quaternion initialRotation = Quaternion.LookRotation(target.position - transform.position) * Quaternion.Euler(rotationOffset);
        transform.rotation = initialRotation;
    }

    private void Update()
    {
        // Assicurati che il riferimento al giocatore sia valido
        if (target != null)
        {
            // Calcola la posizione desiderata della telecamera
            Vector3 desiredPosition = target.position + positionOffset;

            // Interpola gradualmente la posizione corrente della telecamera verso la posizione desiderata
            transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSmoothness * Time.deltaTime);

            // Calcola la rotazione desiderata della telecamera
            Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position) * Quaternion.Euler(rotationOffset);

            // Interpola gradualmente la rotazione corrente della telecamera verso la rotazione desiderata
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotationSmoothness * Time.deltaTime);
        }
    }
}
