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

    private Canvas canvas;

    public void DisplayHitText(string hitText)
    {
        GameObject textObj = Instantiate(textNormalPrefab.gameObject);
        textObj.transform.position = displayer.transform.position;
        DisplayText textComp = textObj.GetComponent<DisplayText>();
        textComp.SetText(hitText);
    }

    private void Awake()
    {
        canvas = displayer.GetComponent<Canvas>();

        if (textNormalPrefab == null)
        {
            textNormalPrefab = Resources.Load<DisplayText>("HitText/HitTextNormal");
        }
    }

    private void Update()
    {
        displayer.transform.LookAt(Camera.main.transform);
    }
}
