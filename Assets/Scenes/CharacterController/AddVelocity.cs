using UnityEngine;
using BepuPhysicsUnity;

public class AddVelocity : MonoBehaviour
{
    public KynematicBody kynematicBody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            kynematicBody.SetVelocity(Vector3.up);
        } else if(Input.GetKey(KeyCode.S))
        {
            kynematicBody.SetVelocity(Vector3.down);
        }

    }
}
