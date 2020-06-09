using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    public FabricatedMap fabricatedMap;
    int nX, nY;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.SetTexture("_MainTex", fabricatedMap.landMap);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.point);
                GetTexturePositionFromUV(hit.textureCoord.x, hit.textureCoord.y);
                if(fabricatedMap.landMap.GetPixel(nX, nY).r +
                    fabricatedMap.landMap.GetPixel(nX, nY).g +
                    fabricatedMap.landMap.GetPixel(nX, nY).b == 0)
                {
                    Debug.Log("Water");
                } else
                {
                    Debug.Log("Land");
                }
            }
        }
    }

    void GetTexturePositionFromUV(float x, float y)
    {
        nX = Mathf.FloorToInt(x * fabricatedMap.landMap.width);
        nY = Mathf.FloorToInt(y * fabricatedMap.landMap.height);
    }
}
