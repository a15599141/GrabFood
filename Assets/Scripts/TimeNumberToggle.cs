using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeNumberToggle : MonoBehaviour
{
    public Text TimeUnit;
    public Text Label;
    // Start is called before the first frame update
    void Start()
    {
        if (TimeUnit.text.Equals("h")) GetComponent<Toggle>().onValueChanged.AddListener(TimeSelectorUpdateHour);
        if (TimeUnit.text.Equals("m")) GetComponent<Toggle>().onValueChanged.AddListener(TimeSelectorUpdateMin);
        if (TimeUnit.text.Equals("s")) GetComponent<Toggle>().onValueChanged.AddListener(TimeSelectorUpdateSec);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TimeSelectorUpdateHour(bool isToggleOn)
    {
        if (isToggleOn)
        {
            int.TryParse(Label.text, out int label);
            if(label<10) VoucherEditor.TimeSelectorHour.text = "0" + Label.text;
            else VoucherEditor.TimeSelectorHour.text = Label.text;
        }
    }
    public void TimeSelectorUpdateMin(bool isToggleOn)
    {
        if (isToggleOn)
        {
            int.TryParse(Label.text, out int label);
            if (label < 10) VoucherEditor.TimeSelectorMin.text = "0" + Label.text;
            else VoucherEditor.TimeSelectorMin.text = Label.text;
        }
    }
    public void TimeSelectorUpdateSec(bool isToggleOn)
    {
        if (isToggleOn)
        {
            int.TryParse(Label.text, out int label);
            if (label < 10) VoucherEditor.TimeSelectorSec.text = "0" + Label.text;
            else VoucherEditor.TimeSelectorSec.text = Label.text;
        }
    }
}
