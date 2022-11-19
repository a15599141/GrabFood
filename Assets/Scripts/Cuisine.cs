using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cuisine : MonoBehaviour
{
    public Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        toggle.onValueChanged.AddListener(SetDishCuisine);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDishCuisine(bool toggleIsOn)
    {
        if (toggleIsOn)
        {
            if (DishEditor.SelectCuisineBtnText != null) DishEditor.SelectCuisineBtnText.text = gameObject.transform.Find("Label").GetComponent<Text>().text;
            else
            {
                string cuisine = gameObject.transform.Find("Label").GetComponent<Text>().text;
                if (cuisine.Equals("All"))
                {
                    if (UserManager.UserType == 1)
                    {
                        Restaurant.CuisineEditInputField.text = cuisine;
                        Restaurant.TipsForCuisineAll.gameObject.SetActive(true);
                        Restaurant.CuisineEditInputField.interactable = false;
                        Restaurant.CuisineDeleteBtn.interactable = false;
                        Restaurant.CuisineUpdateBtn.interactable = false;
                        Restaurant.SpawnDishes();
                    }
                    else Customer.SpawnEateryDishes();
                }
                else
                {
                    Restaurant.selected_cuisine_id = gameObject.transform.Find("tag_id").GetComponent<Text>().text;
                    if (UserManager.UserType == 1)
                    {
                        Restaurant.CuisineEditInputField.text = cuisine;
                        Restaurant.cuisineName_before = cuisine;
                        Restaurant.TipsForCuisineAll.gameObject.SetActive(false);
                        Restaurant.CuisineEditInputField.interactable = true;
                        Restaurant.CuisineDeleteBtn.interactable = true;
                        Restaurant.CuisineUpdateBtn.interactable = true;
                        Restaurant.SpawnDishes(CUISINE: cuisine);
                    }
                    else Customer.SpawnEateryDishes(CUISINE: cuisine);
                }
            }

        }
    }

}
