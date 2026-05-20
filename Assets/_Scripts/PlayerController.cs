using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NewMonoBehaviour
{
    private void Update()
    {
        Vector3 linearVelocity = GetComponent<Rigidbody>().linearVelocity;
        if (Input.GetKeyDown(KeyCode.Space) && linearVelocity.y == 0)
        {
            Jump();
        }
        Vector3 v = Vector3.zero;
        if (Keyboard.current.wKey.isPressed)    v.z += 1;
        if(Keyboard.current.sKey.isPressed)    v.z -= 1;
        if(Keyboard.current.aKey.isPressed)    v.x -= 1;
        if(Keyboard.current.dKey.isPressed)    v.x += 1;

        GetComponent<Rigidbody>().AddForce(v.normalized * 10);

        //transform.position += v.normalized * 0.1f;
    }
    private void Jump()
    {
        Vector3 v = GetComponent<Rigidbody>().linearVelocity;
        v.y = 10;
        GetComponent<Rigidbody>().linearVelocity = v;
        Debug.Log("Player Jumped!");
    }
}
