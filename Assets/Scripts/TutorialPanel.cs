using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0.0f;
    }

    void Update()
    {
        if(Input.anyKeyDown)
        {
            Time.timeScale = 1.0f;
            this.gameObject.SetActive(false);
        }
    }
}
