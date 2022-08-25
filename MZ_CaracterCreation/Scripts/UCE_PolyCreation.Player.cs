using UnityEngine;
using System.Collections;
using Mirror;
public partial class Player
{
    [Header("---PolyCreation setting------")]
    [SyncVar]
    public string gender = "male";
    [SyncVar]
    public int hair = 0;
    [SyncVar]
    public int face = 0;
    [SyncVar]
    public string HairColor = "white";
    [SyncVar]
    public string SkinColor = "white";
    [Header("Material")]
    public Material mat;
    public GameObject[] genderObject;
    public GameObject[] hairManObject;
    public GameObject[] hairFemaleObject;
    public GameObject[] faceManObject;
    public GameObject[] faceFemaleObject;
    public string description;
    [HideInInspector]public bool IsHelmet;



    [Command]
    void Cmd_BasedHumanStyle()
    {
        //Database.singleton.LoadPoly(this);
        //Debug.Log("poly load in databse ready");
    }

    public void PolyRefresh()
    {
         switch (SkinColor)
         {
             case "white":
                 mat.SetColor("_Color_Skin", new Color(1f, 0.8000001f, 0.682353f));
                 mat.SetColor("_Color_Stubble", new Color(0.8039216f, 0.7019608f, 0.6313726f));
                 mat.SetColor("_Color_Scar", new Color(0.9245283f, 0.4440126f, 0.1613563f));
                 break;
             case "brown":
                 mat.SetColor("_Color_Skin", new Color(0.8196079f, 0.6352941f, 0.4588236f));
                 mat.SetColor("_Color_Stubble", new Color(0.6588235f, 0.572549f, 0.4627451f));
                 mat.SetColor("_Color_Scar", new Color(0.9245283f, 0.4440126f, 0.1613563f));
                 break;
             case "black":
                 mat.SetColor("_Color_Skin", new Color(0.5647059f, 0.4078432f, 0.3137255f));
                 mat.SetColor("_Color_Stubble", new Color(0.3882353f, 0.2901961f, 0.2470588f));
                 mat.SetColor("_Color_Scar", new Color(0.9245283f, 0.4440126f, 0.1613563f));
                 break;
             case "cry":
                 mat.SetColor("_Color_Skin", new Color(0.6415094f, 0.4145603f, 0.4335407f));
                 mat.SetColor("_Color_Stubble", new Color(0.735849f, 0.5171769f, 0.536028f));
                 mat.SetColor("_Color_Scar", new Color(0.9245283f, 0.4440126f, 0.1613563f));
                 break;
             case "elf":
                 mat.SetColor("_Color_Skin", new Color(0.9607844f, 0.7843138f, 0.7294118f));
                 mat.SetColor("_Color_Stubble", new Color(0.8627452f, 0.7294118f, 0.6862745f));
                 mat.SetColor("_Color_Scar", new Color(0.9245283f, 0.4440126f, 0.1613563f));
                 break;
             default:
                 mat.SetColor("_Color_Skin", new Color(1f, 0.8000001f, 0.682353f));
                 mat.SetColor("_Color_Stubble", new Color(0.8039216f, 0.7019608f, 0.6313726f));
                 mat.SetColor("_Color_Scar", new Color(0.9245283f, 0.4440126f, 0.1613563f));
                 break;
         }
         mat.SetColor("_Color_Hair", (Color)typeof(Color).GetProperty(HairColor.ToLowerInvariant()).GetValue(null, null));
        if (this.gender == "male")
        {
            this.genderObject[1].SetActive(false);
            this.genderObject[0].SetActive(true);
            if (this.hairManObject != null && this.faceManObject != null)
            {
                for (int i = 0; i < this.hairManObject.Length; i++)
                {
                    if (i == this.hair)
                    {
                        this.hairManObject[i].SetActive(true);
                    }
                    else
                    {
                        this.hairManObject[i].SetActive(false);

                    }
                }

                for (int i = 0; i < this.faceManObject.Length; i++)
                {
                    if (i == face)
                    {
                        // Debug.Log("you use helmet already");
                        this.faceManObject[i].SetActive(true);

                    }
                    else
                    {
                        this.faceManObject[i].SetActive(false);

                    }
                }
            }
        }
        else
        {
            this.genderObject[0].SetActive(false);
            this.genderObject[1].SetActive(true);
            if (this.hairFemaleObject != null && this.faceFemaleObject != null)
            {

                for (int i = 0; i < this.hairFemaleObject.Length; i++)
                {
                    if (i == hair)
                    {

                        this.hairFemaleObject[i].SetActive(true);

                    }
                    else
                    {
                        this.hairFemaleObject[i].SetActive(false);

                    }
                }

                for (int i = 0; i < faceFemaleObject.Length; i++)
                {
                    if (i == face)
                    {
                        this.faceFemaleObject[i].SetActive(true);

                    }
                    else
                    {
                        this.faceFemaleObject[i].SetActive(false);

                    }
                }
            }
        }
    }
}
