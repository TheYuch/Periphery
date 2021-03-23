using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitShader : MonoBehaviour
{
    private SpriteRenderer rend;
    private bool isDisintegrating = true;
    private float fade = 1f;

    public Texture thisTex;
    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        rend.material.mainTexture = thisTex;
    }

    private void Update()
    {
        if(isDisintegrating) //test
        {
            fade -= Time.deltaTime;
            if(fade <= 0f)
            {
                isDisintegrating = false;
                fade = 0f;
            }
            rend.material.SetFloat("_Fade", fade);
        }
    }
}
