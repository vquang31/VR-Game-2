using UnityEngine;

public class DeliveryButton : BaseButton
{
    protected override void OnClick()
    {
        Player.Instance.DeliveryItems();
    }
}
