using NUnit.Framework;
using PDollarGestureRecognizer;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : Singleton<Player>
{
    public Order cart = new();

    public List<EnhancementType> activeEnhancementMagic = new();
    
    //private bool SummonMode = false;
    
    public List<Item> inventory = new();

    public CastStatus Cast(Result gestureResult)
    {
        DetectorShape.Shape shape = DetectorShape.LabelToShape(gestureResult.GestureClass);
        Debug.Log($"Shape: {gestureResult.GestureClass} ,gesture: {shape} -- score: {gestureResult.Score}");
        if (gestureResult.Score < ConstGame.MIN_GESTURE_SCORE)
        {
            return CastStatus.Failed ;
        }
        Magic magic = MagicManager.Instance.GetMagic(shape);
        if(magic == null)
        {
            Debug.Log($"No magic found for gesture: {shape}");
            return CastStatus.Failed;
        }
        if(magic.IsOnCooldown())
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
                if(!activeEnhancementMagic.Contains(enhancementMagic.EnhancementType))
                    activeEnhancementMagic.Add(enhancementMagic.EnhancementType);
                break;
            case SummonMagic summonMagic:
                /// Summon item and add to inventory
                Item item = ItemFactory.CreateItem(summonMagic);
                item.ApplyEnhancement(activeEnhancementMagic);
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
        inventory.Add(item);
    }

    public void AddItemToCart(Item item)
    {
        cart.AddItem(item);
    }

}

public enum CastStatus
{
    Success,
    Failed,
    Cooldown,
    None,
}