using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform transform;
    public float shakeDur = 0f;
    public float shakeMagnitude = 0.7f;
    public float dampSpeed = 1f;

    private SpriteRenderer rend;

    Vector3 initPos;

    private void Awake()
    {
        if(transform == null)
        {
            transform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    private void OnEnable()
    {
        Vector3 tmpPos = GameObject.Find("Player").transform.position;
        initPos = new Vector3(tmpPos.x, tmpPos.y, transform.position.z);
    }
    private void Update()
    {
        Vector3 tmpPos = GameObject.Find("Player").transform.position;
        initPos = new Vector3(tmpPos.x, tmpPos.y, transform.position.z);
        if (shakeDur > 0)
        {
            transform.localPosition = initPos + Random.insideUnitSphere * shakeMagnitude;

            shakeDur -= Time.deltaTime * dampSpeed;
        }
        else
        {
            shakeDur = 0f;
            transform.localPosition = initPos;
        }
    }

    public void TriggerShake(float dur, float dampSpeed, float shakeMag)
    {
        shakeDur = dur;
        this.dampSpeed = dampSpeed;
        shakeMagnitude = shakeMag;
    }
}
