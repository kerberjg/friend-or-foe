using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_scr : MonoBehaviour
{
    enum Character
    {
        MOVING,
        SEEING
    }


    public KeyCode up_k = KeyCode.UpArrow;
    public KeyCode down_k = KeyCode.DownArrow;
    public KeyCode left_k = KeyCode.LeftArrow;
    public KeyCode right_k = KeyCode.RightArrow;
    public KeyCode switchChar_k = KeyCode.Z;
    public KeyCode highFive_k = KeyCode.X;

    [Min(0.0f)]
    public float speed = 0.03f;
    [Min(0.0f)]
    public float switchSpeed = 0.03f;
    public float xMin = -7.0f;
    public float xMax = -2.0f;
    public float yMin = -2.0f;
    public float yMax = 2.0f;

    public string foeTag = "Foe";
    public string friendTag = "Friend";

    public GameObject roomScrollerObj;

    Character curCharacter = Character.MOVING;
    bool switchCharacter = false;

    Animator spriteAnimator;


    // Start is called before the first frame update
    void Start()
    {
        spriteAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (curCharacter == Character.MOVING)
        {
            if (switchCharacter)
            {
                spriteAnimator.SetBool("DoSwitch", false);
                roomScrollerObj.GetComponent<CorridorScroller>().scrollSpeed = 3.0f;

                switchCharacter = false;
            }

            Vector3 moveDirection = new Vector3(0.0f, 0.0f, 0.0f);

            if (Input.GetKey(up_k))
            {
                moveDirection.y -= 1.0f;
            }
            if (Input.GetKey(down_k))
            {
                moveDirection.y += 1.0f;
            }
            if (Input.GetKey(left_k))
            {
                moveDirection.x -= 1.0f;
            }
            if (Input.GetKey(right_k))
            {
                moveDirection.x += 1.0f;
            }
            moveDirection.Normalize();

            transform.Translate(moveDirection * speed);

            if (transform.position.x < xMin)
            {
                Vector3 newPos = transform.position;
                newPos.x = xMin;
                transform.position = newPos;
            }
            else if (transform.position.x > xMax)
            {
                Vector3 newPos = transform.position;
                newPos.x = xMax;
                transform.position = newPos;
            }
            if (transform.position.y < yMin)
            {
                Vector3 newPos = transform.position;
                newPos.y = yMin;
                transform.position = newPos;
            }
            else if (transform.position.y > yMax)
            {
                Vector3 newPos = transform.position;
                newPos.y = yMax;
                transform.position = newPos;
            }
        }
        else if (curCharacter == Character.SEEING)
        {
            if (switchCharacter)
            {
                spriteAnimator.SetBool("DoSwitch", false);
                roomScrollerObj.GetComponent<CorridorScroller>().scrollSpeed = 0.0f;

                switchCharacter = false;
            }
        }

        if (Input.GetKeyDown(switchChar_k) && switchCharacter == false)
        {
            if (curCharacter == Character.MOVING)
            {
                curCharacter = Character.SEEING;
            }
            else if (curCharacter == Character.SEEING)
            {
                curCharacter = Character.MOVING;
            }
            switchCharacter = true;
            spriteAnimator.SetBool("DoSwitch", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.gameObject.tag == foeTag)
            {
                // TEST CODE. CHANGE TO DAMAGE AND HIT ANIMATION
                gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
            }
            else if (other.gameObject.tag == friendTag)
            {
                // TEST CODE. CHANGE TO HI-FIVE ANIMATION
                gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(0.0f, 0.0f, 1.0f);
            }
        }
    }
}
