using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Convert_Texture_HDRP : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera Cam;
    public Texture2D NewT;
    public Shader cShader;
    public Material mat;
    public RenderTexture rTexture;
    public int Size = 1024;
    // public TextureFormat tFormat;
    public string Path = "";
    public string TName = "Packed_Image";
    public string fPath;
    public bool Overwrite = true;
    public GameObject rPlane;



    //public int MNumber = 0;
    public void Reload_Name(bool NoOver)
    {
        //int MNumber = 0;
        int MNumber = 0;
        string MNumbers = "";
        bool Digit = false;
        for (int x = TName.Length - 1; x > 0; x--)
        {
            if (char.IsDigit(TName[x]))
            {
                MNumbers += TName[x];
                Digit = true;
                //Debug.Log("Name: " + TName);
                if (TName[x] != '0')
                {
                    //Debug.Log("Removing");
                    TName = TName.Remove(x, 1);
                }
            }
            else
            {
                if(TName[x] != '_')
                {
                    Digit = true;
                    
                    TName += "_01";
                    //MNumbers = "01";
                }
                break;
            }
        }
        //Debug.Log(MNumbers);
        if (Digit)
        {
            char[] invert = MNumbers.ToCharArray();
            System.Array.Reverse(invert);
            MNumbers = new string(invert);
            MNumber = int.Parse(MNumbers.ToString()) + 1;
            for (bool x = false; x != true;){
                string FakeName = TName + MNumber.ToString();
                //Debug.Log("FakeName");
                if (File.Exists(Path + "/" + FakeName + ".png"))
                {
                    MNumber++;
                }
                else
                {
                    break;
                }
            }
            TName += MNumber.ToString();

        }
        //fPath = Application.dataPath + Path + "/" + TName + ".png";
        PathGen();
    }

    public void rPath()
    {
        Path =  Application.dataPath + "/Channel Mixer/Exported";
    }
    public void Generate()
    {
        if(Path == "")
        {
            rPath();
        }
        if (Cam == null)
        {
            GameObject cParent = this.gameObject;
            cParent.name = "Channel_Mixer";
            Cam = cParent.AddComponent<Camera>();
            Cam.orthographic = true;
            Cam.nearClipPlane = 0.005f;
        }
        if (rPlane == null)
        {
            rPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            rPlane.transform.SetParent(Cam.transform, true);
        }
        rPlane.transform.eulerAngles = new Vector3(90, 0, 180);
        rPlane.transform.position = new Vector3(0, 0, 1);
        if (mat == null)
        {
            cShader = Shader.Find("VChannel Mixer/Converter");
            mat = new Material(cShader);
        }
        rPlane.GetComponent<MeshRenderer>().material = mat;
        mat = rPlane.GetComponent<MeshRenderer>().sharedMaterial;
        if (rTexture == null)
        {
            ReCreateR();
        }
        
        
        if (NewT == null)
        {
            NewT = new Texture2D(Size, Size, TextureFormat.RGBA32, false);
        }
    }
    IEnumerator DecodeScreen()
    {
        yield return new WaitUntil(() => rTexture.IsCreated());
        yield return new WaitForSecondsRealtime(0.5f);
        //Debug.Log("Done Creating");
    }
 
    public void ReCreateR()
    {
        rTexture = new RenderTexture(Size, Size, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        rTexture.autoGenerateMips = false;
        rTexture.name = "_eTexture";
        rTexture.Create();
        Cam.targetTexture = rTexture;
        

    }
    public void CheckSize()
    {
        //Debug.Log("Check Size");
        if(rTexture.height != Size && rTexture.width != Size)
        {
            rTexture.Release();
            ReCreateR();
            
        }
        if (NewT.height != Size && NewT.width != Size)
        {
            NewT = new Texture2D(Size, Size, TextureFormat.RGBA32, false);
            //NewT = Texture2D.whiteTexture;
        }
    }

    public void PathGen()
    {
        fPath =  Path + "/" + TName + ".png";
    }
    public void Convert()
    {

        Generate();
        CheckSize();
        PathGen();
        RendTexture();
        //

    }
    public void Export()
    {
        Convert();
        UploadPNG();
    }

    void UploadPNG()
    {
        //yield return new WaitUntil()
        //Texture2D tex = NewT;
        //tex.ReadPixels(new Rect(0, 0, Size, Size), 0, 0);
        //tex.Apply();
        byte[] bytes = NewT.EncodeToPNG();
        //Object.DestroyImmediate(tex);

        if (File.Exists(fPath) && !Overwrite)
        {
            Reload_Name(false);
        }
        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);
        }
        File.WriteAllBytes(fPath, bytes);
        Debug.Log("Done exporting");
        UnityEditor.AssetDatabase.Refresh();
    }


    //public Texture2D RendTexture()
    void RendTexture()
    {
        RenderTexture.active = rTexture;
       // Debug.Log("isreadable"+ rTexture.height + "|" + rTexture.width  + " ### SizeTex: " + NewT.height + "|" + NewT.width);
        Cam.Render();
        //Texture2D ConverterT = new Texture2D(Size, Size, TextureFormat.RGBA32, false);
        NewT.ReadPixels(new Rect(0, 0, rTexture.width, rTexture.height), 0, 0, false);
        NewT.Apply();
        //Debug.Log("Appliying");
        //return NewT;
    }
  

}
