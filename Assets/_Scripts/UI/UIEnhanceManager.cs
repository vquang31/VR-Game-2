using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIEnhanceManager : Singleton<UIEnhanceManager>
{
    public GameObject enhancePanel;

    public List<GameObject> enhanceImageList = new();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        // format EnhanceMagic1_Image, EnhanceMagic2_Image, EnhanceMagic3_Image, EnhanceMagic4_Image
        for (int i = 1; i <= ConstGame.MAX_ENHANCEMENT_LEVEL; i++)
        {
            GameObject enhanceGO = enhancePanel.transform.Find
                ("EnhanceMagic" + i +"_Image").gameObject;
            enhanceImageList.Add(enhanceGO);
        }
        ResetUI();
    }

    private void ResetUI()
    {
        foreach (GameObject go in enhanceImageList)
        {
            go.GetComponent<Image>().enabled = false;
        }
    }
    public void UpdateUI()
    {
        int count = Player.Instance.activeEnhancementMagic.Count;
        for (int i = 0; i < enhanceImageList.Count; i++)
        {
            Image image = enhanceImageList[i].GetComponent<Image>();
            if (i < count)
            {
                image.sprite = MagicManager.Instance.GetEnhancementMagicSprite(Player.Instance.activeEnhancementMagic[i]);
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }
        }
    }

}
