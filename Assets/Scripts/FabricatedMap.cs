using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FabricatedMap", menuName = "Maps/FabricatedMapObject", order = 1)]
public class FabricatedMap : ScriptableObject
{
    public string mapName;

    public Texture2D landMap;
    public Texture2D provinceMapLayer1;
    public Texture2D provinceMapLayer2;
    public Texture2D heightMap;
    public Texture2D waterMap;

}
