using UnityEngine;

using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Preallocation
{
    public GameObject gameObject;
    [Header("Số lượng khởi tạo ban đầu - Init Value")]
    public int count;
    public bool expandable = true;
}

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public List<Preallocation> preAllocations;
    [SerializeField] List<GameObject> pooledGobjects;

    protected override void Awake()
    {
        base.Awake();

        pooledGobjects = new List<GameObject>();

        foreach (Preallocation item in preAllocations)
        {
            for (int i = 0; i < item.count; ++i)
                pooledGobjects.Add(CreateGobject(item.gameObject));
        }
    }

    public GameObject Spawn(string tag)
    {
        for (int i = 0; i < pooledGobjects.Count; ++i)
        {
            if (!pooledGobjects[i].activeSelf && pooledGobjects[i].CompareTag(tag))
            {
                pooledGobjects[i].SetActive(true);
                return pooledGobjects[i];
            }
        }

        for (int i = 0; i < preAllocations.Count; ++i)
        {
            if (preAllocations[i].gameObject.CompareTag(tag))
                if (preAllocations[i].expandable)
                {
                    GameObject obj = CreateGobject(preAllocations[i].gameObject);
                    pooledGobjects.Add(obj);
                    obj.SetActive(true);
                    return obj;
                }
        }
        return null;
    }


    [Header("Dùng để test Spawn và Despawn - Test Spawn and Despawn")]
    public GameObject testGO;

    public GameObject despawnGO;

    [ContextMenu("Test Spawn")]
    public void TestSpawn()
    {
        GameObject obj = Spawn(testGO);
        if (obj != null)
        {
            Debug.Log("Spawned: " + obj.name);
        }
        else
        {
            Debug.Log("Failed to spawn object with name: " + testGO.name);
        }
    }
    [ContextMenu("Test Despawn")]
    public void TestDespawn()
    {
        Despawn(despawnGO);
    }


    public GameObject Spawn(GameObject item)
    {
        for (int i = 0; i < pooledGobjects.Count; ++i)
        {
            if (!pooledGobjects[i].activeSelf && pooledGobjects[i].name == item.name + "(Clone)")
            {
                pooledGobjects[i].SetActive(true);
                return pooledGobjects[i];
            }
        }
        for (int i = 0; i < preAllocations.Count; ++i)
        {
            if (preAllocations[i].gameObject.name == item.name)
                if (preAllocations[i].expandable)
                {
                    GameObject obj = CreateGobject(preAllocations[i].gameObject);
                    pooledGobjects.Add(obj);
                    obj.SetActive(true);
                    return obj;
                }
        }
        // có thể mở rộng và thêm mới pool ở đây
        // cần kiểm chứng lại 
        Preallocation newPreallocation = new();
        newPreallocation.gameObject = item;
        newPreallocation.count = 1;
        newPreallocation.expandable = true;
        preAllocations.Add(newPreallocation);
        GameObject objNew = CreateGobject(item);
        pooledGobjects.Add(objNew);
        objNew.SetActive(true);
        return objNew;
    }

    public void Despawn(GameObject gobject)
    {
        gobject.SetActive(false);
    }

    public void Despawn(GameObject gobject, float v)
    {
        StartCoroutine(DespawnCoroutine(gobject, v));
    }

    private IEnumerator DespawnCoroutine(GameObject gobject, float v)
    {
        yield return new WaitForSeconds(v);
        gobject.SetActive(false);
    }


    GameObject CreateGobject(GameObject item)
    {
        GameObject gobject = Instantiate(item, transform);
        gobject.SetActive(false);
        return gobject;
    }
    ///
    /// Luồng như này tôi:
    //+Gọi Spawn(GOPrefabA) (GOPrefabA là một GO mới chưa trong Object Pooling) ->Chương trình không tìm thấy bản sao(clone) nên thêm  GOPrefabA vào Preallocation và tạo một Clone giống GOPrefabA
    //+ tôi despawn clone vừa sinh 
    //+ tôi lại gọi Spawn(GOPrefabA) thì GO mới có phải GO tôi vừa despawn không ?
    ///
    /// -> hoạt động tốt như dự kiến

}
