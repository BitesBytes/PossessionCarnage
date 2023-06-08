using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TreeEditor;
using UnityEditor.SearchService;
using UnityEditor.Timeline;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject defaultBodyPrefab;
    [SerializeField] private Transform rayMuz;

    private bool isPossessing;
    private Transform possessed;
    private Transform def;

    private float maxPossessionDistance = 5.0f;
    private PossessableEntity possessedBody;
    private PossessableEntity defaultBodyPoss;

    private GameObject possessedGameObject;

    private float rotationSens = 2.0f;

    private void Awake()
    {
        possessedGameObject = Instantiate(defaultBodyPrefab);
        possessed = transform.Find("Possessed");
        def = transform.Find("Default");

        defaultBodyPoss = defaultBodyPrefab.GetComponent<PossessableEntity>();
        possessedBody = defaultBodyPoss;

        possessedGameObject.transform.SetParent(def);
    }
    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSens;

        transform.Rotate(Vector3.up, mouseX);

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += speed * Time.deltaTime * new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            possessedBody.UseAbility();
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
    }

    private void Possess(GameObject obj)
    {
        GameObject oldGO = possessedGameObject;
        transform.position = obj.transform.position;
        transform.rotation = obj.transform.rotation;

        isPossessing = true;
        obj.transform.SetParent(possessed);
        possessedBody = obj.GetComponent<PossessableEntity>();
        possessedGameObject = obj;
        def.gameObject.SetActive(false);

        //Destroy(oldGO);
    }

    private void DePossess()
    {
        def.gameObject.SetActive(true);
        possessedGameObject.SetActive(false);
        possessedBody = defaultBodyPoss;
        isPossessing = false;
    }
}
