using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_scr : MonoBehaviour
{
    public KeyCode up_k = KeyCode.UpArrow;
    public KeyCode down_k = KeyCode.DownArrow;
    public KeyCode left_k = KeyCode.LeftArrow;
    public KeyCode right_k = KeyCode.RightArrow;
    public KeyCode switchChar_k = KeyCode.Z;
    public KeyCode highFive_k = KeyCode.X;

    [Min(0.0f)]
    public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(up_k))
            transform.Translate(0.0f, speed, 0.0f);
        if (Input.GetKey(down_k))
            transform.Translate(0.0f, -speed, 0.0f);
        if (Input.GetKey(left_k))
            transform.Translate(-speed, 0.0f, 0.0f);
        if (Input.GetKey(right_k))
            transform.Translate(speed, 0.0f, 0.0f);
    }
}
