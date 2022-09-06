using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stabilizeKickback : MonoBehaviour
{

    public float returnSpeed = 2.0f;
    public Transform myTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        myTransform.localRotation = Quaternion.Slerp(myTransform.localRotation, Quaternion.identity, Time.deltaTime * returnSpeed);
    }
}