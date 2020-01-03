using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class TopoJsonReader : MonoBehaviour
{
    TextAsset testText;
    string path = "TopoJsonMaps/map2";
    topoJsonWrapper topoInfo;

    // Start is called before the first frame update
    void Start()
    {
        testText = Resources.Load<TextAsset>(path);
        //Debug.Log(testText.ToString());

        topoInfo = JsonUtility.FromJson<topoJsonWrapper>(testText.ToString());

        topoInfo.arcs = ReadArcs(testText.ToString());

        if (topoInfo != null && topoInfo.arcs != null)
        {
            Debug.Log("TopoJSON parsed successfully");
        } else
        {
            Debug.Log("Couldn't parse TopoJSON");
        }
    }

    List<List<Vector2>> ReadArcs(string text)
    {
        List<List<Vector2>> newList = new List<List<Vector2>>();

        string toFind1 = "\"arcs\":";
        string toFind2 = "],\"transform\"";
        int firstArc = text.IndexOf(toFind1) + toFind1.Length;
        int secondArc = text.IndexOf(toFind1, firstArc) + toFind1.Length;
        int end = text.IndexOf(toFind2, secondArc);
        string string2 = text.Substring(secondArc + 1, end - secondArc - 1);
        string2 = string2.Replace(" ", "");

        Regex rx = new Regex("(\\[\\s*\\[)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        Regex rx2 = new Regex("(\\[-*\\d+\\.*\\d+,-*\\d+\\.*\\d+\\])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
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



}
