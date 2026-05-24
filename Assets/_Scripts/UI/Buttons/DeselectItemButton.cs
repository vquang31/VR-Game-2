using UnityEngine;

public class DeselectItemButton : BaseButton
{
    protected override void OnClick()
    {
        base.OnClick();
        int index = GetItemIndex();
        Player.Instance.RemoveItemFromCart(index);
    }

    private int GetItemIndex()
    {
        string name = gameObject.name;
        // format name: ItemCart (0)
        int index = int.Parse(name.Substring(name.LastIndexOf('(') + 1, name.LastIndexOf(')') - name.LastIndexOf('(') - 1));
        return index;
    }



}
