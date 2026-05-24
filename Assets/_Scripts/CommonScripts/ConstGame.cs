using System.Collections.Generic;
using UnityEngine;

public class ConstGame
{
    public const float MIN_GESTURE_SCORE = 0.8f;

    public const int SPAWN_ORDER_PER_WAVE = 3;
    public const int WAVE_PER_LEVEL = 4;
    public const float TIME_PER_WAVE = 180f;
    public const int MAX_ORDER = 5;
    public const int TUTORIAL_LEVEL = 3;

    public const int SCORE_PER_ORDER_PER_VALUE = 1;
    public const float REMAINING_TIME_ORDER = 1000f;

    public const int MAX_INVENTORY_SIZE = 20;
    public const int MAX_QUANTITY_PER_ORDER = 5;
    public const int MAX_ENHANCEMENT_LEVEL = 4;

    public const float SPAWN_ITEM_MODEL_DURATION = 10f;



    public static int MIN_ITEM_VALUE
    {
        get
        {
            int minValue = int.MaxValue;
            foreach (var value in baseValuesItem.Values)
            {
                if (value < minValue)
                {
                    minValue = value;
                }
            }
            return minValue;
        }
    }
    public static int MAX_ITEM_VALUE
    {
        get
        {
            int maxValue = int.MinValue;
            foreach (var value in baseValuesItem.Values)
            {
                if (value > maxValue)
                {
                    maxValue = value;
                }
            }
            return maxValue;
        }
    }


    public static int MIN_ENHANCEMENT_VALUE
    {
        get
        {
            int minValue = int.MaxValue;
            foreach (var value in enhancementValues.Values)
            {
                if (value < minValue)
                {
                    minValue = value;
                }
            }
            return minValue;
        }
    }

    public static int MAX_ENHANCEMENT_VALUE
    {
        get
        {
            int maxValue = int.MinValue;
            foreach (var value in enhancementValues.Values)
            {
                if (value > maxValue)
                {
                    maxValue = value;
                }
            }
            return maxValue;
        }
    }
    public static ItemType GetRandomValidItemTypeByValue(int itemValue)
    {
        List<ItemType> validItemTypes = new();
        foreach (var kvp in baseValuesItem)
        {
            if (kvp.Value <= itemValue)
            {
                validItemTypes.Add(kvp.Key);
            }
        }
        if (validItemTypes.Count == 0)
        {
            Debug.LogWarning("No valid item types found for the given item value.");
            return default;
        }
        int randomIndex = Random.Range(0, validItemTypes.Count);
        return validItemTypes[randomIndex];
    }

    public static EnhancementType GetRandomValidEnhancementTypeByValue(int enhancementValue)
    {
        List<EnhancementType> validEnhancementTypes = new();
        foreach (var kvp in enhancementValues)
        {
            if (kvp.Value <= enhancementValue)
            {
                validEnhancementTypes.Add(kvp.Key);
            }
        }
        if (validEnhancementTypes.Count == 0)
        {
            Debug.LogWarning("No valid enhancement types found for the given enhancement value.");
            return default;
        }
        int randomIndex = Random.Range(0, validEnhancementTypes.Count);
        return validEnhancementTypes[randomIndex];
    }


    public static Dictionary<ItemType, int> baseValuesItem = new()
    {
        { ItemType.Potion, 100 },
        { ItemType.Shield, 100 },
        { ItemType.Sword, 100 },
        { ItemType.Bow, 250 },
        { ItemType.Halberd, 250 },
    };

    public static Dictionary<EnhancementType, int> enhancementValues = new()
    {
        { EnhancementType.IncreaseSize, 60  },
        { EnhancementType.Exchange, 100 },
        { EnhancementType.YellowAura, 80 },
        { EnhancementType.PurpleAura, 80 },
    };

}
