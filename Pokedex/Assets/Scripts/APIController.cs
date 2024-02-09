using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;

public class APIController : MonoBehaviour
{
    public RawImage imageSprite;
    public TextMeshProUGUI p_name, p_height;
    public TextMeshProUGUI[] typesArray;
    public string[] p_characters;
    string characterName;
    readonly string baseURL = "https://pokeapi.co/api/v2/";

    // Start is called before the first frame update
    void Start()
    {
        
        imageSprite.texture = Texture2D.blackTexture;
        p_name.text = "";
        p_height.text = "";

        foreach(TextMeshProUGUI typesArrayText in typesArray)
        {
            typesArrayText.text = "";
        }
    }

    public void RandomCardPicker()
    {
        int randomIndex = Random.Range(0, 4);

        imageSprite.texture = Texture2D.blackTexture;
        p_name.text = "Loading...";
        p_height.text = "Loading...";

        foreach(TextMeshProUGUI typesArrayText in typesArray)
        {
            typesArrayText.text = "";
        }
        characterName = p_characters[randomIndex];
        StartCoroutine(GetCardDetails(characterName));

    }

    IEnumerator GetCardDetails(string i)
    {
       
        string mainURL = baseURL + "pokemon/" + i;

        UnityWebRequest req = UnityWebRequest.Get(mainURL);
        yield return req.SendWebRequest();

        if(req.isNetworkError || req.isHttpError)
        {
            Debug.LogError(req.error);
            yield break;
        }

        JSONNode info = JSON.Parse(req.downloadHandler.text);

        //Fetching details of character
        string nameP = info["name"];
        string heightP = info["height"];
        string spriteURL = info["sprites"]["front_default"];

        JSONNode p_Types = info["abilities"];
        string[] p_TypesName = new string[p_Types.Count];

        for (int k = 0, j = p_Types.Count - 1; k < p_Types.Count; k++, j--)
        {
            p_TypesName[j] = p_Types[k]["ability"]["name"];
            Debug.Log("p_TypesName[j] - " + p_TypesName[j]);
        }

        //Sprite
        UnityWebRequest spriteReq = UnityWebRequestTexture.GetTexture(spriteURL);
        yield return spriteReq.SendWebRequest();

        if(spriteReq.isNetworkError || spriteReq.isHttpError)
        {
            Debug.LogError(spriteReq.error);
            yield break;
        }

        //UI
        Texture2D t = DownloadHandlerTexture.GetContent(spriteReq);
        imageSprite.texture = t;
        

        p_name.text = nameP;
        p_height.text = heightP;

        for (int k = 0; k < p_TypesName.Length; k++)
        {
            typesArray[k].text = p_TypesName[k];

        }
    }

   public void MannualCharacter(string cName)
   {
        StartCoroutine(GetCardDetails(cName));
   }
}
