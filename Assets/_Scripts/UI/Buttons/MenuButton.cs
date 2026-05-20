using UnityEngine;

public class MenuButton : BaseButton
{

    [SerializeField] protected GameObject _currentMenuGO;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        _currentMenuGO = transform.parent.gameObject;
    }

}
