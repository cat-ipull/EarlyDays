using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon_FollowCam : MonoBehaviour
{
    public GameObject target; // The target object to follow
    
    public Vector3 offset; // The offset from the target's position

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.transform.position + offset;
    }
}
