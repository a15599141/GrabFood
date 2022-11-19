using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DishDetail : MonoBehaviour
{
    public Button AddBtn;
    public Button BackBtn;
    public Text Dish_ID;
    public Text Stock;
    public Text State;
    // Start is called before the first frame update
    void Start()
    {
        if (State.text.Equals("0"))
        {
            AddBtn.interactable = false;
            AddBtn.transform.Find("Text").GetComponent<Text>().text = "Dish off shelved";
        }
        if (Stock.text.Equals("0"))
        {
            AddBtn.interactable = false;
            AddBtn.transform.Find("Text").GetComponent<Text>().text = "Dish sold out";
        }

        AddBtn.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(Customer.CurrentPlacedOrderID.text))
            {
                int.TryParse(Dish_ID.text, out int dish_id);
                int result = MySqlManager.DishIsAvaliable(dish_id);
                if (result == 0)
                {
                    TipsManager.SpawnTips("Connection error");
                }
                else if (result == 1)
                {
                    TipsManager.SpawnTips("Dish sold out");
                    Stock.text = "0";
                    AddBtn.interactable = false;
                    AddBtn.transform.Find("Text").GetComponent<Text>().text = "Dish sold out";
                }
                else if (result == 2)
                {
                    TipsManager.SpawnTips("Dish off shelved");
                    State.text = "0";
                    AddBtn.interactable = false;
                    AddBtn.transform.Find("Text").GetComponent<Text>().text = "Dish off shelved";
                }
                else if (Customer.OrderDishIDsList.Contains(dish_id))
                {
                    TipsManager.SpawnTips("Dish already in cart");
                }
                else
                {
                    Customer.SpawnDishInShoppingCart(dish_id); // add select dish to cart
                    Destroy(gameObject);
                }
            }
            else TipsManager.SpawnTips("You have an unpaid order");
        });

        BackBtn.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
