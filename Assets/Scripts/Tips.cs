using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tips : MonoBehaviour
{
    public float DestroyTime = 1.5f; // destory after time
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
