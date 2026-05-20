using UnityEngine;

public class BackButton : MenuButton
{
    
    [SerializeField] private GameObject _previousMenuGO;

    protected override void OnClick()
    {
        base.OnClick();
        _currentMenuGO.SetActive(false);
        _previousMenuGO.SetActive(true);
    }
}
