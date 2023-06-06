using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayer : MonoBehaviour
{
    private float speed = 2f;
    private CharacterController controller;
    private Camera mainCamera;
    private Vector3 direction;
    [SerializeField] private Vector3 cameraOffset;


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
