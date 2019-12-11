using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public GameObject GameCanvas;
    public GameObject MenuCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ToggleMenuCanvas()
    {
        MenuCanvas.SetActive(!MenuCanvas.activeSelf); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
