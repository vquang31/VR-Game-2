using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OrderGame
{
    public Order order;
    public GameObject gameObject;
    public GameObject iconTimelineGameObject;
}



public class OrderManager : Singleton<OrderManager>
{

    public int orderCounter = 0;
    public List<OrderGame> receivedOrders = new();




    public GameObject orderPrefab;
    public Transform orderSpawnPoint;






    public float spawnTime = 40f;
    public bool isSpawning = true;
    private Coroutine spawnCoroutine;


    public void Init()
    {
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float elapsedTime = 0f;
            while (elapsedTime < spawnTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            if (isSpawning)
            {
                CreateOrder(Random.Range(100, 101));
            }
        }
    }

    void Update()
    {
        UpdateRemainingTime();
        OrderTimeline.Instance.UpdateUI();
    }

    private void UpdateRemainingTime() {
        foreach (var order in receivedOrders)
        {
            order.order.UpdateRemainingTime(Time.deltaTime);
        }
    }
    public void CreateOrder(int totalValue)
    {
        Order newOrder = new(orderCounter, totalValue, ConstGame.REMAINING_TIME_ORDER);
        orderCounter++;
        // update UI and other logic: createObject for order, add Icon to timeline, etc.

        // createObject for order
        GameObject orderGO = ObjectPoolManager.Instance.Spawn(orderPrefab);
        orderGO.transform.position = orderSpawnPoint.position;
        // Set Information of order to orderGO
        SetOrderInformation(orderGO, newOrder);

        //
         


        // add Icon to timeline
        GameObject orderIcon = OrderTimeline.Instance.AddOrderIconToTimeline();

        OrderGame newOrderGame = new()
        {
            order = newOrder,
            gameObject = orderGO,
            iconTimelineGameObject = orderIcon
        };

        ReceiveOrder(newOrderGame);

        OrderTimeline.Instance.UpdateUI();
    }



    private void SetOrderInformation(GameObject gameObject, Order order)
    {
        // Set order information to the gameObject
        OrderUI orderUI = gameObject.GetComponent<OrderUI>();
        if (orderUI != null)
        {
            orderUI.SetOrderInformation(order);
        }
    }
    public void DeliveryOrder(Order order)
    {
        List<Order> matchedOrders = GetMatchedOrders(order);
        Order bestOrder = GetBestMatchedOrder(matchedOrders);
        if (bestOrder != null)
        {
            Debug.Log($"Delivered order: {bestOrder}");
            OrderGame orderGame = receivedOrders.Find(o => o.order.OrderId == bestOrder.OrderId);
            receivedOrders.Remove(orderGame);
            // Add score or other logic for successful delivery
            /////
            ///
            /// 
            ///

        }
        //
    }

    private Order GetBestMatchedOrder(List<Order> matchedOrders)
    {
        Order bestOrder = null;
        //float minRemainingTime = float.MaxValue;
        int minOrderId = int.MaxValue;
        foreach (var matchedOrder in matchedOrders)
        {
            if (matchedOrder.OrderId < minOrderId)
            {
                bestOrder = matchedOrder;
                minOrderId = matchedOrder.OrderId;
            }
        }
        return bestOrder;
    }


    private void ReceiveOrder(OrderGame orderGame)
    {
        receivedOrders.Add(orderGame);
        Debug.Log($"Received order: {orderGame.order}");
    }

    private List<Order> GetMatchedOrders(Order order)
    {
        List<Order> matchedOrders = new();
        foreach (var receivedOrder in receivedOrders)
        {
            if (receivedOrder.order.IsSameOrder(order))
            {
                matchedOrders.Add(receivedOrder.order);
            }
        }
        return matchedOrders;
    }


}
