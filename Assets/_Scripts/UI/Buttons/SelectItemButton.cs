using UnityEngine;

public class SelectItemButton : BaseButton
{
    protected override void OnClick()
    {
        string name = gameObject.name;
        // format name: ItemInventory (0)
        int index = int.Parse(name.Substring(name.LastIndexOf('(') + 1, name.LastIndexOf(')') - name.LastIndexOf('(') - 1));
        Player.Instance.ToggleSelectItem(index);
    }
}
