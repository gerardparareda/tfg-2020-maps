using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    List<GameObject> nodes;
    GameMap gameMap;
    public GameObject nodePrefab;
    public static GameObject selectedProvince;

    public void InitializeMap(List<GameObject> nodes)
    {
        this.nodes = nodes;
        FixProvinces();

        gameMap = new GameMap(nodes);

    }

    void FixProvinces()
    {
        for(int i = 0; i < nodes.Count-1; i++)
        {
            if(nodes[i].name == nodes[i + 1].name)
            {
                GameObject tmpGameObject = Instantiate(nodePrefab, gameObject.transform.position, Quaternion.identity);
                tmpGameObject.GetComponent<LineRenderer>().enabled = false;
                tmpGameObject.name = nodes[i].name;
                tmpGameObject.transform.parent = nodes[i].transform.parent;
                tmpGameObject.AddComponent<Node>();
                tmpGameObject.GetComponent<Node>().province = nodes[i].GetComponent<Node>().province;
                nodes[i].transform.parent = tmpGameObject.transform;
                nodes[i + 1].transform.parent = tmpGameObject.transform;
                nodes.RemoveAt(i + 1);
                nodes.RemoveAt(i);
                nodes.Add(tmpGameObject);
                i -= 1; //Perquè n'hem eliminat dos
            }
        }
    }

    public void ChangeMapPolitical()
    {
        foreach (GameObject node in nodes)
        {
            if (node.transform.childCount > 0)
            {
                //
                float revUnity = node.GetComponent<Node>().province.revUnity;
                foreach (Transform nodeChild in node.transform)
                {
                    nodeChild.GetComponent<Renderer>().material.SetColor("_Color", new Color(revUnity, 0, 0));
                    //Debug.Log(node.name + " Med: " + medQuality);
                }
            }
            else
            {
                float revUnity = node.GetComponent<Node>().province.revUnity;
                node.GetComponent<Renderer>().material.SetColor("_Color", new Color(revUnity, 0, 0));
                //Debug.Log(node.name + " Med: " + medQuality);
            }
        }
    }

    public void ChangeMapMediambiental()
    {
        foreach(GameObject node in nodes)
        {
            if (node.transform.childCount > 0)
            {
                float medQuality = node.GetComponent<Node>().province.medQuality;
                foreach (Transform nodeChild in node.transform){
                    nodeChild.GetComponent<Renderer>().material.SetColor("_Color", new Color(0, medQuality, 0));
                    //Debug.Log(node.name + " Med: " + medQuality);
                }
            }
            else
            {
                float medQuality = node.GetComponent<Node>().province.medQuality;
                node.GetComponent<Renderer>().material.SetColor("_Color", new Color(0, medQuality, 0));
                //Debug.Log(node.name + " Med: " + medQuality);
            }
        }
    }

    public void ChangeMapIntersectionality()
    {
        foreach (GameObject node in nodes)
        {
            if (node.transform.childCount > 0)
            {
                float intersec = node.GetComponent<Node>().province.intersec;
                foreach (Transform nodeChild in node.transform)
                {
                    nodeChild.GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 0, intersec));
                    //Debug.Log(node.name + " Med: " + medQuality);
                }
            }
            else
            {
                float intersec = node.GetComponent<Node>().province.intersec;
                node.GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 0, intersec));
                //Debug.Log(node.name + " Med: " + medQuality);
            }
        }
    }

    public static void ChangeSelected(GameObject selected)
    {
        if(selectedProvince != null)
        {
            selectedProvince.GetComponent<OnHoverProvince>().DeselectProvince();
        }
        selectedProvince = selected;
    }
}

class GameMap
{
    float totalMedQuality;
    float totalRevUnity;
    float totalIntersec;

    List<Province> provinces;

    public GameMap(List<GameObject> nodes)
    {
        Province newProv;

        totalMedQuality = 1.0f;
        totalRevUnity = 0.0f;
        totalIntersec = 0.0f;

        provinces = new List<Province>();
        
        for(int i = 0; i < nodes.Count; i++)
        {
            newProv = new Province(nodes[i].name);
            provinces.Add(newProv);
            if(nodes[i].GetComponent<Node>() == null)
            {
                //Per evitar que dos nodes de la mateixa província tinguin dues províncies diferents només assigno al pare
                nodes[i].GetComponent<Node>().province = newProv;
            }
            else
            {
                nodes[i].GetComponent<Node>().province = newProv;
            }
        }
    }

    public List<Province> GetProvinces()
    {
        return this.provinces;
    }
}

public class Province
{
    public float medQuality;
    public float revUnity;
    public float intersec;
    public int population;

    List<Faction> factions;
    List<Modifier> activeModifiers;

    public Province(string name)
    {
        activeModifiers = new List<Modifier>();
        medQuality = 1.0f;
        revUnity = 0.0f;
        intersec = 0.0f;
        int numModifiers = Random.Range(0,Modifier.NUMMOD);

        for(int i = 0; i < numModifiers; i++)
        {

            Modifier newMod = Modifier.GetNewRandomModifier();
            activeModifiers.Add(newMod);
            medQuality -= newMod.medQuality;
            revUnity += newMod.revUnity;
            intersec += newMod.intersec;
        }
    }

    
}