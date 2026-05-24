using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Order
{
    public int orderId;
    //public float remainingTime = 100f; 
    public List<Item> items = new();

    public int OrderId
    {
        get
        {
            return orderId;
        }
    }
    public int TotalValue
    {
        get
        {
            int total = 0;
            foreach (var item in items)
            {
                total += item.Value;
            }
            return total;
        }
    }



    public Order()
    {

    }

    public Order(List<Item> items)
    {
        this.items = items;
    }

    public Order(int orderId, int totalValue)
    {
        this.orderId = orderId;
        //this.remainingTime = remainTime;

        int totalItem = Random.Range(1, ConstGame.MAX_QUANTITY_PER_ORDER + 1);
        int minValue = ConstGame.MIN_ITEM_VALUE;
        int count = 0;
        int attempt = 0;
        while (attempt < 100 && count < totalItem && totalValue >= minValue)
        {
            attempt++;
            int maxValue = Mathf.Max(100, totalValue); //test 
            int itemValue = Random.Range(minValue, maxValue + 1);

            if (itemValue >= totalValue)
            {
                itemValue = totalValue;
            }
            Item item = ItemFactory.CreateValidItem(itemValue);
            totalValue -= item.Value;
            AddItem(item);
            count++;

        }
    }
    //public void UpdateRemainingTime(float deltaTime)
    //{
    //    remainingTime -= deltaTime;
    //    if (remainingTime < 0)
    //    {
    //        remainingTime = 0;
    //        // end Game
    //    }
    //}

    public void AddItem(Item item)
    {
        if (item == null)
        {
            Debug.LogError("Cannot add null item to order");
            return;
        }
        items.Add(item);
    }


    //public bool IsSameOrder(Order order)
    //{
    //    if(order == null) return false;
    //    if(items.Count != order.items.Count)
    //    {
    //        return false;
    //    }
    //    if (order.items.Count == 0) return false;
    //    for (int i = 0; i < items.Count; i++)
    //    {

    //        if (!items[i].IsSameItem(order.items[i]))
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}
    public bool IsSameOrder(Order order)
    {
        if (order == null) return false;
        if (items.Count != order.items.Count) return false;
        if (items.Count == 0) return false;

        // Tạo bản sao danh sách item của order kia
        var otherItems = new List<Item>(order.items);

        foreach (var item in items)
        {
            // Tìm item tương ứng trong otherItems
            int idx = otherItems.FindIndex(x => x.IsSameItem(item));
            if (idx == -1)
                return false; // Không tìm thấy item tương ứng
            otherItems.RemoveAt(idx); // Loại bỏ để tránh trùng lặp
        }
        return true;
    }

}
