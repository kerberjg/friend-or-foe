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
    public float speed = 1.0f;
    [Min(0.0f)]
    public float switchSpeed = 1.0f;
    [Min(0.0f)]
    public float moveDelay = 0.5f;

    Character curCharacter = Character.MOVING;
    Transform[] charactersTrans;
    float curMoveDelay = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        charactersTrans = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (curCharacter == Character.MOVING)
        {
            Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);

            if (Input.GetKey(up_k))
            {
                direction.y += 1.0f;
            }
            if (Input.GetKey(down_k))
            {
                direction.y -= 1.0f;
            }
            if (Input.GetKey(left_k))
            {
                direction.x -= 1.0f;
            }
            if (Input.GetKey(right_k))
            {
                direction.x += 1.0f;
            }
            direction.Normalize();

            charactersTrans[0].Translate(direction * speed);
            charactersTrans[2].Translate(-direction * speed);

            if (charactersTrans[2].position.x < charactersTrans[1].position.x - 0.51f ||
                charactersTrans[2].position.x > charactersTrans[1].position.x - 0.49f ||
                charactersTrans[2].position.y < charactersTrans[1].position.y + 0.19f ||
                charactersTrans[2].position.y > charactersTrans[1].position.y + 0.21f)
            {
                curMoveDelay += Time.deltaTime;
            }
            else
            {
                curMoveDelay = 0.0f;
            }

            if (curMoveDelay >= moveDelay)
            {
                Vector3 direction2 = charactersTrans[1].position;
                direction2.x -= 0.5f;
                direction2.y += 0.2f;
                direction2 -= charactersTrans[2].position;

                if (direction2.magnitude > (direction2.normalized * speed).magnitude)
                    charactersTrans[2].Translate(direction2.normalized * speed);
                else
                    charactersTrans[2].Translate(direction2);
            }
        }
        else if (curCharacter == Character.SEEING)
        {

        }

        if (Input.GetKeyDown(switchChar_k))
        {
            if (curCharacter == Character.MOVING)
            {
                curCharacter = Character.SEEING;
            }
            else if (curCharacter == Character.SEEING)
            {
                curCharacter = Character.MOVING;
            }
        }
    }
}
