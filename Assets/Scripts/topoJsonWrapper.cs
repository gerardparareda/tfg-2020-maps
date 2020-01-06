using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class topoJsonWrapper
{
    public string type;
    public ObjectField objects;
    [SerializeField]
    public List<List<Vector2>> arcs;
    public TransformField transform;
    public double[] bbox;

}

[Serializable]
public class TransformField
{
    public float[] scale;
    public int translate;
}

[Serializable]
public class ObjectField
{
    public CollectionField collection;
}

[Serializable]
public class CollectionField
{
    public string type;
    public Geometries[] geometries;
}

[Serializable]
public class Geometries
{
    public string type;
    public int[] arcs;
}

