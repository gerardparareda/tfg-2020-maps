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
    public float[] translate;
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
    public List<Geometry> geometries;
}

[Serializable]
public class Geometry
{
    public string type;
    public List<List<int>> arcs;
    //public Properties properties;

    public Geometry(string type, List<List<int>> arcs)
    {
        this.type = type;
        this.arcs = arcs;
    }
}
