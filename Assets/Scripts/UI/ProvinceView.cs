using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProvinceView : MonoBehaviour
{

    public static ProvinceView _instance;
    public Text ProvinceNameText;
    public Text MedQualityText;
    public Text RevUnityText;
    public Text IntersecText;

    private void Awake()
    {
        if (_instance == null)
        {

            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetActive(string name, float med, float rev, float inter)
    {
        gameObject.SetActive(true);
        ProvinceNameText.text = name;
        MedQualityText.text = "Mediambient Quality: " + (med*100.0f) + "%";
        RevUnityText.text = "Revolutionary Unity: " + (rev * 100.0f) + "%";
        IntersecText.text = "Intersectionality: " + (inter * 100.0f) + "%";
    }
}
