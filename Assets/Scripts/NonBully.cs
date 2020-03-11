using System.Collections;
using UnityEngine;

public class NonBully : MonoBehaviour
{
    public enum FaceState
    {
        SHOW_REAL_FACE,
        DONT_SHOW_REAL_FACE
    }

    enum NonBullyState
    {
        MOVINGWITHBACKGROUND,
        HIGHFIVE,
        MOVEOUTSIDESCREEN
    }

    NonBullyState nonBullyState = NonBullyState.MOVINGWITHBACKGROUND;
    public FaceState faceState = FaceState.DONT_SHOW_REAL_FACE;
    public float speed = 3.0f;

    [Header("Neutral")]
    public Sprite neutralSprite;
    public Material neutralMaterial;

    [Header("Friend")]
    public Sprite friendSprite;
    public Material friendMaterial;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(faceState == FaceState.DONT_SHOW_REAL_FACE)
        {
            spriteRenderer.sprite = neutralSprite;
            spriteRenderer.material = neutralMaterial;
        }
        else
        {
            spriteRenderer.sprite = friendSprite;
            spriteRenderer.material = friendMaterial;
        }
    }

    private void Update()
    {
        if(nonBullyState == NonBullyState.MOVEOUTSIDESCREEN)
        {
            transform.position += Vector3.left * 0.04f;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nonBullyState = NonBullyState.HIGHFIVE;
            // Play High Five Animation Here..
            // Finished HighFive.
            StartCoroutine(FinishedHighFive());
        }
    }

    IEnumerator FinishedHighFive()
    {
        yield return new WaitForSeconds(1.5f);
        nonBullyState = NonBullyState.MOVEOUTSIDESCREEN;
    }
}
