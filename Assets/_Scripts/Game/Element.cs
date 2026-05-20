//using UnityEngine;

//[System.Serializable]
//public class Element 
//{
//    [SerializeField]
//    private int fire = 0;
//    [SerializeField]
//    private int water = 0;
//    [SerializeField]
//    private int earth = 0;


//    public int GetCountElement()
//    {
//        return fire + water + earth;
//    }

//    public void AddElement(ElementType elementType)
//    {
//        if (GetCountElement() >= ConstGame.MAX_ELEMENT) return;
//        switch (elementType)
//        {
//            case ElementType.Fire:
//                fire++;
//                break;
//            case ElementType.Water:
//                water++;
//                break;
//            case ElementType.Earth:
//                earth++;
//                break;
//        }
//    }


//    public void ResetElement()
//    {
//        fire = 0;
//        water = 0;
//        earth = 0;
//    }

//}
//public enum ElementType
//{
//    Fire,
//    Water,
//    Earth
//}