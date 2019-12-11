using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvasScript : MonoBehaviour
{

    public Text sysTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sysTime.text = System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + " " 
            + System.DateTime.Now.Day + "/" + System.DateTime.Now.Month + "/" + System.DateTime.Now.Year;
    }
}
