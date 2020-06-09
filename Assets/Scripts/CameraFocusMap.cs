using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusMap : MonoBehaviour
{

    public GameObject topoJsonMap;

    public float cameraDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CenterCameraFocus(Vector2 center)
    {
        transform.position = new Vector3(center.x, center.y, cameraDistance);
    }
}
