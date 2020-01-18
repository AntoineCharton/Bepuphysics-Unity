using BepuPhysicsUnity;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    public DynamicBody dynamicBody;
    private Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            dynamicBody.AddAngularImpulse(transform.forward);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            dynamicBody.SetPosition(initialPosition);
            dynamicBody.SetRotation(Quaternion.identity);
            dynamicBody.SetVelocity(Vector3.up);
            dynamicBody.SetAngularVelocity(Vector3.zero);
        }
    }
}
