using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProvinceTooltip : MonoBehaviour
{

    public static ProvinceTooltip _instance;

    public string provinceName;

    private Text tooltipText;
    private RectTransform backgroundRectTransform;
    private Vector3 offset;

    public void Start()
    {
        offset = new Vector3(2.5f, 2.5f, 0.0f);
    }
    private void Awake()
    {
        if (_instance == null)
        {

            _instance = this;
            DontDestroyOnLoad(this.gameObject);

            //Rest of your Awake code
            //backgroundRectTransform = Transform.Find("Background").gameObject.GetComponent<RectTransform>();
            tooltipText = transform.GetChild(1).GetComponent<Text>();
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(this);
        }
    }

    public void Update()
    {
        transform.position = Input.mousePosition + offset;
    }

    public void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);

        tooltipText.text = tooltipString;
        //Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth, tooltipText.preferredHeight);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
