using UnityEngine;
[CreateAssetMenu(fileName = "SummonMagic", menuName = "Magic/Summon Magic")]
public class SummonMagicData : MagicData
{

    [Header("============================")]
    public GameObject summonPrefab; // Prefab của đối tượng được triệu hồi -- model

    public GameObject enhanceSummonPrefab; // Prefab của đối tượng được triệu hồi khi có enhancement -- model
    public ItemType itemType; // Loại vật phẩm để triệu hồi 
    public Sprite summonIcon; // image - iconModel

    public Sprite enhanceSummonIcon; // image - iconModel khi có enhancement
}
