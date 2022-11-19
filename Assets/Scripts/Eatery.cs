using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eatery : MonoBehaviour
{
    public Button EateryDetailBtn;
    public Text res_id;
    public Text user_id;
    public Text liscense_num;
    public Text Name;
    public Text Rate;
    public Text Sale;
    public Text Max_Discount;
    public Text Tag;
    public Text Image;

    // Start is called before the first frame update
    void Start()
    {
        EateryDetailBtn.onClick.AddListener(() =>
        {
            Customer.EateryDetailName.text = Name.text;
            Customer.selected_res_id = int.Parse(res_id.text);
            Customer.SpawnCuisines();
            Customer.SpawnEateryDishes();
            Customer.SpawnEateryComments();
            Customer.SpawnEateryProfile();
            Customer.EateryDetail.SetActive(true);

            // pop up voucher for customer to collect
            if(!string.IsNullOrEmpty(Max_Discount.text))
            {
                GameObject VoucherPopUpPrefab = (GameObject)Resources.Load("Prefabs/VoucherPopUp");

                int.TryParse(res_id.text, out int resID);
                VoucherStruct voucher = MySqlManager.GetMaxAvailableVouchersByResID(resID);
                int.TryParse(voucher.voucher_id, out int voucher_id);
                if( voucher_id > 0 && !MySqlManager.isCusAlreadyOwnVoucher(voucher_id)) // has available voucher and customner doesn't own this voucher
                {
                    VoucherPopUpPrefab.transform.Find("Discount").GetComponent<Text>().text = Max_Discount.text;
                    VoucherPopUpPrefab.transform.Find("res_id").GetComponent<Text>().text = res_id.text;
                    Instantiate(VoucherPopUpPrefab, Customer.EateryDetail.transform);
                }

            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
