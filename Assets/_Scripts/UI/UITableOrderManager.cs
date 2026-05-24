using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITableOrderManager : Singleton<UITableOrderManager>
{
    public GameObject orderPanel;
    public GameObject firstColumnTable;
    public GameObject orderTable;



    public List<GameObject> orderFirstColumnSlots = new();
    public List<GameObject> orderTables = new();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        foreach(Transform child in firstColumnTable.transform)
        {
            orderFirstColumnSlots.Add(child.gameObject);
        }
   
        foreach(Transform child in orderTable.transform)
        {
            orderTables.Add(child.gameObject);
        }
    }

    protected override void Start()
    {
        UpdateUI();
    }


    public void UpdateUI() 
    {

        List<Order> currentOrders = OrderManager.Instance.receivedOrders.ConvertAll(orderGame => orderGame.order);
        // get 3 minest id orders
        List<Order> recentOrders = currentOrders.Count > 3 ? currentOrders.GetRange(0, 3) : currentOrders;


        for(int i = 0; i < recentOrders.Count; i++)
        {
            Order order = recentOrders[i];
            orderFirstColumnSlots[i].SetActive(true);
            orderFirstColumnSlots[i].GetComponent<TextMeshProUGUI>().text = "Order " + order.orderId.ToString();
            
            orderTables[i].SetActive(true);
            for (int j = 0; j < order.items.Count; j++)
            {
                Item item = order.items[j];
                GameObject itemGO = orderTables[i].transform.Find("ItemOrder" + (j + 1).ToString()).gameObject;
                Image itemIcon = itemGO.transform.Find("Image").gameObject.GetComponent<Image>();
                itemIcon.sprite = MagicManager.Instance.GetIconItem(item);
                itemIcon.enabled = true;
                GameObject checkmark = itemGO.transform.Find("Check").gameObject;
                
                if (Player.Instance.cart.Exists(ig => ig.item.IsSameItem(item)))
                {
                    checkmark.SetActive(true);
                }
                else
                {
                    checkmark.SetActive(false);
                }
            }
            for (int j = order.items.Count; j < ConstGame.MAX_ORDER; j++)
            {
                GameObject itemGO = orderTables[i].transform.Find("ItemOrder" + (j + 1).ToString()).gameObject;
                Image itemIcon = itemGO.transform.Find("Image").gameObject.GetComponent<Image>();
                itemIcon.enabled = false;
                GameObject checkmark = itemGO.transform.Find("Check").gameObject;
                checkmark.SetActive(false);
            }
        }
        for (int i = recentOrders.Count; i < 3; i++)
        {
            orderFirstColumnSlots[i].SetActive(false);
            orderTables[i].SetActive(false);
        }



        foreach (var table in orderTables)
        {
            // Update each table
        }
    }
    





}
