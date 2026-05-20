using System.Collections.Generic;
using UnityEngine;

public class OrderTimeline : Singleton<OrderTimeline>
{
    public GameObject endPosition;
    public GameObject startPosition;

    public GameObject timelineGO;
    public List<GameObject> orderIconGOList;

    public GameObject orderIconTimelinePrefab;



    public GameObject AddOrderIconToTimeline()
    {
        // Instantiate a new order icon on the timeline
        GameObject newOrderIcon = ObjectPoolManager.Instance.Spawn(orderIconTimelinePrefab);
        newOrderIcon.transform.SetParent(timelineGO.transform, false);
        // Set the position of the new order icon to the start position
        newOrderIcon.transform.position = startPosition.transform.position;
        // Add the new order icon to the list
        orderIconGOList.Add(newOrderIcon);
        return newOrderIcon;
    }


    public void UpdateUI()
    {

    }


}