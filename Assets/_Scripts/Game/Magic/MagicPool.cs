using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MagicPool : Singleton<MagicPool>
{
    // Danh sách chứa tất cả MagicData được tạo trong Unity Editor 
    // tạm thời sử dụng kéo thả vào inspector, sau này có thể load từ Resources hoặc AssetBundle
    public List<MagicData> magicDataList;

}
