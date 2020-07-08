using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusMap : MonoBehaviour
{

    public float cameraDistance;

    public void CenterCameraFocus(Vector2 center)
    {
        transform.position = new Vector3(center.x, center.y, cameraDistance);
    }
}
