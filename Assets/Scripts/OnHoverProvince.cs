using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHoverProvince : MonoBehaviour
{

    public Material onEnter;
    public Material onExit;

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        //Debug.Log("Mouse is over GameObject.");
        if(transform.parent.gameObject.name != "Map")
        {
            foreach(Transform childProvince in transform.parent.gameObject.transform)
            {
                childProvince.GetComponent<Renderer>().material = onEnter;
            }
        }
        else
        {
            GetComponent<Renderer>().material = onEnter;
        }

        ProvinceTooltip._instance.ShowTooltip(gameObject.name);
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        //Debug.Log("Mouse is no longer on GameObject.");
        if (transform.parent.gameObject.name != "Map")
        {
            foreach (Transform childProvince in transform.parent.gameObject.transform)
            {
                childProvince.GetComponent<Renderer>().material = onExit;
            }
        }
        else
        {
            GetComponent<Renderer>().material = onExit;
        }
        ProvinceTooltip._instance.HideTooltip();
    }
}
