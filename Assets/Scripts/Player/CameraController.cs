using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Movement Settings")]
    public Vector3 Offset;
    public float Bias = 0.125f;

    [Header("Camera Movement Settings")]
    public float RayMaxDistance = 10;
    public LayerMask RaycastLayer;

    private Transform Target, CamT;
    private Vector3 Velocity = Vector3.zero;
    private Camera Cam;

    // Start is called before the first frame update
    void Start()
    {
        CamT = Camera.main.transform;
        Cam = Camera.main;
        Target = transform;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraPosition();
        /*Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, RayMaxDistance, RaycastLayer)){

            transform.LookAt(hit.point + new Vector3(0, transform.position.y, 0));
        }*/
    }

    private void UpdateCameraPosition()
    {
        Vector3 DesiredPosition = Target.position + Offset;
        CamT.position = Vector3.SmoothDamp(CamT.position, DesiredPosition, ref Velocity, Bias);
    }
}
