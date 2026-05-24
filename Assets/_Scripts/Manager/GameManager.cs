using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool IsIniting = false;
    public bool IsGameOver = false;
    public bool IsTutorialCompleted = false;
    public GameObject directionLight;

    protected override void Start()
    {
        base.Start();
        StartGame();
    }


    public void StartGame()
    {
        IsIniting = true;
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        UIGameManager.Instance.Init();
        UIInventoryManager.Instance.UpdateInventoryUI();
        UICartManager.Instance.UpdateCartUI();

        yield return new WaitForSeconds(5f);
        //GeneratorEnemy.Instance.Init();
        UIGameManager.Instance.ShowAnnouncement(ConstUI.announcementStartTutorial, ConstUI.durationAnnouncement * 3);
        yield return new WaitForSeconds(5f);
        UIGameManager.Instance.ShowAnnouncement("Hold left trigger to open inventory,", 4f);
        yield return new WaitForSeconds(5f);
        UIGameManager.Instance.ShowAnnouncement("trigger right trigger to select item to cart", 4f);
        yield return new WaitForSeconds(5f);
        UIGameManager.Instance.ShowAnnouncement("Trigger right to draw magic shape to cast magic", 4f);
        yield return new WaitForSeconds(5f);
        UIGameManager.Instance.ShowAnnouncement("Complete the order to win the level", 4f);
        yield return new WaitForSeconds(5f);
        UIGameManager.Instance.ShowAnnouncement("Dont forget to check the magic book to see the magic shape and effect", 4f);
        
        yield return new WaitForSeconds(5f);
        UIGameManager.Instance.ShowAnnouncement("Good luck!", 5f);
        yield return new WaitForSeconds(5f);
        UIGameManager.Instance.ShowAnnouncement("3", 1f);
        yield return new WaitForSeconds(1f);
        UIGameManager.Instance.ShowAnnouncement("2", 1f);
        yield return new WaitForSeconds(1f);
        UIGameManager.Instance.ShowAnnouncement("1", 1f);
        yield return new WaitForSeconds(1f);
        //UIGameManager.Instance.ShowAnnouncement()
        OrderManager.Instance.Init();
        yield return new WaitForSeconds(1f);

        IsIniting = false;
    }

    public void EndGame() 
    {
        IsGameOver = true;
        UIGameManager.Instance.ShowAnnouncement(ConstUI.announcementLose, ConstUI.durationAnnouncement);
        SoundFXManager.Instance.PlaySoundFX("losing", transform);
        UIManager.Instance.ShowPanelEndGame();
    }

    public IEnumerator EndLevel() 
    {
        //OrderManager.Instance.StopSpawning();
        UIGameManager.Instance.ShowAnnouncement(ConstUI.announcementCompleteLevel, ConstUI.durationAnnouncement);

        yield return StartCoroutine(RunAnimationSun());
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.ShowPanelEndLevel();
    }

    private IEnumerator RunAnimationSun()
    {
        float duration = 4f;
        float elapsedTime = 0f;
        Vector3 initialRotation = directionLight.transform.rotation.eulerAngles;
        Vector3 targetRotation = initialRotation + new Vector3(180, 0, 0);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            Vector3 currentRotation = Vector3.Lerp(initialRotation, targetRotation, t);
            directionLight.transform.rotation = Quaternion.Euler(currentRotation);
            yield return null;
        }
        directionLight.transform.rotation = Quaternion.Euler(targetRotation);
    }

    public void StartLevel()
    {
        StartCoroutine(StartLevelCoroutine());
    }

    

    public IEnumerator StartLevelCoroutine()
    {
        UIManager.Instance.HidePanelEndLevel();
        GameData.Instance.currentLevel++;

        if(GameData.Instance.currentLevel > ConstGame.TUTORIAL_LEVEL && IsTutorialCompleted == false)
        {
            IsTutorialCompleted = true;
            UIGameManager.Instance.ShowAnnouncement(ConstUI.announcementCompleteTutorial, ConstUI.durationAnnouncement * 3);
            yield return new WaitForSeconds(5f);

            UIGameManager.Instance.ShowAnnouncement("1 level have 4 waves," , 4f);
            yield return new WaitForSeconds(5f);
            UIGameManager.Instance.ShowAnnouncement("each wave will spawn 3 orders", 4f);
            yield return new WaitForSeconds(5f);
            UIGameManager.Instance.ShowAnnouncement("Complete all waves to win the level", 4f);
            yield return new WaitForSeconds(5f);
            //UIGameManager.Instance.ShowAnnouncement("")
           
        }
        UIGameManager.Instance.ShowAnnouncement(ConstUI.announcementStartLevel + GameData.Instance.currentLevel, ConstUI.durationAnnouncement);
        //Debug.Log(" Start Level " + GameData.Instance.currentLevel);


        yield return StartCoroutine(RunAnimationSun());

        yield return new WaitForSeconds(5f);

        OrderManager.Instance.StartSpawning();
    }


    public MagicData CreateClone(MagicData original)
    {
        return Instantiate(original);
    }




}
