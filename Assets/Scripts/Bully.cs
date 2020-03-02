using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bully : MonoBehaviour
{
    enum BullyState
    {
        MOVINGWITHBACKGROUND,
        TACKLE,
        MOVETOWARDSTHEPLAYER,
        DODGED
    }
        
    private Vector3 direction;
    public bool detected = false;

    Transform player;
    Transform parent;
    Vector3 tacklePosition = Vector3.zero;
    [SerializeField]
    BullyState bullyState = BullyState.MOVINGWITHBACKGROUND;

    public float speed = 3.0f;
    void Start()
    {
        player = GameObject.Find("Player").transform;
        direction = Vector3.left;
        detected = false;
        bullyState = BullyState.MOVINGWITHBACKGROUND;
        parent = transform.parent;
        speed = 3.0f;
    }

    private void Update()
    {
        switch (bullyState)
        {
            case BullyState.MOVINGWITHBACKGROUND:
                if(Vector3.Distance(transform.position, player.position) <= 4.0f || detected)
                {
                    bullyState = BullyState.MOVETOWARDSTHEPLAYER;
                }
                break;
            case BullyState.TACKLE:
                // play tackle animation or something.
                //transform.position = Vector3.Lerp(transform.position, transform.position + direction, Time.deltaTime * tackleSpeed);
                transform.position = Vector3.MoveTowards(transform.position, tacklePosition, Time.deltaTime * speed);
                if (transform.position.x <= player.position.x - 0.5f)
                {
                    transform.parent = parent;
                    bullyState = BullyState.DODGED;
                }
                break;
            case BullyState.MOVETOWARDSTHEPLAYER:
                //direction = (player.position - transform.position).normalized;
                transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * speed);
                if (Vector3.Distance(transform.position, player.position) <= 1.5f)
                {
                    bullyState = BullyState.TACKLE;
                    direction = (player.position - transform.position).normalized * speed;
                    tacklePosition = player.position + direction;
                    this.transform.parent = null;
                }
                break;
            case BullyState.DODGED:
                Destroy(this.gameObject);
                break;
            default:
                bullyState = BullyState.MOVINGWITHBACKGROUND;
                break;
        }
    }
}
