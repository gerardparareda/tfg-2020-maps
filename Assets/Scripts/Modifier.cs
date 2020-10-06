using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class Modifier : MonoBehaviour
{
    public readonly static int NUMMOD = 4;

    public int id;
    public string mname;
    public string description;
    public float medQuality;
    public float revUnity;
    public float intersec;

    public Modifier(int id, string mname, string description, float medQuality, float revUnity, float intersec)
    {
        this.id = id;
        this.mname = mname;
        this.description = description;
        this.medQuality = medQuality;
        this.revUnity = revUnity;
        this.intersec = intersec;
    }

    public static Modifier GetNewRandomModifier()
    {
        int randInst = Random.Range(0, NUMMOD);

        switch (randInst)
        {
            case 0:
                return new Modifier(
                    0,
                    "Despedida massiva",
                    "Una empresa ha externalitzat la feina i ha deixat milers de treballadors sense sou.",
                    0.0f,
                    0.75f,
                    0.2f
                    );
            case 1:
                return new Modifier(
                    1,
                    "Baixa de sous",
                    "Els treballadors cada cop reben menys sou i està sent insostenible per les seves vides.",
                    0.0f,
                    0.4f,
                    0.05f
                    );
            case 2:
                return new Modifier(
                    2,
                    "Massificació",
                    "Els turistes estan ocupant aquesta icònica regió i la policia fa fora les veines de casa seva.",
                    0.3f,
                    0.6f,
                    0.4f
                    );
            case 3:
                return new Modifier(
                    3,
                    "Accidents laborals",
                    "Treballadors estant sent assassinats per culpa de la precarietat.",
                    0.0f,
                    0.3f,
                    0.2f
                    );
            case 4:
                return new Modifier(
                    4,
                    "Accidents laborals",
                    "Treballadors estant sent assassinats per culpa de la precarietat.",
                    0.0f,
                    0.3f,
                    0.2f
                    );
            case 5:
                return new Modifier(
                    5,
                    "Pandèmica vírica",
                    "Un virus altament mortal està afectant a les treballadores. L'economia no s'atura.",
                    0.8f,
                    0.5f,
                    0.55f
                    );
            case 6:
                return new Modifier(
                    6,
                    "Fràquing",
                    "El fràquing està causant terratrèmols a aquesta província. Les infraestructures es deterioren, les cases no aguanten.",
                    0.65f,
                    0.2f,
                    0.25f
                    );
            case 7:
                return new Modifier(
                    7,
                    "Rius contaminats",
                    "Els rius estant sent contaminats, no ens hi podem banyar ni beure'n aigua. S'està morint la biodiversitat de la zona.",
                    0.7f,
                    0.15f,
                    0.34f
                    );
            case 8:
                return new Modifier(
                    8,
                    "Plaga d'insectes",
                    "Per culpa del canvi climàtic i les invesions de l'home, ara tenim plagues que no havíem vist mai. Fan malbé l'equilibri del medi.",
                    0.65f,
                    0.1f,
                    0.2f
                    );
        }

        return null;
    }

}