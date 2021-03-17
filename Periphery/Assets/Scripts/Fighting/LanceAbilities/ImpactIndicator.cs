using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ImpactIndicator : MonoBehaviour //play gong sound when ability activates 
{
    public float lineWidth = 0.2f;
    public float radius;
    public bool circleFillScreen;

    public LineRenderer rend;
    public GameObject parent;
    public GameObject fill;

    private Vector3 startPos;

    private const int vertexCount = 310;

    private void Awake()
    {
        rend = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        startPos = parent.transform.position;
        Instantiate(fill, startPos, Quaternion.identity);
    }

    private void Update()
    {
        SetupCircle();
    }

    private void SetupCircle()
    {
        rend.widthMultiplier = lineWidth;

        if(circleFillScreen)
        {
            radius = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0f, Camera.main.pixelRect.yMax, 0f)),
            Camera.main.ScreenToWorldPoint(new Vector3(0f, Camera.main.pixelRect.yMin, 0f))) * 0.5f - lineWidth;
        }

        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        rend.positionCount = vertexCount;
        for(int i=0; i<rend.positionCount; i++)
        {
            Vector3 pos = startPos + new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
            rend.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }

    public IEnumerator FadeAlpha(float fadeTo, float fadeDur)
    {
        float t = 0;
        Color color = rend.material.color;
        while (t < fadeDur)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1, 0, t / fadeDur);
            Color c = new Color(color.r, color.g, color.b, a);
            rend.SetColors(c, c);
            yield return null;
        }
        Destroy(gameObject);
    }
}
