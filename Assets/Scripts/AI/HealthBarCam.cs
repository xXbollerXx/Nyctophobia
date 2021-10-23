using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarCam : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.back, mainCamera.transform.rotation * Vector3.down);
    }
}
