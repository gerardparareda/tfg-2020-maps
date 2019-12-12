using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public GameObject GameCanvas;
    public GameObject MenuCanvas;
    public GameObject WikiaCanvas;

    bool isInGame = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ToggleMenuCanvas()
    {
        if (isInGame)
        {
            isInGame = false;
            MenuCanvas.SetActive(!MenuCanvas.activeSelf);
        }
        else
        {
            MenuCanvas.SetActive(false);
            WikiaCanvas.SetActive(false);
            isInGame = true;
        }
    }

    public void ToggleWikiaCanvas()
    {
        MenuCanvas.SetActive(!MenuCanvas.activeSelf);
        WikiaCanvas.SetActive(!WikiaCanvas.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
