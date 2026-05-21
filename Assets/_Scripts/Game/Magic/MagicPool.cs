using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MagicPool : Singleton<MagicPool>
{
    // Danh sách chứa tất cả MagicData được tạo trong Unity Editor 
    // tạm thời sử dụng kéo thả vào inspector, sau này có thể load từ Resources hoặc AssetBundle
    public List<MagicData> magicDataList;



    public SummonMagicData GetSummonMagicData(ItemType itemType)
    {
        foreach (MagicData magicData in magicDataList)
        {
            if (magicData is SummonMagicData summonMagicData)
            {
                if (summonMagicData.itemType == itemType)
                    return summonMagicData;
            }
        }
        return null;
    }
    public List<EnhancementMagicData> GetEnhancementMagicData(List<EnhancementType> enhancementTypes)
    {
        List<EnhancementMagicData> enhancementMagicDatas = new();
        foreach (MagicData magicData in magicDataList)
        {
            if (magicData is EnhancementMagicData enhancementMagicData)
            {
                if (enhancementTypes.Contains(enhancementMagicData.enhancementType))
                {
                    enhancementMagicDatas.Add(enhancementMagicData);
                }
            }
        }
        // or
        // // List<EnhancementMagicData> enhancementMagicDatas = magicDataList.Find(())
        // foreach (EnhancementType enhancementType in enhancementTypes)
        // {
        //     enhancementMagicDatas.Find()
        // }
        return enhancementMagicDatas;
    }

}
