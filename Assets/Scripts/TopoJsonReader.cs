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

        if(topoInfo.objects.collection.geometries == null)
        {
            topoInfo.objects.collection.geometries = new List<Geometry>();
        } else
        {
            topoInfo.objects.collection.geometries.Clear();
        }

        string toFind2 = "arcs\":[[";
        string toFind3 = "]]";
        string toFind4 = "]]]";

        Regex rx = new Regex("(\\[\\s*\\[)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        Regex rx2 = new Regex("(-*\\d+\\,*)+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        int posArcs = 0;
        int posBracket = text.IndexOf(toFind2, posGeometries) + toFind2.Length;

        for (int g = 0; g < numGeometries; g++)
        {

            if(g == 5)
            {
                //
            }

            //Only happens the first time 
            if (posArcs != 0)
            {
                posBracket = text.IndexOf(toFind2, posArcs + 2) + toFind2.Length;
            }

            //If type == "multipolygon"
            char tmp = text[posBracket];
            string tmpS = text.Substring(posBracket, 5);

            if (text[posBracket].Equals('['))
            {
                if (posArcs == 0)
                {
                    posArcs = text.IndexOf(toFind2, posGeometries) + toFind2.Length - 1;
                }
                else
                {
                    posArcs = text.IndexOf(toFind2, posArcs + 2) + toFind2.Length - 1;
                }

                int posEndArcs = text.IndexOf(toFind4, posArcs) + 2;

                string manipulate = text.Substring(posArcs, posEndArcs - posArcs);

                Debug.Log("g: " + g + " str:" + manipulate);

                MatchCollection matchCollection = rx.Matches(manipulate);

                List<List<int>> tmpList = new List<List<int>>();

                foreach(Match match in matchCollection)
                {
                    List<int> tmpInts = new List<int>();

                    int endMC = manipulate.IndexOf("]]", match.Index);

                    string parseList = manipulate.Substring(match.Index + 1, endMC - match.Index );

                    MatchCollection matchCollection2 = rx2.Matches(parseList);

                    foreach(Match match2 in matchCollection2)
                    {
                        //int endMCArray = parseList.IndexOf("]", match2.Index);
                        string parsedArray = parseList.Substring(match2.Index, match2.Length);

                        tmpList.Add(JsonUtility.FromJson<TopoArcsWrapper>("{\"newGeometryArcs\":[" + parsedArray + "]}").newGeometryArcs);
                    }

                    //tmpList.Add(JsonUtility.FromJson<TopoArcsWrapper>("{\"newGeometryArcs\":[" + parseList + "]}").newGeometryArcs);
                }

                topoInfo.objects.collection.geometries.Add(new Geometry("MultiPolygon", tmpList));

            }
            else
            {
                if(posArcs == 0)
                {
                    posArcs = text.IndexOf(toFind2, posGeometries + 2) + toFind2.Length - 2;
                } else
                {
                    posArcs = text.IndexOf(toFind2, posArcs + 2) + toFind2.Length - 2;
                }

                int posEndArcs = text.IndexOf(toFind3, posArcs) + 2;

                string string2 = text.Substring(posArcs + 1, posEndArcs - posArcs - 2); //Start, Length
                string2 = string2.Replace(" ", "");

                Debug.Log("g: " + g + " str:" + string2);

                MatchCollection matchCollection = rx2.Matches(string2);

                List<List<int>> tmpList = new List<List<int>>();

                MatchCollection matchCollection2 = rx2.Matches(string2);

                foreach (Match match2 in matchCollection2)
                {
                    //int endMCArray = parseList.IndexOf("]", match2.Index);
                    string parsedArray = string2.Substring(match2.Index, match2.Length);

                    tmpList.Add(JsonUtility.FromJson<TopoArcsWrapper>("{\"newGeometryArcs\":[" + parsedArray + "]}").newGeometryArcs);
                }

                topoInfo.objects.collection.geometries.Add(new Geometry("Polygon", tmpList));

                //topoInfo.objects.collection.geometries.Add(new Geometry("Polygon", (JsonUtility.FromJson<TopoArcsWrapper>("{\"newGeometryArcs\":" + string2 + "}")).newGeometryArcs));

            }
            
        }
        
    }

    List<List<Vector2>> ReadArcs(string text)
    {
        List<List<Vector2>> newList = new List<List<Vector2>>();

        string toFindGeom = "geometries";
        int posGeometries = text.IndexOf(toFindGeom) + toFindGeom.Length; //find where geometries start

        string toFind1 = "(\"arcs\":\\s*\\[\\s*\\[\\s*\\[)";
        string toFind2 = "]]]";
        int posArc = text.IndexOf("[[[") + 1;

        MatchCollection arcs = (new Regex(toFind1, RegexOptions.Compiled | RegexOptions.IgnoreCase)).Matches(text);

        if (arcs.Count > 1)
        {
            if(posArc > posGeometries)
            {
                posArc = arcs[arcs.Count - 1].Index;
            }
        }

        int end = text.IndexOf(toFind2, posArc) + 2;        

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
