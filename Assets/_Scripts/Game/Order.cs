using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Order 
{
    public int orderId;
    public float remainingTime = 100f; 
    public List<Item> items = new();


    public int OrderId
    {
        get
        {
            return orderId; 
        }
    }
    public float RemainingTime 
    {
        get
        {
            return remainingTime; 
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

    public Order(int orderId, int totalValue, float remainTime)
    {
        this.orderId = orderId;
        this.remainingTime = remainTime;

        int totalItem = Random.Range(1, ConstGame.MAX_QUANTITY_PER_ORDER + 1);
        int minValue = ConstGame.MIN_ITEM_VALUE;
        int count = 0;
        int attempt = 0;
        while (attempt < 100 && count < totalItem && totalValue >= minValue)
        {
            attempt++;
            int maxValue = Mathf.Min(100, totalValue); //test 
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
    public void UpdateRemainingTime(float deltaTime)
    {
        remainingTime -= deltaTime;
        if (remainingTime < 0)
        {
            remainingTime = 0;
            // end Game
        }
    }

    public void AddItem(Item item)
    {
        if(item== null)
        {
            Debug.LogError("Cannot add null item to order");
            return;
        }
        items.Add(item);
    }


    public bool IsSameOrder(Order order)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (!items[i].IsSameItem(order.items[i]))
            {
                return false;
            }
        }
        return true;
    }


}
