using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNumberToggle : MonoBehaviour
{
    public Text Label;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(DateSelectorUpdateDay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DateSelectorUpdateDay(bool isOn)
    {
        if(isOn)
        {
            int.TryParse(Label.text, out int label);
            if (label < 10) VoucherEditor.DateSelectorDay.text = "0" + Label.text;
            else VoucherEditor.DateSelectorDay.text = Label.text;
        }
    }
}
