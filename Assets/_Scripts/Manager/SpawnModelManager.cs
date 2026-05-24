using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnModelManager : Singleton<SpawnModelManager>
{

    [SerializeField]
    private List<Transform> spawnItemPosition;



    public GameObject SpawnItemModel(Item item)
    {
        GameObject modelPrefabGO = MagicManager.Instance.GetItemModel(item);
        GameObject itemModel = ObjectPoolManager.Instance.Spawn(modelPrefabGO);
        itemModel.transform.position = spawnItemPosition[Random.Range(0, spawnItemPosition.Count)].position;

        // clear VFX, add new VFX
        // clear old VFX
        // add new VFX

        ResetVFXItem(item, itemModel);


        //
        StartCoroutine(DespawnItem(itemModel, ConstGame.SPAWN_ITEM_MODEL_DURATION));
        return itemModel;
    }


    public void SpawnItemToCart(Item item, GameObject gameObject)
    {
        gameObject.transform.position = spawnItemPosition[Random.Range(0, spawnItemPosition.Count)].position;

        ResetVFXItem(item, gameObject);

        gameObject.SetActive(true);

    }

    private void ResetVFXItem(Item item, GameObject itemModel)
    {

    }

    private IEnumerator DespawnItem(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        ObjectPoolManager.Instance.Despawn(go);
    }
    public void DespawnItem(GameObject go)
    {
        ObjectPoolManager.Instance.Despawn(go);
    }
}
