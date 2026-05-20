using UnityEngine;

public class PauseGameButton : NextButton
{
    protected override void OnClick()
    {


        UIManager.Instance.ShowPauseMenuAndPauseGame();
    }


}
