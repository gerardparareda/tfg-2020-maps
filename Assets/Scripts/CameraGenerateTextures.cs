using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraGenerateTextures : MonoBehaviour
{
    public Camera renderableCamera;
    public GameObject nodes;
    public RawImage rawImage;
    public Shader shader;
    private Material mat;
    RenderTexture oldTex, newTex, temp;

    void OnEnable()
    {
        mat = new Material(shader);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (renderableCamera.targetTexture != null)
        {
            renderableCamera.targetTexture.Release();
        }

        renderableCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        rawImage.texture = renderableCamera.targetTexture;

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < nodes.transform.childCount; i++)
        {
            nodes.transform.GetChild(i).gameObject.layer = 8;
            renderableCamera.Render();
            nodes.transform.GetChild(i).gameObject.layer = 1;
        }

    }

    private void OnPreRender()
    {
        oldTex = renderableCamera.targetTexture;
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        mat.SetTexture("_MaskTex", renderableCamera.targetTexture);
        temp = RenderTexture.GetTemporary(source.width, source.height, 24);
        Graphics.Blit(renderableCamera.targetTexture, temp, mat);
        Graphics.Blit(temp, oldTex);
        rawImage.texture = temp;
        RenderTexture.ReleaseTemporary(temp);
    }

    private void OnPostRender()
    {
        
    }
}
