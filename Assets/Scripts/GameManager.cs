using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool TestMode;
    // Start is called before the first frame update
    void Start()
    {
        if(!TestMode) SpawnLogin();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnLogin()
    {
        GameObject LoginPagePrefab = (GameObject)Resources.Load("Prefabs/LoginPage");
        Instantiate(LoginPagePrefab);
    }
}
