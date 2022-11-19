using UnityEngine;
using UnityEngine.UI;
 
public class ChangeContentType : MonoBehaviour
{
    public InputField inputField;
    public Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        toggle.onValueChanged.AddListener(ToggleEvent);
    }
    private void ToggleEvent(bool isOn)
    {
        inputField.contentType = isOn ? InputField.ContentType.Standard : InputField.ContentType.Password;
 
        //��������޸�inputField.contentType���������⣬��Ҫ�ĵ���������ſ�����ʾ
        inputField.ForceLabelUpdate();
 
    }
 
}