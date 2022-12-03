using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject dialogueBox;
    private float timerDisplay;

    private void Start()
    {
        dialogueBox.SetActive(false);
        timerDisplay = -1.0f;
    }

    private void Update()
    {
        if (timerDisplay > 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay <= 0)
            {
                dialogueBox.SetActive(false);
            }
        }
    }
    
    public void DisplayDialogue()
    {
        dialogueBox.SetActive(true);
        timerDisplay = displayTime;
    }
}
