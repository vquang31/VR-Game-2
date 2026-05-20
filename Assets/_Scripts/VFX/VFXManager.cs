using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : Singleton<VFXManager>
{


    //public void PlayVFX(GameObject vfxPrefab, TargetVFX vfxTarget, List<Enemy> enemies)
    //{
    //    if(vfxPrefab == null)
    //    {
    //        Debug.LogWarning("VFX Prefab is null. Cannot play VFX.");
    //        return;
    //    }
    //    if(vfxTarget == TargetVFX.All)
    //    {
    //        //enemies = EnemyManager.Instance.GetAllEnemies();
    //        Transform transform = EnemyManager.Instance.allAttackVFXTransform;

    //        GameObject vfxGO = Instantiate(vfxPrefab, transform.position, Quaternion.identity);
    //        StartCoroutine(ProcessVFXGO(vfxGO));
    //        return;
    //    }
    //    if (vfxTarget == TargetVFX.SelectedEnemy && (enemies == null || enemies.Count == 0))
    //    {
    //        Debug.LogWarning("No target enemies provided for SelectedEnemy VFX.");
    //        return;
    //    }

    //    foreach (Enemy enemy in enemies)
    //    {
    //        GameObject vfxGO = Instantiate(vfxPrefab, enemy.transform.position, Quaternion.identity);
    //        vfxGO.transform.parent = gameObject.transform;
    //        //Destroy(vfxGO, timeDuration);
    //        StartCoroutine(ProcessVFXGO(vfxGO));
    //    }

    //}

    //private IEnumerator ProcessVFXGO(GameObject vfxGO)
    //{
    //    yield return null;

    //    foreach (Transform child in vfxGO.transform)
    //    {
    //        if (child.gameObject.TryGetComponent<Animator>(out Animator animator))
    //        {
    //            while (animator != null && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
    //            {
    //                yield return null;
    //            }
    //            break;
    //        }
    //    }

    //    vfxGO.SetActive(false);
    //    Destroy(vfxGO);
    //}





}