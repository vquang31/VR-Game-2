using NUnit.Framework;
using PDollarGestureRecognizer;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


[System.Serializable]
public class ItemGame
{
    public Item item;
    public GameObject gameObject;
}

public class Player : Singleton<Player>
{
    public bool canDraw = true;
    public List<ItemGame> cart = new();

    public List<EnhancementType> activeEnhancementMagic = new();

    //private bool SummonMode = false;

    public List<ItemGame> inventory = new();

    public CastStatus Cast(Result gestureResult)
    {

        DetectorShape.Shape shape = DetectorShape.LabelToShape(gestureResult.GestureClass);
        Debug.Log($"Shape: {gestureResult.GestureClass} ,gesture: {shape} -- score: {gestureResult.Score}");
        if (gestureResult.Score < ConstGame.MIN_GESTURE_SCORE)
        {
            return CastStatus.Failed;
        }
        Magic magic = MagicManager.Instance.GetMagic(shape);
        if (magic == null)
        {
            Debug.Log($"No magic found for gesture: {shape}");
            return CastStatus.Failed;
        }
        if (magic.IsOnCooldown())
        {
            Debug.Log($"Magic {shape} is on cooldown.");
            return CastStatus.Cooldown;
        }
        switch (magic)
        {
            case BuffMagic buffMagic:
                buffMagic.ApplyBuff(this);
                // 
                //VFX
                break;
            case EnhancementMagic enhancementMagic:
                //enhancementMagic.ApplyEnhancement(this);
                //enhancementMagic.ApplyEnhancement(this);
                if (!activeEnhancementMagic.Contains(enhancementMagic.EnhancementType))
                {
                    activeEnhancementMagic.Add(enhancementMagic.EnhancementType);
                    UIEnhanceManager.Instance.UpdateUI();
                }
                break;
            case SummonMagic summonMagic:
                /// Summon item and add to inventory
                Item item = ItemFactory.CreateItem(summonMagic);
                item.ApplyEnhancement(activeEnhancementMagic);
                UIGameManager.Instance.ShowAnnouncement($"Summoned {item.itemType} with enhancements: {string.Join(", ", item.enhancements)}", ConstUI.durationAnnouncement);
                activeEnhancementMagic.Clear(); // Clear enhancement after use
                UIEnhanceManager.Instance.UpdateUI();
                AddItemToInventory(item);
                break;
            default:
                Debug.Log("Unknown magic type");
                break;
        }
        magic.OnCooldown();
        //}
        return CastStatus.Success;

    }
    public void AddItemToInventory(Item item)
    {
        GameObject itemModel = SpawnModelManager.Instance.SpawnItemModel(item);
        inventory.Add(new ItemGame { item = item, gameObject = itemModel });
        // Update UI Inventory
        UIInventoryManager.Instance.UpdateInventoryUI();
    }


    public void ToggleSelectItem(int index)
    {
        if (index < 0 || index >= inventory.Count)
        {
            //Debug.LogError("Invalid inventory index");
            return;
        }
        ItemGame itemGame = inventory[index];
        if (cart.Contains(itemGame))
        {
            RemoveItemFromCart(itemGame);
        }
        else
        {
            AddItemToCart(itemGame);

        }
    }


    public void AddItemToCart(ItemGame itemGame)
    {
        cart.Add(itemGame);
        // Spawn Model + VFX
        GameObject itemModel = itemGame.gameObject;
        SpawnModelManager.Instance.SpawnItemToCart(itemGame.item, itemModel);
        UIInventoryManager.Instance.HighlightInventoryItem(itemGame.item);
        UICartManager.Instance.UpdateCartUI();
        UITableOrderManager.Instance.UpdateUI();
    }



    // Check hiệu năng và GC --optimize
    public void DeliveryItems()
    {
        Order order = new Order(cart.Select(itemGame => itemGame.item).ToList());
        bool success = OrderManager.Instance.DeliveryOrder(order);
        if (success)
        {
            RemoveItemInInventory();

        }
    }

    public void RemoveItemFromCart(int index)
    {
        if (index < 0 || index >= cart.Count)
        {
            //Debug.LogError("Invalid cart index");
            return;
        }
        ItemGame itemGame = cart[index];
        RemoveItemFromCart(itemGame);
    }


    public void RemoveItemFromCart(ItemGame itemGame)
    {
        if (cart.Contains(itemGame))
        {
            GameObject itemModel = itemGame.gameObject;
            cart.Remove(itemGame);
            SpawnModelManager.Instance.DespawnItem(itemModel);
            UIInventoryManager.Instance.ResetHighlightInventoryItem(itemGame.item);
            UICartManager.Instance.UpdateCartUI();
            UITableOrderManager.Instance.UpdateUI();
        }
    }




    /// <summary>
    /// remove item from inventory and cart, despawn model, reset highlight, update UI
    /// </summary>
    public void RemoveItemInInventory()
    {
        foreach (var itemGame in cart)
        {
            RemoveItemFromInventory(itemGame.item);
        }
        cart.Clear();
        UIInventoryManager.Instance.ResetHighlightAllInventoryItems();
        UIInventoryManager.Instance.UpdateInventoryUI();
        UITableOrderManager.Instance.UpdateUI();
        UICartManager.Instance.UpdateCartUI();
        //int count = cart.Count;
        //for (int i = count - 1; i >= 0; i--)
        //{
        //    ItemGame itemGame = cart[i];
        //    RemoveItemFromInventory(itemGame.item);
        //    //RemoveItemFromCart(itemGame);
        //}
        //cart.Clear();
        //UIInventoryManager.Instance.ResetHighlightAllInventoryItems();
        //UIInventoryManager.Instance.UpdateInventoryUI();
        //UITableOrderManager.Instance.UpdateUI();
        //UICartManager.Instance.UpdateCartUI();
    }
    private void RemoveItemFromInventory(Item item)
    {
        //Debug.Log($"Remove item from inventory: {item}");
        ItemGame itemGame = inventory.FirstOrDefault(ig => ig.item == item);
        if (itemGame != null)
        {
            inventory.Remove(itemGame);
            SpawnModelManager.Instance.DespawnItem(itemGame.gameObject);
            // Update UI Inventory
            UIInventoryManager.Instance.UpdateInventoryUI();
            UICartManager.Instance.UpdateCartUI();
        }
    }




}

public enum CastStatus
{
    Success,
    Failed,
    Cooldown,
    None,
}