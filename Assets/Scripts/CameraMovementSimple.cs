using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementSimple : MonoBehaviour
{
    public float panSpeed = 2.0f;
    public float scrollSpeed = 2.0f;
    public float maxScroll = -3.0f;
    public float minScroll = -0.4012633f;
    public float maxPanHoritzontal = -3.0f;
    public float maxPanVertical = -0.4012633f;
    public float minPanHoritzontal = -3.0f;
    public float minPanVertical = -0.4012633f;
    Vector3 newPos;

    void Start()
    {
        newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);    
    }

    void Update()
    {
        newPos.y += Input.GetAxis("Vertical") * panSpeed * Time.deltaTime;
        newPos.x += Input.GetAxis("Horizontal") * panSpeed * Time.deltaTime;
        newPos.z += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;

        newPos.x = Mathf.Clamp(newPos.x, minPanHoritzontal, maxPanHoritzontal);
        newPos.y = Mathf.Clamp(newPos.y, minPanVertical, maxPanVertical);
        newPos.z = Mathf.Clamp(newPos.z, maxScroll, minScroll);

        transform.position = newPos;
    }
}
