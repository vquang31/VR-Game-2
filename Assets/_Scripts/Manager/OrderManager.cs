using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[System.Serializable]
public class OrderGame
{
    public Order order;
    public GameObject gameObject;
}



public class OrderManager : Singleton<OrderManager>
{

    private int orderCounter = 0;

    public void SetOrderCounter(int value)
    {
        orderCounter = value;

        if (orderCounter > ConstGame.MAX_ORDER)
        {
            GameManager.Instance.EndGame();
        }
        if (orderCounter == 0 && isSpawning == false)
        {
            StartCoroutine(GameManager.Instance.EndLevel());
        }
        remainOrderCounter.text = $"Orders: {orderCounter}/{ConstGame.MAX_ORDER}";       
        if(orderCounter >= ConstGame.MAX_ORDER)
        {
            remainOrderCounter.color = Color.red;
            if(orderCounter == ConstGame.MAX_ORDER)
            {
                remainTimeSpawnText.text = "Last order! Be careful! Do your best!";
            }
            else
            {
                remainTimeSpawnText.text = "Too many orders! Game Over!";
            }
        }
        else
        {
            remainOrderCounter.color = Color.white;
        }
    }


    public List<OrderGame> receivedOrders = new();

    public GameObject orderPrefab;
    public List<Transform> orderSpawnPoints;

    public Transform parentOrder;

    public TextMeshProUGUI remainTimeSpawnText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI remainOrderCounter;

    public bool isSpawning = true;
    private Coroutine spawnCoroutine;


    public void Init()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        isSpawning = true;
        if (spawnCoroutine == null)
        {
            //Debug.Log("Start Spawning Orders");
            spawnCoroutine = StartCoroutine(SpawnRoutine());
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
             Debug.Log("Stop Spawning Orders");
        }
    }


    private IEnumerator SpawnRoutine()
    {
        int waveCounter = 0;
        int wavePerLevel = GameManager.Instance.IsTutorialCompleted ? ConstGame.WAVE_PER_LEVEL : 1;
        while (waveCounter < wavePerLevel)
        {
            if (isSpawning)
            {
                if(GameManager.Instance.IsTutorialCompleted == false)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        CreateOrder(Random.Range(100, 200 + GameData.Instance.currentLevel * 50));
                        yield return new WaitForSeconds(2f);
                    }
                }
                else
                {
                    for (int i = 0; i < ConstGame.SPAWN_ORDER_PER_WAVE; i++)
                    {
                        CreateOrder(Random.Range(350, 500 + GameData.Instance.currentLevel * 100));
                        yield return new WaitForSeconds(2f);
                    }
                }
            }
            float elapsedTime = 0f;
            waveCounter++;
            if(waveCounter >= wavePerLevel)
            {
                break;
            }
            while (elapsedTime < ConstGame.TIME_PER_WAVE)
            {
                remainTimeSpawnText.text = $"Next wave in: {Mathf.Ceil(ConstGame.TIME_PER_WAVE - elapsedTime)}s";
                elapsedTime += Time.deltaTime;
                yield return null;
                if(orderCounter <= 0)
                {
                    break;
                }
            }
        }
        remainTimeSpawnText.text = "No more waves";
        StopSpawning();
    }

    void Update()
    {
        //UpdateRemainingTime();
        //OrderTimeline.Instance.UpdateUI();
    }

    //private void UpdateRemainingTime() {
    //    foreach (var order in receivedOrders)
    //    {
    //        order.order.UpdateRemainingTime(Time.deltaTime);
    //    }
    //}
    public void CreateOrder(int totalValue)
    {
        Order newOrder = new(orderCounter, totalValue);
        SetOrderCounter(orderCounter + 1);
        // update UI and other logic: createObject for order, add Icon to timeline, etc.

        // createObject for order
        GameObject orderGO = ObjectPoolManager.Instance.Spawn(orderPrefab);
        orderGO.transform.position = orderSpawnPoints[Random.Range(0, orderSpawnPoints.Count)].position;
        // Set Information of order to orderGO
        SetOrderInformation(orderGO, newOrder);
        orderGO.transform.SetParent(parentOrder);

        // add Icon to timeline
        // GameObject orderIcon = OrderTimeline.Instance.AddOrderIconToTimeline();

        OrderGame newOrderGame = new()
        {
            order = newOrder,
            gameObject = orderGO,
            // iconTimelineGameObject = orderIcon
        };

        ReceiveOrder(newOrderGame);
        UITableOrderManager.Instance.UpdateUI();
        //OrderTimeline.Instance.UpdateUI();
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
    public bool DeliveryOrder(Order order)
    {
        List<Order> matchedOrders = GetMatchedOrders(order);
        Order bestOrder = GetBestMatchedOrder(matchedOrders);
        if (bestOrder != null)
        {
            //Debug.Log($"Delivered order: {bestOrder}");
            OrderGame orderGame = receivedOrders.Find(o => o.order.OrderId == bestOrder.OrderId);
            ObjectPoolManager.Instance.Despawn (orderGame.gameObject);
            receivedOrders.Remove(orderGame);
            SetOrderCounter(orderCounter - 1);
            // Add score or other logic for successful delivery
            /////
            /// 
            ///
            GameData.Instance.AddMoney(bestOrder.TotalValue * ConstGame.SCORE_PER_ORDER_PER_VALUE);
            scoreText.text = $"Score: {GameData.Instance.currentMoney}";
            //
            return true;
        }
        return false;
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
        //Debug.Log($"Received order: {orderGame.order}");
        SoundFXManager.Instance.PlaySoundFX("quest", transform);
    }

    private List<Order> GetMatchedOrders(Order order)
    {
        List<Order> matchedOrders = new();
        foreach (var receivedOrder in receivedOrders)
        {
            if (receivedOrder.order.IsSameOrder(order) 
                && order.items.Count == receivedOrder.order.items.Count
                && order.items.Count != 0)
            {
                matchedOrders.Add(receivedOrder.order);
            }
        }
        return matchedOrders;
    }


}
