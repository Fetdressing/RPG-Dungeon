using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayText : MonoBehaviour
{
    private Camera lookCamera;

    private TextMeshPro textComp;

    public void SetText(string text)
    {
        textComp.text = text;

        StartCoroutine(DisplayHitTextIE());
    }

    private void Awake()
    {
        this.textComp = this.GetComponent<TextMeshPro>();
        lookCamera = Camera.main;
    }

    private void Update()
    {
        this.transform.LookAt(-lookCamera.transform.position);
    }

    private IEnumerator DisplayHitTextIE()
    {
        float displayTime = 2f;
        float speed = 10;

        float accumTime = 0f;

        while (displayTime > accumTime)
        {
            float norReductionVal = 1 - (accumTime / displayTime);

            this.transform.position = this.transform.position + new Vector3(0, Time.deltaTime * speed, 0);
            speed = System.Math.Max(speed * norReductionVal, 0.05f);
            textComp.color = new Color(1, 1, 1, norReductionVal);

            accumTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Destroy(this.gameObject);
    }
}
