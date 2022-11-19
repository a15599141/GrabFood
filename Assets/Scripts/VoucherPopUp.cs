using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoucherPopUp : MonoBehaviour
{
    public Button CollectBtn;
    public Text res_id;
    public VoucherStruct voucher;
    // Start is called before the first frame update
    void Start()
    {
        int.TryParse(res_id.text, out int resID);
        voucher = MySqlManager.GetMaxAvailableVouchersByResID(resID);
        int.TryParse(voucher.voucher_id, out int voucher_id);
        CollectBtn.onClick.AddListener(() =>
        {
            if (InsertVoucherList() > 0)
            {
                 Customer.SpawnMyVouchers();
                 TipsManager.SpawnTips("Voucher collected");
            }
            else
            {
                 TipsManager.SpawnTips("Connection error");
            }
            Destroy(gameObject);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int InsertVoucherList()
    {
        int.TryParse(voucher.voucher_id, out int voucher_id);
        double.TryParse(voucher.discount, out double discount);
        int.TryParse(voucher.type, out int type);

        string query = "INSERT INTO `voucher_list` VALUES (NULL,"+ voucher_id + ","+ UserManager.ResOrCusID+","+ res_id.text+", '"+voucher.res_name+"', '"+voucher.name+"',"+ discount+", "+type+", 0, '"+voucher.start_time+"', '"+voucher.end_time+"', '"+voucher.start_date+"', '"+voucher.end_date+"');";
        return MySqlManager.Insert(query);
    }
}
