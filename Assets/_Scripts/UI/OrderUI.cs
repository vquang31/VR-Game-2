using UnityEngine;

public class OrderUI : NewMonoBehaviour
{


    protected override void LoadComponents()
    {
        base.LoadComponents();
        
    
    }

    protected override void Reset()
    {
        ResetUI();
    }

    private void ResetUI()
    {

    }
    public void SetOrderInformation(Order order)
    {
        Reset();
    }


}
