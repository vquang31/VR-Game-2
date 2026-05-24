using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UICartManager : Singleton<UICartManager>
{
    public GameObject cartPanel;
    

    public void UpdateCartUI()
    {
        int itemCount = Player.Instance.cart.Count;
        for (int i = 0; i < cartPanel.transform.childCount; i++)
        {
            GameObject itemSlot = cartPanel.transform.GetChild(i).gameObject;

            Image itemIcon = itemSlot.transform.GetChild(0).GetComponent<Image>();

            if (i < itemCount)
            {
                ItemGame itemGame = Player.Instance.cart[i];
                itemIcon.sprite = MagicManager.Instance.GetIconItem(itemGame.item);
                itemIcon.enabled = true;
            }
            else
            {
                itemIcon.enabled = false;
            }
        }
    }




}
