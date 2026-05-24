using UnityEngine;

public class GameData : Singleton<GameData>
{
    public int currentLevel = 0;

    
    public int currentMoney = 0;

    public void AddMoney(int amount)
    {
        currentMoney += amount;
    }




}
