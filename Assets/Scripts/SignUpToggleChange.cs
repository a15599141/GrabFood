using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUpToggleChange : MonoBehaviour
{
    public GameObject ResBg;
    public GameObject CusBg;
    public Toggle ResToggle;
    public Toggle CusToggle;
    public GameObject ResMask;
    public GameObject CusMask;

    // Start is called before the first frame update
    void Start()
    {
        ResToggle.onValueChanged.AddListener(ToggleEvent);
    }
    private void ToggleEvent(bool isOn)
    {
        if(isOn)
        {
            ResBg.SetActive(true);
            CusBg.SetActive(false);
            ResMask.SetActive(false);
            CusMask.SetActive(true);
        }
        else
        {
            ResBg.SetActive(false);
            CusBg.SetActive(true);
            ResMask.SetActive(true);
            CusMask.SetActive(false);
        }
 
    }
}
