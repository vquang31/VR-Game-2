using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : BaseButton
{
    public int indexScene;
    protected override void OnClick()
    {
        Debug.Log("Change Scene to index: " + indexScene);
        ChangeSceneManager.Instance.ChangeScene(indexScene);
    }

}
