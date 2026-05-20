using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : Singleton<ChangeSceneManager>
{

    private string nameGO = "Crossfade";


    public void ChangeScene(int indexScene)
    {
        StartCoroutine(ChangeSceneRoutine(indexScene));
    }
    public IEnumerator ChangeSceneRoutine(int indexScene)
    {

        GameObject crossfadeGO = GameObject.Find(nameGO);
        Animator animator = crossfadeGO.GetComponent<Animator>();
        animator.SetTrigger("End");

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(indexScene);
    }
}
