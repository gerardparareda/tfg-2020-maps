using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TopoArcsWrapper
{
    public List<Vector2> newArcs;
    public List<int> newGeometryArcs;
    public List<List<int>> newGeometryArcsLayered;
}