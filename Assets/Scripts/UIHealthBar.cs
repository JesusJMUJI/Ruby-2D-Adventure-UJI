using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }
    
    public Image mask;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI enemyCountTxt;
    public GameObject Ruby;
    public float originalSize;
    public Text healthText;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
        var speed = Ruby.GetComponent<RubyController>().speed;
    }

    private void Update()
    {
        var speed = Ruby.GetComponent<RubyController>().speed;
        speedText.text = "Speed:" + Mathf.Round(speed).ToString();
        healthText.text = Mathf.Round(Ruby.GetComponent<RubyController>().health).ToString();
    }

    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}
