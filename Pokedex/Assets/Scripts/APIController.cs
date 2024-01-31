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
    public TextMeshProUGUI p_name, p_number;
    public TextMeshProUGUI[] typesArray;

    readonly string baseURL = " https://pokeapi.co/api/v2/";

    // Start is called before the first frame update
    void Start()
    {
        imageSprite.texture = Texture2D.blackTexture;
        p_name.text = "";
        p_number.text = "";

        foreach(TextMeshProUGUI typesArrayText in typesArray)
        {
            typesArrayText.text = "";
        }
    }

    public void RandomCardPicker()
    {
        int randomIndex = Random.Range(1, 808);

        imageSprite.texture = Texture2D.blackTexture;
        p_name.text = "Loading...";
        p_number.text = "#" + randomIndex;

        foreach(TextMeshProUGUI typesArrayText in typesArray)
        {
            typesArrayText.text = "";
        }
        StartCoroutine(GetIndexCard(randomIndex));

    }

    IEnumerator GetIndexCard(int i)
    {
        string mainURL = baseURL + "pokemon/" + i.ToString();

        UnityWebRequest req = UnityWebRequest.Get(mainURL);
        yield return req.SendWebRequest();

        if(req.isNetworkError || req.isHttpError)
        {
            Debug.LogError(req.error);
            yield break;
        }

        JSONNode info = JSON.Parse(req.downloadHandler.text);

        string nameP = info["name"];
        string spriteURL = info["sprites"]["front_default"];

        JSONNode p_Types = info["types"];
        string[] p_TypesName = new string[p_Types.Count];

        for(int k=0, j = p_Types.Count - 1; k< p_Types.Count; k++, j--)
        {
            p_TypesName[j] = p_Types[k]["type"]["name"];
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
        imageSprite.texture = DownloadHandlerTexture.GetContent(spriteReq);
        imageSprite.texture.filterMode = FilterMode.Point;

        p_name.text = FirstLetter(nameP);

        for(int k =0; k< p_TypesName.Length; k++)
        {
            typesArray[k].text = FirstLetter(p_TypesName[k]);

        }
    }

    string FirstLetter(string s)
    {
        return char.ToUpper(s[0]) + s.Substring(1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
