using UnityEngine;

[System.Serializable]
public class BuffMagic : Magic
{
    public bool isBuffApplied = false;
    public BuffMagic(MagicData magicData) : base(magicData)
    {
    }

    private EffectType effectType => ((BuffMagicData)MagicData).effectType;

    public virtual void ApplyBuff(Player player)
    {
        switch (effectType) {
            case EffectType.Freeze:

                break;
            default:
                break;
        }
        
        // Implement buff logic here
    }



}
