using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoucherCode : MonoBehaviour
{
    public Button OkBtn;
    // Start is called before the first frame update
    void Start()
    {
        OkBtn.onClick.AddListener(() =>
         {
             Destroy(gameObject);
         });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
