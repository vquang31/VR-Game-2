using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Magic
{
    public float cooldownTime = 0f;
    private MagicData magicData;
    public MagicData MagicData
    {
        get { return magicData; }
    }

    public Magic(MagicData magicData)
    {
        //this.magicData = magicData;
        this.magicData = GameManager.Instance.CreateClone(magicData);
    }

    public void OnCooldown()
    {
        // Implement cooldown logic here
        cooldownTime = MagicData.cooldown;
    }

    public bool IsOnCooldown()
    {
        return cooldownTime > 0f;
    }

    public void UpdateCooldown(float deltaTime)
    {
        if (cooldownTime > 0f)
        {
            cooldownTime -= deltaTime;
            if (cooldownTime < 0f)
                cooldownTime = 0f;
        }
    }
}
