using UnityEngine;
using System.IO;

[ExecuteInEditMode]

public class CriaIcone : MonoBehaviour
{
    public bool criar;
    public RenderTexture ren;
    public Camera bakeCam;
    public string spriteNome;

    // Update is called once per frame
    void Update()
    {
        if(criar)
        {
            criar = false;
            CriarIconeAgora();
        }
    }

    void CriarIconeAgora()
    {
        if (string.IsNullOrEmpty(spriteNome))
        {
            spriteNome = "icone";
        }

        string path = SaveLocal();
        path += spriteNome;

        bakeCam.targetTexture = ren;
        RenderTexture texturaAtual = RenderTexture.active;
        bakeCam.targetTexture.Release();
        RenderTexture.active = bakeCam.targetTexture;
        bakeCam.Render();

        Texture2D imgPng = new Texture2D(bakeCam.targetTexture.width, bakeCam.targetTexture.height, TextureFormat.ARGB32, false);
        imgPng.ReadPixels(new Rect(0, 0, bakeCam.targetTexture.width, bakeCam.targetTexture.height), 0, 0);
        imgPng.Apply();

        RenderTexture.active = texturaAtual;
        byte[] bytesPng = imgPng.EncodeToPNG();

        File.WriteAllBytes(path + ".png", bytesPng);
    }


    string SaveLocal()
    {
        string saveLocal = Application.streamingAssetsPath + "/Icones/";

        if (!Directory.Exists(saveLocal))
        {
            Directory.CreateDirectory(saveLocal);
        }

        return saveLocal;
    }
}
