//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float speedFollow;
    public Transform target;

    void Update()
    {
        
        Vector3 newPosition = target.position;
        newPosition.z = -10;
        transform.position = Vector3.Slerp(transform.position, newPosition, speedFollow * Time.deltaTime);
        print(newPosition);

    }
}
