using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public List<EnhancementType> enhancements = new();

    public void ApplyEnhancement(List<EnhancementType> enhancementTypes)
    {
        foreach (var type in enhancementTypes)
        {
            if (!enhancements.Contains(type))
            {
                enhancements.Add(type);
            }
        }
    }

    public int Value
    {
        get
        {
            int baseValue = GetBaseValue(itemType);
            int enhancementValue = GetEnhancementValue(); // Each enhancement adds 10 to the value
            return baseValue + enhancementValue;
        }
    }

    private int GetBaseValue(ItemType type)
    {
        if (ConstGame.baseValuesItem.TryGetValue(type, out int value))
        {
            return value;
        }
        return 0; // Default value if item type is not found
    }
    private int GetEnhancementValue()
    {
        int totalEnhancementValue = 0;
        foreach (var enhancement in enhancements)
        {
            totalEnhancementValue += GetEnhancementValue(enhancement);
        }
        return totalEnhancementValue;
    }

    private int GetEnhancementValue(EnhancementType type)
    {
        if (ConstGame.enhancementValues.TryGetValue(type, out int value))
        {
            return value;
        }
        return 0; // Default value if enhancement type is not found
    }

    public bool IsSameItem(Item other)
    {
        if (other == null) return false;
        if (itemType != other.itemType) return false;
        // Check if both items have the same enhancements
        if (enhancements.Count != other.enhancements.Count) return false;
        foreach (var enhancement in enhancements)
        {
            if (!other.enhancements.Contains(enhancement))
            {
                return false;
            }
        }
        return true;
    }

}

public enum ItemType
{
    Sword,
    Shield,
    Potion,
    Bow,
    Halberd,
}

public enum EnhancementType
{
    IncreaseSize,
    Exchange,
    YellowAura,
    PurpleAura,
}