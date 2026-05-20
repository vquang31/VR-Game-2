using UnityEngine;
[CreateAssetMenu(fileName = "SummonMagic", menuName = "Magic/Summon Magic")]
public class SummonMagicData : MagicData
{

    [Header("============================")]
    public GameObject summonPrefab; // Prefab của đối tượng được triệu hồi
    public ItemType itemType; // Loại vật phẩm để triệu hồi
    public Sprite summonIcon;
}
