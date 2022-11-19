using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuisineEditBtn : MonoBehaviour
{
    public Button EditBtn;
    public Text EditBtnText;
    public Button AddNewCuisineBtn;
    // Start is called before the first frame update
    void Start()
    {
        EditBtn.onClick.AddListener(() =>
        {

            Restaurant.CuisineEditor.SetActive(true);
            Restaurant.CuisineEditorAdd.SetActive(false);

            AddNewCuisineBtn.gameObject.SetActive(true);
            EditBtn.interactable = false;
        });
        AddNewCuisineBtn.onClick.AddListener(() =>
        {
            Restaurant.CuisineEditor.SetActive(false);
            Restaurant.CuisineEditorAdd.SetActive(true);

            AddNewCuisineBtn.gameObject.SetActive(false);
            EditBtn.interactable = true;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
