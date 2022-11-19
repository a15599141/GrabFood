using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TipsManager : MonoBehaviour
{
    
    private static GameObject TipPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public static void SpawnTips(string tips)
    {
        //create tips prefab
        TipPrefab = (GameObject)Resources.Load("Prefabs/Tips");
        TipPrefab.GetComponentInChildren<Text>().text = tips;
        Instantiate(TipPrefab);
    }


}
