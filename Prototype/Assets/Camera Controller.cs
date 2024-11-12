using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float ZoomSpeed;
    float Zoom = 5;
    void Update()
    {
        Zoom -= Input.mouseScrollDelta.y * ZoomSpeed;
        if (Zoom < 5) Zoom = 5;
        Camera.main.orthographicSize = Zoom;
    }
}
