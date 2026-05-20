using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameManager : Singleton<UIGameManager>
{
    [SerializeField]
    private GameObject magicCooldownUI_GOprefab;
    /// <summary>
    /// magicCooldownUI_GOprefab have GO:
    /// - Image: MagicIcon
    /// - Slider: CooldownSlider
    /// </summary>

    [SerializeField]
    private Transform magicCooldownUI_Parent;


    [SerializeField]
    private GameObject announcementGO;
    
    public void Init()
    {
        announcementGO.SetActive(false);
    }



    private Coroutine announcementCoroutine;
    public void ShowAnnouncement(string message, float duration)
    {
        if (announcementCoroutine != null)
        {
            StopCoroutine(announcementCoroutine);
        }
        announcementCoroutine = StartCoroutine(AnnouncementCoroutine(message, duration));
    }

    private IEnumerator AnnouncementCoroutine(string message, float duration)
    {
        WaitForSeconds wait = new(duration);
        announcementGO.SetActive(true);
        GameObject childGO = announcementGO.transform.GetChild(0).gameObject;
        TextMeshProUGUI textMeshPro = childGO.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = message;
        //float elapsedTime = 0f;
        //while (elapsedTime < duration)
        //{
        //    elapsedTime += Time.deltaTime;
        //    yield return null;
        //}
        yield return wait;

        announcementGO.SetActive(false);
    }

    public void InitCooldownUI(List<Magic> magicList)
    {
        int index = 0;
        // Cập nhật UI cooldown cho mỗi phép thuật
        foreach (Magic magic in magicList)
        {
            if(magic is not BuffMagic)
            {
                continue; // Bỏ qua phép thuật Summon
            }
            // Giả sử bạn có một phương thức để cập nhật UI cooldown cho từng phép thuật
            InitCooldownUI(magic, index);
            index++;
        }
    }

    // Replace the invalid property with a valid expression-bodied property
    private float XOffset
    {
        get
        {
            return magicCooldownUI_GOprefab.transform.localScale.x * 100f;
        }
    }

    private void InitCooldownUI(Magic magic,int index)
    {
        GameObject go = Instantiate(magicCooldownUI_GOprefab, magicCooldownUI_Parent);
        go.name = magic.MagicData.magicName + "_CooldownUI";


        // Use GetComponent<RectTransform>() instead of .rectTransform
        // Fix for setting localPosition.y (cannot assign to property directly)
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        Vector3 localPos = rectTransform.localPosition;
        localPos.x = index * XOffset;
        rectTransform.localPosition = localPos;

        GameObject ImageGO = go.transform.Find("MagicIcon").gameObject;
        GameObject SliderGO = go.transform.Find("CooldownSlider").gameObject;

        Image image = ImageGO.GetComponent<Image>();
        Slider slider = SliderGO.GetComponent<Slider>();

        slider.maxValue = magic.MagicData.cooldown;
        slider.minValue = 0;

        image.sprite = magic.MagicData.magicIcon;

        // Cập nhật UI cooldown cho phép thuật cụ thể
        // Ví dụ: Bạn có thể có một thanh cooldown hoặc một biểu tượng để hiển thị thời gian còn lại
        // Dưới đây là một ví dụ giả định về cách cập nhật UI cooldown
        //float cooldownPercentage = magic.CooldownTime > 0 ? (magic.CurrentCooldown / magic.CooldownTime) : 0;
        // Cập nhật thanh cooldown hoặc biểu tượng dựa trên cooldownPercentage
        // Ví dụ: CooldownBar.fillAmount = cooldownPercentage;
    }
    public void UpdateCooldownUI(List<Magic> magicList)
    {
        foreach (Magic magic in magicList)
        {

            UpdateCooldownUI(magic);

        }
    }

    private void UpdateCooldownUI(Magic magic)
    {
        // Replace this line:
        // go.transform.position.y = index * 100;

        // With the following code:
        GameObject go = magicCooldownUI_Parent.Find(magic.MagicData.magicName + "_CooldownUI")?.gameObject;
        
        if (go != null)
        {
            GameObject SliderGO = go.transform.Find("CooldownSlider").gameObject;
            Slider slider = SliderGO.GetComponent<Slider>();
            slider.value = magic.cooldownTime;
            GameObject ImageGO = go.transform.Find("MagicIcon").gameObject;
            Image image = ImageGO.GetComponent<Image>();
            Color color = image.color;
            if(slider.value > 0)
            {
                color.a = 0.5f; // Giảm độ mờ khi đang cooldown
            }
            else
            {
                color.a = 1f; // Đặt lại độ mờ khi không còn cooldown
            }
            image.color = color;
        }
    }
}
