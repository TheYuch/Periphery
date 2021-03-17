using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionImminent : MonoBehaviour
{
    public float delay;
    public GameObject targetObj;

    private void Start()
    {
        Destroy(targetObj, delay);   
    }
}
