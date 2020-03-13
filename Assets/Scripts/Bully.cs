using System.Collections;
using UnityEngine;

public class Bully : MonoBehaviour
{
    enum BullyState
    {
        MOVINGWITHBACKGROUND,
        TACKLE,
        MOVETOWARDSTHEPLAYER,
        PASSEDPLAYER
    }

    public enum FaceState
    {
        SHOW_REAL_FACE,
        DONT_SHOW_REAL_FACE
    }

    private Vector3 direction;
    public bool detected = false;

    Transform player;
    Vector3 tacklePosition = Vector3.zero;
    [SerializeField]
    BullyState bullyState = BullyState.MOVINGWITHBACKGROUND;

    public FaceState faceState = FaceState.DONT_SHOW_REAL_FACE;

    public float speed = 1.0f;
    public float moveTowardsPlayerDistance = 1.5f;
    public float bullyDistance = 0.5f;

    [Header("Neutral")]
    public Sprite neutralSprite;
    public Material neutralMaterial;

    [Header("Bully")]
    public Sprite bullySprite;
    public Material bullyMaterial;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (faceState == FaceState.DONT_SHOW_REAL_FACE)
        {
            spriteRenderer.sprite = neutralSprite;
            spriteRenderer.material = neutralMaterial;
        }
        else
        {
            spriteRenderer.sprite = bullySprite;
            spriteRenderer.material = bullyMaterial;
        }

        player = GameObject.Find("Player").transform;
        direction = Vector3.left;
        detected = false;
        bullyState = BullyState.MOVINGWITHBACKGROUND;
        speed = 3.0f;
    }

    private void Update()
    {
        switch (bullyState)
        {
            case BullyState.MOVINGWITHBACKGROUND:
                if (Vector3.Distance(transform.position, player.position) <= 4.0f || detected)
                {
                    bullyState = BullyState.MOVETOWARDSTHEPLAYER;
                }
                break;
            case BullyState.TACKLE:
                spriteRenderer.sprite = bullySprite;
                spriteRenderer.material = bullyMaterial;
                transform.position = Vector3.MoveTowards(transform.position, tacklePosition, Time.deltaTime * speed);
                if (transform.position.x <= player.position.x - bullyDistance)
                {
                    bullyState = BullyState.PASSEDPLAYER;
                }
                break;
            case BullyState.MOVETOWARDSTHEPLAYER:
                spriteRenderer.sprite = bullySprite;
                spriteRenderer.material = bullyMaterial;
                transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * speed);
                if (Vector3.Distance(transform.position, player.position) <= moveTowardsPlayerDistance)
                {
                    Debug.Log("Switching");
                    bullyState = BullyState.TACKLE;
                    direction = (player.position - transform.position)* 2.0f;
                    tacklePosition = player.position + direction;
                }
                break;
            case BullyState.PASSEDPLAYER:
                break;
            default:
                bullyState = BullyState.MOVINGWITHBACKGROUND;
                break;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Play Some Animation.
        }
    }
}
