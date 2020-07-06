using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    List<GameObject> nodes;
    GameMap gameMap;

    public MapHandler(List<GameObject> nodes)
    {
        this.nodes = nodes;
        gameMap = new GameMap(nodes.Count);

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

    void GenerateStats()
    {

    }
}

class GameMap
{
    float totalMedQuality;
    float totalRevUnity;
    float totalIntersec;

    List<Province> provinces;

    public GameMap(int size)
    {
        provinces = new List<Province>();
    }
}

class Province
{
    float medQuality;
    float revUnity;
    float intersec;
    int population;

    List<Faction> factions;

    Province() { }

}