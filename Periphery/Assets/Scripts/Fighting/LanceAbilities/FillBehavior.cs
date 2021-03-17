using System.Collections;
using UnityEngine;

public class FillBehavior : MonoBehaviour //Should always be paired with RazeIndicator
{
    public Camera main;
    public GameObject camGameObj;
    public GameObject razeIndicator;
    public GameObject meteorEffect;

    private float finalSize;
    private float fillSpeed;
    private float radius = 4f;
    private float startTime;
    private bool isFading;
    private Vector3 finalSizeVector;
    private SpriteRenderer rend;

    private const float fillTime = 2f;
    private const float quakeShakeDur = 1.5f;
    private const float quakeShakeMag = 0.08f;
    private const float m_impactShakeDur = 0.25f;
    private const float m_impactShakeMag = 0.1f;
    private const float meteorDelay = 0.1f;
    private const int numOfMeteors = 15;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        main = Camera.main;
        camGameObj = main.gameObject;
    }

    void Start()
    {
        startTime = Time.time;
        transform.localScale = Vector3.zero;
        radius = GameObject.Find("RazeIndicator").GetComponent<ImpactIndicator>().radius;
        finalSize = radius * 2f;        
        finalSizeVector = new Vector3(finalSize, finalSize, 0);
    }

    void Update()
    {
        float runTime = Time.time - startTime;
        if (this.transform.localScale != finalSizeVector && this.transform.localScale.magnitude < finalSizeVector.magnitude) 
        {
            fillSpeed = finalSize / fillTime * Time.deltaTime;
            transform.localScale += new Vector3(fillSpeed, fillSpeed, 0f);
        }
        else
        {
            if(!isFading)
            {
                Destroy(GameObject.Find("RazeIndicator"));
                StartCoroutine(Fade(1f, 0.5f));
            }
        }
    }

    private IEnumerator Fade(float fadeTo, float fadeDur)
    {
        StartCoroutine(SpawnRain(numOfMeteors, quakeShakeDur + 0.2f));
        camGameObj.GetComponent<CameraShake>().TriggerShake(quakeShakeDur, 1.5f, quakeShakeMag);
        isFading = true;
        float t = 0;
        Color color = rend.material.color;
        while (t < fadeDur)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1, 0, t / fadeDur);

            rend.color = new Color(color.r, color.g, color.b, a);
            yield return null;
        }
    }
    
    private IEnumerator SpawnRain(int n, float delay)
    {
        yield return new WaitForSeconds(delay);
        for(int i=0; i<n; i++)
        {
            Vector3 randTarget = transform.position + (Vector3)Random.insideUnitCircle * radius;
            Instantiate(meteorEffect, randTarget, Quaternion.identity);
            camGameObj.GetComponent<CameraShake>().TriggerShake(m_impactShakeDur, 1.5f, m_impactShakeMag);
            yield return new WaitForSeconds(meteorDelay); //TODO: Add indicator for meteor impact area
        }
        Destroy(gameObject);
    }
}
