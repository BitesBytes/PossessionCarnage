using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float distance;
    [SerializeField] private Quaternion cameraRotation;

    private Camera mainCamera;
    private GameObject owner;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    private void Start()
    {
        mainCamera.transform.rotation = cameraRotation;
        owner = GameObject.FindGameObjectWithTag("Player");

        mainCamera.transform.position = owner.transform.position + cameraOffset;
    }
}
