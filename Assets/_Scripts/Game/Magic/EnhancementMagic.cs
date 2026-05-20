using UnityEngine;

public class EnhancementMagic : Magic
{
    public EnhancementMagic(MagicData magicData) : base(magicData)
    {
    }

    public EnhancementType EnhancementType
    {
        get { return ((EnhancementMagicData)MagicData).enhancementType; }
    }

}
