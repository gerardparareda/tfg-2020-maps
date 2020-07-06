using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Faction", menuName = "GameObjects/Faction")]
public class Faction : ScriptableObject
{

    public string Name;
    public string Acronym;
    public Texture image;

}
