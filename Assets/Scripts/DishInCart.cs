using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DishInCart : MonoBehaviour
{
    public Text Dish_ID;
    public Button AddDishAmountBtn, MinusDishAmountBtn;
    public Button RemoveDishBtn;
    public Text Amount, Stock, Price, Sale; 
    // Start is called before the first frame update
    void Start()
    {
        int.TryParse(Dish_ID.text, out int dish_id);
        AddDishAmountBtn.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(Customer.CurrentPlacedOrderID.text)) // check wheather the current placed order is paid
            {
                int result = MySqlManager.DishIsAvaliable(dish_id);
                if (result == 0)
                {
                    TipsManager.SpawnTips("Connection error");
                }
                else if (result == 1)
                {
                    TipsManager.SpawnTips("Dish sold out");
                    Stock.text = "0";
                }
                else if (result == 2)
                {
                    TipsManager.SpawnTips("Dish off shelved");
                }
                else
                {
                    float price = MySqlManager.GetDishPriceByDishID(dish_id);

                    float.TryParse(Price.text, out float dish_total_price);
                    dish_total_price = dish_total_price + price;
                    Price.text = dish_total_price.ToString();

                    float.TryParse(Customer.ShoppingCartTotalPrice.text, out float cart_total_price);
                    cart_total_price = cart_total_price + price;
                    Customer.ShoppingCartTotalPrice.text = cart_total_price.ToString();

                    int.TryParse(Customer.ShoppingCartTotalAmount.text, out int cart_total_amount);
                    cart_total_amount = cart_total_amount + 1;
                    Customer.ShoppingCartTotalAmount.text = cart_total_amount.ToString();

                    int.TryParse(Stock.text, out int stock);
                    stock = stock - 1;
                    Stock.text = stock.ToString();
                    if (stock == 0) AddDishAmountBtn.interactable = false;

                    int.TryParse(Amount.text, out int amount);
                    amount = amount + 1;
                    Amount.text = amount.ToString();
                    MinusDishAmountBtn.interactable = true;

                    Customer.OrderDishAmountsList[Customer.OrderDishIDsList.IndexOf(dish_id)] = Customer.OrderDishAmountsList[Customer.OrderDishIDsList.IndexOf(dish_id)] + 1;
                }
            }
            else TipsManager.SpawnTips("You have an unpaid order");

        });

        MinusDishAmountBtn.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(Customer.CurrentPlacedOrderID.text))
            {
                float price = MySqlManager.GetDishPriceByDishID(dish_id);

                float.TryParse(Price.text, out float dish_total_price);
                dish_total_price = dish_total_price - price;
                Price.text = dish_total_price.ToString();

                float.TryParse(Customer.ShoppingCartTotalPrice.text, out float cart_total_price);
                cart_total_price = cart_total_price - price;
                Customer.ShoppingCartTotalPrice.text = cart_total_price.ToString();

                int.TryParse(Customer.ShoppingCartTotalAmount.text, out int cart_total_amount);
                cart_total_amount = cart_total_amount - 1;
                Customer.ShoppingCartTotalAmount.text = cart_total_amount.ToString();

                int.TryParse(Stock.text, out int stock);
                stock = stock + 1;
                Stock.text = stock.ToString();
                AddDishAmountBtn.interactable = true;

                int.TryParse(Amount.text, out int amount);
                amount = amount - 1;
                Amount.text = amount.ToString();

                if (amount == 0)
                {
                    Customer.OrderDishAmountsList.Remove(Customer.OrderDishAmountsList[Customer.OrderDishIDsList.IndexOf(dish_id)]);
                    Customer.OrderDishIDsList.Remove(dish_id);

                    MinusDishAmountBtn.interactable = false;
                    Destroy(gameObject);
                }
                else Customer.OrderDishAmountsList[Customer.OrderDishIDsList.IndexOf(dish_id)] = Customer.OrderDishAmountsList[Customer.OrderDishIDsList.IndexOf(dish_id)] - 1;
            }
            else TipsManager.SpawnTips("You have an unpaid order");
        });

        RemoveDishBtn.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(Customer.CurrentPlacedOrderID.text))
            {
                float price = MySqlManager.GetDishPriceByDishID(dish_id);

                int.TryParse(Amount.text, out int amount);
                float.TryParse(Price.text, out float dish_total_price);

                float.TryParse(Customer.ShoppingCartTotalPrice.text, out float cart_total_price);
                cart_total_price = cart_total_price - dish_total_price;
                Customer.ShoppingCartTotalPrice.text = cart_total_price.ToString();

                int.TryParse(Customer.ShoppingCartTotalAmount.text, out int cart_total_amount);
                cart_total_amount = cart_total_amount - amount;
                Customer.ShoppingCartTotalAmount.text = cart_total_amount.ToString();

                Customer.OrderDishAmountsList.Remove(Customer.OrderDishAmountsList[Customer.OrderDishIDsList.IndexOf(dish_id)]);
                Customer.OrderDishIDsList.Remove(dish_id);


                Destroy(gameObject);
            }
            else TipsManager.SpawnTips("You have an unpaid order");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
