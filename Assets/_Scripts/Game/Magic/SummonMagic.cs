using UnityEngine;

[System.Serializable]
public class SummonMagic : Magic
{
    public GameObject SummonPrefab
    {
        get { return ((SummonMagicData)MagicData).summonPrefab; }
    }

    public GameObject EnhanceSummonPrefab
    {
        get { return ((SummonMagicData)MagicData).enhanceSummonPrefab; }
    }

    public ItemType ItemType
    {
        get { return ((SummonMagicData)MagicData).itemType; }
    }


    public SummonMagic(SummonMagicData magicData) : base(magicData)
    {
    }
}
