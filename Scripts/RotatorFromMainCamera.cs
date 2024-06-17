using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorFromMainCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.eulerAngles = new Vector3(90, 0, transform.position.z);
    }
}
