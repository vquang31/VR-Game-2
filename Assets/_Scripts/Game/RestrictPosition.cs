using UnityEngine;

//public class RestrictPosition : NewMonoBehaviour
//{
//    [SerializeField]
//    private GameObject anchorPositionGO;

//    [SerializeField]
//    private float maxDistanceFromAnchor = 5f;

//    void Update()
//    {
//        if (anchorPositionGO == null)
//        {
//            Debug.LogWarning("Anchor Position GameObject is not assigned.");
//            return;
//        }
//        Vector3 anchorPosition = anchorPositionGO.transform.position;
//        Vector3 currentPosition = transform.position;
//        float distanceFromAnchor = Vector3.Distance(currentPosition, anchorPosition);
//        if (distanceFromAnchor > maxDistanceFromAnchor)
//        {
//            Vector3 directionToAnchor = (anchorPosition - currentPosition).normalized;
//            transform.position = anchorPosition - directionToAnchor * maxDistanceFromAnchor;
//        }
//    }
//}
public class RestrictPosition : MonoBehaviour
{
    [SerializeField] private Transform anchor;
    [SerializeField] private float maxDistance = 5f;

    void LateUpdate() // dùng LateUpdate để override sau XR update
    {
        if (anchor == null) return;

        Vector3 pos = transform.position;
        Vector3 center = anchor.position;

        Vector3 offset = pos - center;

        if (offset.magnitude > maxDistance)
        {
            transform.position = center + offset.normalized * maxDistance;
        }
    }
}