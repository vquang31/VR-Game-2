using UnityEngine;

[CreateAssetMenu(fileName = "BuffMagic", menuName = "Magic/Buff Card")]
public class BuffMagicData : MagicData
{
    [Header("============================")]
    public EffectType effectType;
}

public enum EffectType
{
    Freeze
}