using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Modifier", menuName = "GameObjects/Modifier")]
public class Modifier : ScriptableObject
{
    public string Name;
    [TextArea]
    public string Description;
    public Texture image;

    [Header("Modifiers")]
    [Range(0.0f, -100.0f)]
    public float medQuality;

    [Range(0.0f, 100.0f)]
    public float revUnity;

    [Range(0.0f, 100.0f)]
    public float intersec;
}