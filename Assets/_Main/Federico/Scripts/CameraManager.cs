using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float distance;
    [SerializeField] private Quaternion cameraRotation;
    [SerializeField] private GameObject owner;

    private Camera mainCamera;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    private void Start()
    {
        mainCamera.transform.rotation = cameraRotation;
        mainCamera.transform.position = owner.transform.position + cameraOffset;
    }
}
