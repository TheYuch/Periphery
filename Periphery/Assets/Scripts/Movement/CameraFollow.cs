using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Camera mainCam;

    const float lerpSpeed = 5f;
    private void Start()
    {
        mainCam = Camera.main;
    }
    void Update()
    {
        Vector3 camInitialPosition = mainCam.transform.position;
        Vector3 camFinalPosition = new Vector3(this.transform.position.x, this.transform.position.y, mainCam.transform.position.z);
        mainCam.transform.position = Vector3.Lerp(camInitialPosition, camFinalPosition, lerpSpeed * Time.deltaTime);
    }
}
