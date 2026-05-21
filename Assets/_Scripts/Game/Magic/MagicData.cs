using Unity.VectorGraphics;
using UnityEngine;

public abstract class MagicData : ScriptableObject
{
    public string magicName;
    public Sprite magicIcon;
    public string description;
    public float duration; // Thời gian tồn tại của phép thuật (nếu có)
    public float cooldown; // Thời gian hồi chiêu của phép thuật

    [Header("=========Dữ liệu người chơi không thấy=========")]

    [Header("MagicIcon và Shape phải giống nhau")]
    [Tooltip("MagicIcon và Shape phải giống nhau")]
    public DetectorShape.Shape shape;// Hình dạng của phép thuật

}

///
/// Label(String) -----[DetectorShape]--------> (MagicData)Shape ----[GameDesigner - ScriptableObject]------> Magic
/// 
/// 
///







