using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    Camera myCamera;
    RectTransform rt;

    public float dragSpeed = 2.0f;
    public float scrollSpeed = 1.0f;

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
        if(Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            zoom(difference * 0.005f);

        }else if (isHeld)
        {
            //Debug.Log("Held");

            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = myCamera.ScreenToViewportPoint(mousePos);

            myCamera.transform.localPosition = new Vector3(-(mousePos.x * dragSpeed) + startPosX, -(mousePos.y * dragSpeed) + startPosY, -10);
        }

        zoom(Input.GetAxis("Mouse ScrollWheel") * scrollSpeed);
        

    }

    void LateUpdate()
    {
        var v3 = myCamera.transform.position;
        v3.x = Mathf.Clamp(v3.x, minX, maxX);
        v3.y = Mathf.Clamp(v3.y, minY, maxY);
        myCamera.transform.position = v3;
    }

    void zoom(float increment)
    {
        myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize - (increment*scrollSpeed), 2.4f, 7.4f);

        float vertExtent = myCamera.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        // Calculations assume map is position at the origin
        minX = horzExtent - rt.rect.width / 2.0f;
        maxX = rt.rect.width / 2.0f - horzExtent;
        minY = vertExtent - rt.rect.height / 2.0f;
        maxY = rt.rect.height / 2.0f - vertExtent;
    }
}
