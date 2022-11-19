using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour
{
    public float lastDuration;
    public float repeat;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Text>().canvasRenderer.SetAlpha(Mathf.PingPong(Time.time / repeat, lastDuration));
    }
}
