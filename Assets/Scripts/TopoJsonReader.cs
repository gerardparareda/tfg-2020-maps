using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class TopoJsonReader : MonoBehaviour
{
    TextAsset testText;
    //string path = "TopoJsonMaps/map2";
    topoJsonWrapper topoInfo;

    public void ParseTopoJSON(string path)
    {
        testText = Resources.Load<TextAsset>(path);
        //Debug.Log(testText.ToString());

        topoInfo = JsonUtility.FromJson<topoJsonWrapper>(testText.ToString());

        ReadGeometryArcs(testText.ToString());

        topoInfo.arcs = ReadArcs(testText.ToString());

        if (topoInfo != null && topoInfo.arcs != null)
        {
            Debug.Log("TopoJSON parsed successfully");
        }
        else
        {
            Debug.Log("Couldn't parse TopoJSON");
        }
    }

    void ReadGeometryArcs(string text)
    {

        //Anar fins a geometies
        string toFind1 = "geometries";
        int posGeometries = text.IndexOf(toFind1) + toFind1.Length; //find where geometries start
        int numGeometries = (new Regex("Polygon", RegexOptions.Compiled | RegexOptions.IgnoreCase)).Matches(text).Count;
        numGeometries += (new Regex("MultiPolygon", RegexOptions.Compiled | RegexOptions.IgnoreCase)).Matches(text).Count;

        if(topoInfo.objects.collection.geometries == null)
        {
            topoInfo.objects.collection.geometries = new List<Geometry>();
        } else
        {
            topoInfo.objects.collection.geometries.Clear();
        }

        int posArcs = 0;
        string toFind2 = "arcs\":[[";
        string toFind3 = "]]";

        for (int g = 0; g < numGeometries; g++)
        {
            //If type == "multipolygon"
            char tmp = text[text.IndexOf(toFind2) + toFind2.Length];
            if (text[text.IndexOf(toFind2) + toFind2.Length].Equals('['))
            {
                posArcs = text.IndexOf(toFind2) + toFind2.Length - 2;
            } else
            {
                if(posArcs == 0)
                {
                    posArcs = text.IndexOf(toFind2, posGeometries + 2) + toFind2.Length - 2;
                } else
                {
                    posArcs = text.IndexOf(toFind2, posGeometries + (posArcs - posGeometries) + 2) + toFind2.Length - 2;
                }

                int posEndArcs = text.IndexOf(toFind3, posArcs) + 2;

                string string2 = text.Substring(posArcs + 1, posEndArcs - posArcs - 2); //Start, Length
                string2 = string2.Replace(" ", "");

                topoInfo.objects.collection.geometries.Add(new Geometry("Polygon", (JsonUtility.FromJson<TopoArcsWrapper>("{\"newGeometryArcs\":" + string2 + "}")).newGeometryArcs));

                /*Regex rx = new Regex("(\\[\\s*\\[)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                MatchCollection matches = rx.Matches(string2);

                string manipulate;
                for (int i = 0; i <= matches.Count - 1; i++)
                {
                    manipulate = string2;
                    string evalOutput = "{\"newGeometryArcs\":[";
                    manipulate = manipulate.Substring(matches[i].Index, matches[i].Length); ;

                    evalOutput = evalOutput.Substring(0, evalOutput.Length - 1);
                    evalOutput += "]}";

                    //tmpList = JsonUtility.FromJson<TopoArcsWrapper>(evalOutput);
                    //newList.Add(tmpList.newArcs);
                }*/
            }
            
        }
        
    }

    List<List<Vector2>> ReadArcs(string text)
    {
        List<List<Vector2>> newList = new List<List<Vector2>>();

        string toFind1 = "[[[";
        string toFind2 = "]]]";
        int posArc = text.IndexOf(toFind1) + toFind1.Length - 2;
        int end = text.IndexOf(toFind2) + 2;

        string string2 = text.Substring(posArc, end - posArc); //Start, Length
        string2 = string2.Replace(" ", "");

        Regex rx = new Regex("(\\[\\s*\\[)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        Regex rx2 = new Regex("(\\[-*\\d+\\.*\\d*,-*\\d+\\.*\\d*\\])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        MatchCollection matches = rx.Matches(string2);

        string manipulate;
        TopoArcsWrapper tmpList;
        for (int i = 0; i <= matches.Count-1; i++)
        {
            manipulate = string2;
            string evalOutput = "{\"newArcs\":[";
            if ( i == matches.Count-1)
            {
                manipulate = string2.Substring(matches[i].Index+1, (string2.Length - matches[i].Index - 2));
                MatchCollection matches2 = rx2.Matches(manipulate);
                foreach (Match match in matches2)
                {
                    string data = manipulate.Substring(match.Index, match.Length);
                    data = data.Replace("[", "{\"x\":");
                    data = data.Replace(",", ", \"y\":");
                    data = data.Replace("]", "},");
                    evalOutput += data;
                }
            } else
            {
                manipulate = string2.Substring(matches[i].Index+1, (matches[i + 1].Index - matches[i].Index - 3));
                MatchCollection matches2 = rx2.Matches(manipulate);
                foreach (Match match in matches2)
                {
                    string data = manipulate.Substring(match.Index, match.Length);
                    data = data.Replace("[", "{\"x\":");
                    data = data.Replace(",", ", \"y\":");
                    data = data.Replace("]", "},");
                    evalOutput += data;
                    
                }
                
            }

            evalOutput = evalOutput.Substring(0, evalOutput.Length - 1);
            evalOutput += "]}";

            tmpList = JsonUtility.FromJson<TopoArcsWrapper>(evalOutput);
            newList.Add(tmpList.newArcs);
        }

        return newList;
    }

    public topoJsonWrapper RetrieveJsonData()
    {
        return topoInfo;
    }

}
