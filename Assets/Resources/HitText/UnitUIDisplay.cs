using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Canvas))]
public class UnitUIDisplay : MonoBehaviour
{
    private static DisplayText textNormalPrefab;

    [SerializeField]
    private Transform displayer;

    private List<TextMeshProUGUI> hitInfoTextList = new List<TextMeshProUGUI>();

    [SerializeField]
    private Image healthBar;

    private static Transform lookCameraTransform;
    private Vector3 vecToCamera;

    public void DisplayHitText(string hitText, int fontSize)
    {
        if (textNormalPrefab == null)
        {
            textNormalPrefab = Resources.Load<DisplayText>("HitText/HitTextNormal");
        }

        GameObject textObj = Instantiate(textNormalPrefab.gameObject);

        float randomValue = 0.4f;
        float randomX = Random.Range(-randomValue, randomValue);
        float randomY = Random.Range(-randomValue, randomValue);
        float randomZ = Random.Range(-randomValue, randomValue);
        Vector3 randomPos = new Vector3(randomX, randomY, randomZ) + (lookCameraTransform.up) + (vecToCamera.normalized * randomValue * 2);

        textObj.transform.position = displayer.transform.position + randomPos;
        DisplayText textComp = textObj.GetComponent<DisplayText>();
        textComp.SetText("<size=" + fontSize + ">" + hitText + "</size>");
    }

    public void SetHealth(float normalizedAmount)
    {
        healthBar.fillAmount = normalizedAmount;
    }

    private void Awake()
    {
        if (lookCameraTransform == null)
        {
            lookCameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        vecToCamera = lookCameraTransform.position - this.transform.position;

        Vector3 vecToLookAt = lookCameraTransform.position;
        displayer.transform.LookAt(vecToLookAt);
    }
}
