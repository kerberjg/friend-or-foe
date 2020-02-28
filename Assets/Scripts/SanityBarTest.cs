using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityBarTest : MonoBehaviour
{
    public Image fill;
    [Range(0.0f, 100.0f)]
    public float health = 100.0f;
    public float maxHealth = 100.0f;

    private void Start()
    {
        health = 100.0f;
        maxHealth = 100.0f;
    }

    void Update()
    {
        fill.fillAmount = health / maxHealth;
    }
}
