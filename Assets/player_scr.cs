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

    enum DelayDirection
    {
        IDLE,
        UP,
        UP_RIGHT,
        RIGHT,
        DOWN_RIGHT,
        DOWN,
        DOWN_LEFT,
        LEFT,
        UP_LEFT
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
    bool switchCharacter = false;
    List<DelayDirection> delayDirections = new List<DelayDirection>();


    // Start is called before the first frame update
    void Start()
    {
        charactersTrans = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (curMoveDelay < moveDelay)
        {
            curMoveDelay += Time.deltaTime;
        }
        else
        {
            delayDirections.RemoveAt(0);
        }

        if (curCharacter == Character.MOVING)
        {
            if (switchCharacter)
            {
                delayDirections.Add(DelayDirection.IDLE);

                Vector3 switchDirection = -charactersTrans[1].localPosition;
                Vector3 switchDirection2 = new Vector3(-0.5f, 0.0f, 0.2f) - charactersTrans[2].localPosition;

                if (charactersTrans[1].position.x < -0.01f ||
                    charactersTrans[1].position.x > 0.01f ||
                    charactersTrans[1].position.z < -0.01f ||
                    charactersTrans[1].position.z > 0.01f)
                {
                    if (switchDirection.magnitude > (switchDirection.normalized * switchSpeed).magnitude)
                        charactersTrans[1].Translate(switchDirection.normalized * switchSpeed);
                    else
                    {
                        charactersTrans[1].Translate(switchDirection);
                        switchCharacter = false;
                    }
                }

                if (charactersTrans[2].position.x < -0.51f ||
                    charactersTrans[2].position.x > -0.49f ||
                    charactersTrans[2].position.z < 0.19f ||
                    charactersTrans[2].position.z > 0.21f)
                {
                    if (switchDirection2.magnitude > (switchDirection2.normalized * switchSpeed).magnitude)
                        charactersTrans[2].Translate(switchDirection2.normalized * switchSpeed);
                    else
                        charactersTrans[2].Translate(switchDirection2);
                }
            }
            else
            {
                int newDir = 0;
                if (Input.GetKey(up_k))
                {
                    newDir += 1;
                }
                if (Input.GetKey(down_k))
                {
                    newDir += 2;
                }
                if (Input.GetKey(left_k))
                {
                    newDir += 4;
                }
                if (Input.GetKey(right_k))
                {
                    newDir += 8;
                }

                if (newDir == 1 || newDir == 13)
                {
                    delayDirections.Add(DelayDirection.UP);
                }
                else if (newDir == 2 || newDir == 14)
                {
                    delayDirections.Add(DelayDirection.DOWN);
                }
                else if (newDir == 4 || newDir == 7)
                {
                    delayDirections.Add(DelayDirection.LEFT);
                }
                else if (newDir == 8 || newDir == 11)
                {
                    delayDirections.Add(DelayDirection.RIGHT);
                }
                else if (newDir == 5)
                {
                    delayDirections.Add(DelayDirection.UP_LEFT);
                }
                else if (newDir == 6)
                {
                    delayDirections.Add(DelayDirection.DOWN_LEFT);
                }
                else if (newDir == 9)
                {
                    delayDirections.Add(DelayDirection.UP_RIGHT);
                }
                else if (newDir == 10)
                {
                    delayDirections.Add(DelayDirection.DOWN_RIGHT);
                }
                else
                {
                    delayDirections.Add(DelayDirection.IDLE);
                }

                Vector3 moveDirection = new Vector3(0.0f, 0.0f, 0.0f);
                Vector3 moveDirection2 = new Vector3(0.0f, 0.0f, 0.0f);

                if (Input.GetKey(up_k))
                {
                    moveDirection.z += 1.0f;
                }
                if (Input.GetKey(down_k))
                {
                    moveDirection.z -= 1.0f;
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

                charactersTrans[0].Translate(moveDirection * speed);
                charactersTrans[2].Translate(-moveDirection * speed);

                switch (delayDirections[0])
                {
                    case DelayDirection.UP:
                        moveDirection2.z = 1.0f;
                        break;
                    case DelayDirection.UP_RIGHT:
                        moveDirection2.x = 1.0f;
                        moveDirection2.z = 1.0f;
                        break;
                    case DelayDirection.RIGHT:
                        moveDirection2.x = 1.0f;
                        break;
                    case DelayDirection.DOWN_RIGHT:
                        moveDirection2.x = 1.0f;
                        moveDirection2.z = -1.0f;
                        break;
                    case DelayDirection.DOWN:
                        moveDirection2.z = -1.0f;
                        break;
                    case DelayDirection.DOWN_LEFT:
                        moveDirection2.x = -1.0f;
                        moveDirection2.z = -1.0f;
                        break;
                    case DelayDirection.LEFT:
                        moveDirection2.x = -1.0f;
                        break;
                    case DelayDirection.UP_LEFT:
                        moveDirection2.x = -1.0f;
                        moveDirection2.z = 1.0f;
                        break;
                    default:
                        break;
                }
                moveDirection2.Normalize();

                charactersTrans[2].Translate(moveDirection2 * speed);
            }
        }
        else if (curCharacter == Character.SEEING)
        {
            if (curMoveDelay >= moveDelay)
            {
                delayDirections.Add(DelayDirection.IDLE);
            }

            if (switchCharacter)
            {
                Vector3 direction = new Vector3(-0.5f, 0.0f, 0.2f) - charactersTrans[1].localPosition;
                Vector3 direction2 = -charactersTrans[2].localPosition;

                if (charactersTrans[1].position.x < -0.51f ||
                    charactersTrans[1].position.x > -0.49f ||
                    charactersTrans[1].position.z < 0.19f ||
                    charactersTrans[1].position.z > 0.21f)
                {
                    if (direction.magnitude > (direction.normalized * switchSpeed).magnitude)
                        charactersTrans[1].Translate(direction.normalized * switchSpeed);
                    else
                    {
                        charactersTrans[1].Translate(direction);
                        switchCharacter = false;
                    }
                }

                if (charactersTrans[2].position.x < -0.01f ||
                    charactersTrans[2].position.x > 0.01f ||
                    charactersTrans[2].position.z < -0.01f ||
                    charactersTrans[2].position.z > 0.01f)
                {
                    if (direction2.magnitude > (direction2.normalized * switchSpeed).magnitude)
                        charactersTrans[2].Translate(direction2.normalized * switchSpeed);
                    else
                        charactersTrans[2].Translate(direction2);
                }
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
        }
    }
}
