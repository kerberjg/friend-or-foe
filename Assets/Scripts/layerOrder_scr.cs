using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class layerOrder_scr : MonoBehaviour
{
    SpriteRenderer mSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mSpriteRenderer.sortingOrder = Mathf.FloorToInt(transform.position.y * 1000.0f);
    }
}
