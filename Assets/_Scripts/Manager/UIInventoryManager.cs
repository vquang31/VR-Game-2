using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryManager : Singleton<UIInventoryManager>
{
    public GameObject inventoryPanel;


    public void UpdateInventoryUI()
    {
        int itemCount = Player.Instance.inventory.Count;
        for (int i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            GameObject itemSlot = inventoryPanel.transform.GetChild(i).gameObject;
            Image itemIcon = itemSlot.transform.GetChild(0).GetComponent<Image>();
            if( i < itemCount)
            {
                ItemGame itemGame = Player.Instance.inventory[i];
                itemIcon.sprite = MagicManager.Instance.GetIconItem(itemGame.item);
                itemIcon.enabled = true;
            }
            else
            {
                itemIcon.enabled = false;
            }
        }
    }


    public void HighlightInventoryItem(Item item)
    {
        int index = Player.Instance.inventory.FindIndex(ig => ig.item == item);
        HighlightInventoryItem(index);
    }

    public void HighlightInventoryItem(int index)
    {
        if (index < 0 || index >= inventoryPanel.transform.childCount)
        {
            Debug.LogError("Invalid inventory index");
            return;
        }
        GameObject itemSlot = inventoryPanel.transform.GetChild(index).gameObject;
        // Add highlight effect to itemSlot
        Image image = itemSlot.GetComponent<Image>();
        image.color = Color.yellow; // Example highlight effect, change as needed
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.8f); // Example highlight effect, change as needed
    }


    public void ResetHighlightInventoryItem(Item item)
    {
        int index = Player.Instance.inventory.FindIndex(ig => ig.item == item);
        ResetHighlightInventoryItem(index);
    }

    public void ResetHighlightInventoryItem(int index)
    {
        if (index < 0 || index >= inventoryPanel.transform.childCount)
        {
            //Debug.LogError("Invalid inventory index");
            return;
        }
        GameObject itemSlot = inventoryPanel.transform.GetChild(index).gameObject;
        // Remove highlight effect from itemSlot
        Image image = itemSlot.GetComponent<Image>();
        image.color = new(0, 0, 0, 0.8f);
    }


    public void ResetHighlightAllInventoryItems()
    {
        for (int i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            ResetHighlightInventoryItem(i);
        }
    }
}
