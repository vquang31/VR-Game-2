using UnityEngine;
using UnityEngine.UI;

public class BaseButton : NewMonoBehaviour
{
    public Button _button;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        _button = GetComponent<Button>();
    }

    protected override void Start()
    {
        _button.onClick.AddListener(OnClick);
    }

    protected virtual void OnClick()
    {

    }

}
