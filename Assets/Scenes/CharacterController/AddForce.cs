using BepuPhysicsUnity;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    public DynamicBody dynamicBody;
    // Start is called before the first frame update
    void Start()
    {
        
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
            dynamicBody.AddImpulse(new Vector3(0,1,0));
        }
    }
}
