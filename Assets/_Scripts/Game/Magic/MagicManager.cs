using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagicManager : Singleton<MagicManager>
{
    public List<Magic> magicList = new();

    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    protected override void Start()
    {
        base.Start();
        InitMagic();
        InitUI();
    }

    void Update()
    {
        UpdateMagicCooldowns();
        UpdateCooldownUI();
    }
    protected void UpdateMagicCooldowns()
    {
        foreach (Magic magic in magicList)
        {
            magic.UpdateCooldown(Time.deltaTime);
        }
    }

    private void UpdateCooldownUI()
    {
        UIGameManager.Instance.UpdateCooldownUI(magicList);
    }

    [ContextMenu("Log Magic List")]
    public void LogTest()
    {
        foreach (Magic magic in magicList)
        {
            Debug.Log($"Magic: {magic.MagicData.magicName}, Description: {magic.ToString()}");
            magic.MagicData.cooldown = 4f;
        }
        //foreach (Magic magic in MagicList)
        //{
        //    Debug.Log($"Magic: {magic.magicData.magicName}, Description: {magic.ToString()}");
        //}
    }

    private void InitMagic()
    {
        foreach (MagicData magicData in MagicPool.Instance.magicDataList)
        {
            Magic magic = MagicFactory.CreateMagic(magicData);
            magicList.Add(magic);
        }
    }

    private void InitUI()
    {
        UIGameManager.Instance.InitCooldownUI(magicList);
    }


    public Magic GetMagic(DetectorShape.Shape shape)
    {
        if(shape == DetectorShape.Shape.Unknown)
        {
            Debug.LogWarning("Shape is None, cannot get magic.");
            return null;
        }
        return magicList.FirstOrDefault(m => m.MagicData.shape == shape);
    }


    public GameObject GetItemModel(Item item)
    {
        SummonMagicData summonMagicData = MagicPool.Instance.magicDataList
            .OfType<SummonMagicData>()
            .FirstOrDefault(m => m.itemType == item.itemType);
        if (item.enhancements.Contains(EnhancementType.Exchange))
        {
            return summonMagicData.enhanceSummonPrefab;
        }
        else
        {
            return summonMagicData.summonPrefab;
        }
    }

}
