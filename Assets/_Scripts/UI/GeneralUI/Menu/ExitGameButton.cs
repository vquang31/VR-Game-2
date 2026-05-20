using UnityEngine;

public class ExitGameButton : BaseButton
{
    protected override void OnClick()
    {
        // Exit the game
        Application.Quit();

                // If running in the editor, log a message instead
        #if UNITY_EDITOR
                Debug.Log("Exit button clicked. Application.Quit() called.");
        #endif
    }
}
