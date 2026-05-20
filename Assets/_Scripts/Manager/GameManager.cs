using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool IsIniting = false;
    public bool IsGameOver = false;

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

        //GeneratorEnemy.Instance.Init();
        yield return new WaitForSeconds(1f);
     
        yield return new WaitForSeconds(1f);

        IsIniting = false;
    }

    public void EndGame() 
    {
        IsGameOver = true;
        
        UIManager.Instance.ShowPanelEndGame();
    }


    public MagicData CreateClone(MagicData original)
    {
        return Instantiate(original);
    }




}
