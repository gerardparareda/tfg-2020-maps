using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    Camera myCamera;
    RectTransform rt;

    public float dragSpeed = 2.0f;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    float startPosX;
    float startPosY;
    bool isHeld = false;


    // Start is called before the first frame update
    void Start()
    {
        //myTransform = GetComponent<Transform>();
        myCamera = Camera.main;
        rt = GetComponent<RectTransform>();

        float vertExtent = myCamera.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        // Calculations assume map is position at the origin
        minX = horzExtent - rt.rect.width / 2.0f;
        maxX = rt.rect.width / 2.0f - horzExtent;
        minY = vertExtent - rt.rect.height / 2.0f;
        maxY = rt.rect.height / 2.0f - vertExtent;
    }

    private void OnMouseDown()
    {
        isHeld = true;

        Vector3 mousePos;
        mousePos = Input.mousePosition;
        mousePos = myCamera.ScreenToViewportPoint(mousePos);

        startPosX = (mousePos.x * dragSpeed) + myCamera.transform.localPosition.x;
        startPosY = (mousePos.y * dragSpeed) + myCamera.transform.localPosition.y;
    }

    private void OnMouseUp()
    {
        isHeld = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHeld)
        {
            //Debug.Log("Held");

            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = myCamera.ScreenToViewportPoint(mousePos);

            myCamera.transform.localPosition = new Vector3(-(mousePos.x * dragSpeed) + startPosX, -(mousePos.y * dragSpeed) + startPosY, -10);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            Debug.Log("scrolling");
            myCamera.orthographicSize -= 0.5f;
            if (myCamera.orthographicSize < 2.4f)
            {
                myCamera.orthographicSize = 2.4f;
            }

            float vertExtent = myCamera.orthographicSize;
            float horzExtent = vertExtent * Screen.width / Screen.height;

            // Calculations assume map is position at the origin
            minX = horzExtent - rt.rect.width / 2.0f;
            maxX = rt.rect.width / 2.0f - horzExtent;
            minY = vertExtent - rt.rect.height / 2.0f;
            maxY = rt.rect.height / 2.0f - vertExtent;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            myCamera.orthographicSize += 0.5f;
            if(myCamera.orthographicSize > 7.4f)
            {
                myCamera.orthographicSize = 7.4f;
            }

            float vertExtent = myCamera.orthographicSize;
            float horzExtent = vertExtent * Screen.width / Screen.height;

            // Calculations assume map is position at the origin
            minX = horzExtent - rt.rect.width / 2.0f;
            maxX = rt.rect.width / 2.0f - horzExtent;
            minY = vertExtent - rt.rect.height / 2.0f;
            maxY = rt.rect.height / 2.0f - vertExtent;
        }

    }

    void LateUpdate()
    {
        var v3 = myCamera.transform.position;
        v3.x = Mathf.Clamp(v3.x, minX, maxX);
        v3.y = Mathf.Clamp(v3.y, minY, maxY);
        myCamera.transform.position = v3;
    }
}
