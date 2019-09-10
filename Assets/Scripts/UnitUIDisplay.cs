using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUIDisplay : MonoBehaviour
{
    [SerializeField]
    private Transform displayer;

    private List<TextMeshProUGUI> hitInfoTextList = new List<TextMeshProUGUI>();

    private Canvas canvas;

    public void DisplayHitText(string hitText)
    {

    }

    private void Awake()
    {
        canvas = displayer.GetComponent<Canvas>();
    }

    private void Update()
    {
        displayer.transform.LookAt(Camera.main.transform);
    }
}
