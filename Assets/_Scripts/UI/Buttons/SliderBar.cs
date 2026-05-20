using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : NewMonoBehaviour
{
    [SerializeField]
    protected Slider _slider;
    [SerializeField]
    protected GameObject _textGO;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        _slider = GetComponent<Slider>();
        _textGO = transform.Find("Text").gameObject;
    }

    public virtual void SetValue( float x)
    {
        _slider.value = x;
        _textGO.GetComponent<TextMeshProUGUI>().text = x.ToString() + "/" + _slider.maxValue;

    }

}
