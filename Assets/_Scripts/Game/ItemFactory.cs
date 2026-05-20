using UnityEngine;

public class ItemFactory
{
    public static Item CreateItem(SummonMagic summonMagic)
    {
        if (summonMagic == null)
        {
            Debug.LogError("SummonMagic is null");
            return null;
        }

        Item item = new()
        {
            itemType = summonMagic.ItemType
        };
        return item;
    }

    public static Item CreateItem()
    {
        Item item = new();
        return item;
    }

    /// <summary>
    ///  use this method to create an item with a specific value,
    ///  create when generating order,
    ///  make sure the item value is valid
    /// </summary>
    /// <param name="itemValue"></param>
    /// <returns></returns>
    public static Item CreateValidItem(int itemValue)
    {
        if (itemValue < ConstGame.MIN_ITEM_VALUE)
        {
            Debug.LogError($"CreateValidItem: itemValue ({itemValue}) is less than minimum base item value ({ConstGame.MIN_ITEM_VALUE}).");
            return null;
        }

        Item item = new();

        ItemType itemType = ConstGame.GetRandomValidItemTypeByValue(itemValue);

        if (!ConstGame.baseValuesItem.TryGetValue(itemType, out int baseValue))
        {
            Debug.LogError($"CreateValidItem: base value for itemType {itemType} not found.");
            return null;
        }

        item.itemType = itemType;

        itemValue -= baseValue;
        if (itemValue < 0)
        {
            itemValue = 0;
        }

        int attempt = 0;
        int minValueEnhancement = ConstGame.MIN_ENHANCEMENT_VALUE;
        while (attempt < 100 && itemValue >= minValueEnhancement && item.enhancements.Count < ConstGame.MAX_ENHANCEMENT_LEVEL)
        {
            attempt++;
            EnhancementType enhancementType = ConstGame.GetRandomValidEnhancementTypeByValue(itemValue);

            if (!ConstGame.enhancementValues.TryGetValue(enhancementType, out int enhVal) || enhVal > itemValue)
            {
                Debug.LogWarning($"CreateValidItem: no suitable enhancement for remaining value {itemValue}. Stopping enhancement loop.");
                break;
            }

            item.enhancements.Add(enhancementType);
            itemValue -= enhVal;
        }

        return item;
    }
}