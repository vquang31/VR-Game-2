using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class OrderUI : NewMonoBehaviour
{
    private Order order;
    [SerializeField] private GameObject orderItemPrefab;

    [SerializeField] private TextMeshProUGUI orderNameText;
    [SerializeField] private GameObject gridLayoutItemGroup;

    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    protected override void Reset()
    {
        ResetUI();
    }

    private void ResetUI()
    {
        orderNameText.text = "";
        foreach (Transform child in gridLayoutItemGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void SetOrderInformation(Order order)
    {
        Reset();
        orderNameText.text = "Order-" + order.orderId.ToString();

        foreach (var item in order.items)
        {
            GameObject itemGO = Instantiate(orderItemPrefab, gridLayoutItemGroup.transform);
            SetInformation(item, itemGO);
        }
    }

    private void SetInformation(Item item, GameObject itemGO)
    {
        Image itemIcon = itemGO.transform.Find("Item_Image").gameObject.GetComponent<Image>();
        // GameObject itemMagicIcon1 = itemGO.transform.Find("MagicIcon1_Image").gameObject;
        // GameObject itemMagicIcon2GO = itemGO.transform.Find("MagicIcon2_Image").gameObject;
        // GameObject itemMagicIcon3GO = itemGO.transform.Find("MagicIcon3_Image").gameObject;

        List<GameObject> itemMagicIconGOs = new();
        for (int j = 1; j <= 3; j++)
        {
            // string s =
            GameObject gameObject = itemGO.transform.Find("MagicIcon" + j.ToString() + "_Image").gameObject;
            itemMagicIconGOs.Add(gameObject);
        }

        SummonMagicData summonMagicData = MagicPool.Instance.GetSummonMagicData(item.itemType);

        itemIcon.sprite = summonMagicData.summonIcon;

        // itemMagicIconGOs[0].GetComponent<Image>().sprite = summonMagicData.magicIcon;

        List<EnhancementType> enhancementTypes = item.enhancements;

        List<EnhancementMagicData> enhancementMagicDatas = MagicPool.Instance.GetEnhancementMagicData(enhancementTypes);

        int i = 0;
        for (; i < enhancementMagicDatas.Count; i++)
        {
            itemMagicIconGOs[i].GetComponent<Image>().sprite = enhancementMagicDatas[i].magicIcon;
        }
        itemMagicIconGOs[i].GetComponent<Image>().sprite = summonMagicData.magicIcon;

        i++;
        for (; i <= ConstGame.MAX_ENHANCEMENT_LEVEL + 1; i++)
        {
            itemMagicIconGOs[i].SetActive(false);
        }

    }

}
