using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBehavior : MonoBehaviour
{
    public int index; //max swords: 6
    public GameObject parent;
    public GameObject effect;
    public ParticleSystem trail;

    SpriteRenderer rend;
    private Vector3 parentPos;
    private float angularVelocity = -80f;
    private float currAngle;
    private bool isFading = false;
    private bool psIsFading = false;

    private const float weaponDist = 1.5f;
    private const float fadeTime = 1f;
    private const float speedMultiplier = -1000f;
    private const float angleOffset = -45f;
    private void Awake()
    {
        rend = this.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        trail.Play();
        Instantiate(effect, this.transform);
        currAngle = angleOffset - 60 * index - 45f;
        transform.position = parent.transform.position + new Vector3(Mathf.Cos(currAngle * Mathf.Deg2Rad), Mathf.Sin(currAngle * Mathf.Deg2Rad), 0) * weaponDist;
        transform.localEulerAngles = new Vector3(0, 0, currAngle + angleOffset - 90f + angularVelocity * Time.deltaTime);
    }

    void Update()
    {
        if(!psIsFading)
        {
            StartCoroutine(FadePS(fadeTime));
        }
        transform.position = parent.transform.position + new Vector3(Mathf.Cos(currAngle * Mathf.Deg2Rad), Mathf.Sin(currAngle * Mathf.Deg2Rad), 0) * weaponDist; //TODO: make compatible with enemies, probably with isEnemy bool
        transform.localEulerAngles = new Vector3(0, 0, currAngle + angleOffset - 90f + angularVelocity * Time.deltaTime);
        StartCoroutine(Wait(0.5f));     
    }
    private IEnumerator fadeAlpha(float fadeTo, float fadeDur)
    {
        isFading = true;
        float t = 0;
        Color color = rend.material.color;
        while(t < fadeDur)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1, 0, t / fadeDur);

            rend.color = new Color(color.r, color.g, color.b, a);
            yield return null;
        }
        Destroy(gameObject);
    }

    private IEnumerator Wait(float t)
    {
        yield return new WaitForSeconds(t);
        transform.position = parent.transform.position + new Vector3(Mathf.Cos(currAngle * Mathf.Deg2Rad), Mathf.Sin(currAngle * Mathf.Deg2Rad), 0) * weaponDist;
        transform.localEulerAngles = new Vector3(0, 0, currAngle + angleOffset - 90f + angularVelocity * Time.deltaTime);
        angularVelocity += speedMultiplier * Time.deltaTime;
        currAngle += angularVelocity * Time.deltaTime;
        if (!isFading)
        {
            StartCoroutine(fadeAlpha(1f, fadeTime));
        }
    }

    private IEnumerator FadePS(float fadeDur)
    {
        psIsFading = true;
        float t = 0;
        Color c = trail.GetComponent<ParticleSystemRenderer>().material.color;
        while (t < fadeDur)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1, 0, t / fadeDur);
            trail.GetComponent<ParticleSystemRenderer>().material.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
    }
}
