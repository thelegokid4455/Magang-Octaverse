using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDestroy : MonoBehaviour
{
    [SerializeField] float DestroyTimer;
    float counter;

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= DestroyTimer) Destroy(gameObject);
    }
}
