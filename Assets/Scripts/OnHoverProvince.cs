using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHoverProvince : MonoBehaviour
{

    bool hovered = false;
    public Material tmpMat;
    Material tmpHighlight;
    public Material onEnter;
    public Material onExit;

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        //Save and release the selected variable to ensure colors go back to normal when mouse leaves the gameobject
        if(hovered != true)
        {
            if (transform.parent.gameObject.name != "Map")
            {
                foreach (Transform childProvince in transform.parent.gameObject.transform)
                {
                    tmpMat = childProvince.GetComponent<Renderer>().material;
                    tmpHighlight = new Material(tmpMat);
                    tmpHighlight.SetColor("_Color", new Color(tmpHighlight.color.r + 0.5f, tmpHighlight.color.g + 0.5f, tmpHighlight.color.b + 0.5f));
                    childProvince.GetComponent<Renderer>().material = tmpHighlight;
                    childProvince.GetComponent<Node>().SetOutlineSelected();
                    //childProvince.GetComponent<Renderer>().material = onEnter;
                }
                ProvinceTooltip._instance.ShowTooltip(gameObject.transform.parent.name);
            }
            else
            {
                tmpMat = GetComponent<Renderer>().material;
                tmpHighlight = new Material(tmpMat);
                tmpHighlight.SetColor("_Color", new Color(tmpHighlight.color.r + 0.5f, tmpHighlight.color.g + 0.5f, tmpHighlight.color.b + 0.5f));
                GetComponent<Renderer>().material = tmpHighlight;
                GetComponent<Node>().SetOutlineSelected();
                //GetComponent<Renderer>().material = onEnter;
                ProvinceTooltip._instance.ShowTooltip(gameObject.name);
            }

            hovered = true;
        }

        if (Input.GetMouseButton(0))
        {
            if(transform.parent.gameObject.name != "Map")
            {
                /*foreach (Transform childProvince in transform.parent.gameObject.transform)
                {
                    tmpMat = childProvince.GetComponent<Renderer>().material;
                    tmpHighlight = new Material(tmpMat);
                    tmpHighlight.SetColor("_Color", new Color(tmpHighlight.color.r + 0.5f, tmpHighlight.color.g + 0.5f, tmpHighlight.color.b + 0.5f));
                    childProvince.GetComponent<Renderer>().material = tmpHighlight;
                    childProvince.GetComponent<Node>().SetOutlineSelected();
                    //childProvince.GetComponent<Renderer>().material = onEnter;
                }*/

                Node parentNode = gameObject.transform.parent.GetComponent<Node>();

                ProvinceView._instance.SetActive(parentNode.name,
                    parentNode.province.medQuality,
                    parentNode.province.revUnity,
                    parentNode.province.intersec
                );

            }
            else
            {
                ProvinceView._instance.SetActive(GetComponent<Node>().name,
                    GetComponent<Node>().province.medQuality,
                    GetComponent<Node>().province.revUnity,
                    GetComponent<Node>().province.intersec
                );
            }
            MapHandler.ChangeSelected(gameObject);
        }
    }

    void OnMouseExit()
    {
        
        if(hovered == true)
        {
            if (transform.parent.gameObject.name != "Map")
            {
                foreach (Transform childProvince in transform.parent.gameObject.transform)
                {
                    childProvince.GetComponent<Renderer>().material = tmpMat;
                    childProvince.GetComponent<Node>().UnsetOutlineSelected();
                }
            }
            else
            {
                GetComponent<Renderer>().material = tmpMat;
                GetComponent<Node>().UnsetOutlineSelected();
            }
            ProvinceTooltip._instance.HideTooltip();
            hovered = false;
        }
    }

    public void DeselectProvince()
    {

    }
}
