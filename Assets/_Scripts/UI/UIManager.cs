using UnityEngine;


public class UIManager : Singleton<UIManager>
{

    [SerializeField]
    private GameObject pauseMenuUI;

    [SerializeField]
    private GameObject settingMenuUI;

    [SerializeField]
    private GameObject endGameUI;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        pauseMenuUI = GameObject.Find("PauseMenu_UI");
        settingMenuUI = GameObject.Find("SettingMenu_UI");
        endGameUI = GameObject.Find("PanelEndGame_UI");
    }

    protected override void Start()
    {
        base.Start();
        HidePauseMenuAndResumeGame();
        HideSettingMenu();
        HidePanelEndGame();
    }


    public void ShowPanelEndGame()
    {
        endGameUI?.SetActive(true);
    }


    private void HidePanelEndGame()
    {
        endGameUI?.SetActive(false);
    }


    public void ShowPauseMenuAndPauseGame()
    {
        Time.timeScale = 0;
        pauseMenuUI?.SetActive(true);
    }

    public void HidePauseMenuAndResumeGame()
    {
        Time.timeScale = 1;
        pauseMenuUI?.SetActive(false);
    }

    public void ShowSettingMenu()
    {
        settingMenuUI?.SetActive(true);
    }

    public void HideSettingMenu()
    {
        settingMenuUI?.SetActive(false);
    }




}
