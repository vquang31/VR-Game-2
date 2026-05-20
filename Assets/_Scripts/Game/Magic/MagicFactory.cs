using UnityEngine;

public static class MagicFactory
{
    public static Magic CreateMagic(MagicData magicData)
    {
        switch (magicData)
        {
            case BuffMagicData buffMagicData:
                return new BuffMagic(buffMagicData);
            case SummonMagicData summonMagicData:
                return new SummonMagic(summonMagicData);
            case EnhancementMagicData enhancementMagicData:
                return new EnhancementMagic(enhancementMagicData);
            default:
                Debug.LogError($"Không thể tạo Magic vì loại MagicData không được hỗ trợ: {magicData.GetType()}");
                return null;
        }
    }
}



