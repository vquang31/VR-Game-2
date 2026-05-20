using UnityEngine;

public class NextButton : MenuButton
{
    [SerializeField] protected GameObject _nextMenuGO;
    protected override void OnClick()
    {
        base.OnClick();
        _currentMenuGO.SetActive(false);
        _nextMenuGO.SetActive(true);
    }
}
