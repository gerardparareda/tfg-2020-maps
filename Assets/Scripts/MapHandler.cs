using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    List<GameObject> nodes;
    GameMap gameMap;
    public List<Modifier> modifiers;

    public void InitializeMap(List<GameObject> nodes)
    {
        this.nodes = nodes;
        gameMap = new GameMap(nodes, modifiers);

        FixProvinces();
    }

    void FixProvinces()
    {
        for(int i = 0; i < nodes.Count-1; i++)
        {
            if(nodes[i].name == nodes[i + 1].name)
            {
                GameObject tmpGameObject = new GameObject();
                tmpGameObject.name = nodes[i].name;
                tmpGameObject.transform.parent = nodes[i].transform.parent;
                nodes[i].transform.parent = tmpGameObject.transform;
                nodes[i+1].transform.parent = tmpGameObject.transform;
                nodes.RemoveAt(i + 1);
                nodes.RemoveAt(i);
                nodes.Add(tmpGameObject);
                i -= 1; //Perquè n'hem eliminat dos
            }
        }
    }

}

class GameMap
{
    float totalMedQuality;
    float totalRevUnity;
    float totalIntersec;

    List<Province> provinces;

    public GameMap(List<GameObject> nodes, List<Modifier> modifiers)
    {

        totalMedQuality = 100.0f;
        totalRevUnity = 0.0f;
        totalIntersec = 0.0f;

        provinces = new List<Province>();
        for(int i = 0; i < nodes.Count; i++)
        {
            provinces.Add(new Province(nodes[i].name, modifiers));
        }
    }
}

class Province
{
    float medQuality;
    float revUnity;
    float intersec;
    int population;

    List<Faction> factions;
    List<Modifier> activeModifiers;

    public Province(string name, List<Modifier> modifiers)
    {
        activeModifiers = new List<Modifier>();
        medQuality = 100.0f;
        revUnity = 0.0f;
        intersec = 0.0f;
        int numModifiers = Random.Range(0, modifiers.Count);
        //Debug.Log(name);

        for(int i = 0; i < numModifiers; i++)
        {
            int index = Random.Range(0, modifiers.Count);
           //Debug.Log(modifiers[index].name);
            Modifier newMod = ScriptableObject.Instantiate(modifiers[index]);
            activeModifiers.Add(newMod);
            medQuality -= newMod.medQuality;
            revUnity += newMod.revUnity;
            intersec += newMod.intersec;
        }
    }
}