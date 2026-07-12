using System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using TMPro;


public class Wall2 : MonoBehaviour
{
    public float wallWidth;
    public float wallHeight;
    public float wallSpeed = 6f;  
    Ball[] multBall;
    void Start()
    {
        
        
       
        wallWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        wallHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        multBall = FindObjectsOfType<Ball>();      
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 wallPos = transform.position;

        if (wallPos.y < (multBall[0].maxY / 1.29f))
        {
            if (Keyboard.current.upArrowKey.isPressed)
            {
                wallPos.y += Time.deltaTime * wallSpeed;
            }
        }
        if (wallPos.y > (multBall[0].minY / 1.29f))
        {
            if (Keyboard.current.downArrowKey.isPressed)
            {
                wallPos.y -= Time.deltaTime * wallSpeed;
            }
        }
        for (int i = 0; i < multBall.Length; i++)
        {
            Ball nextBall = multBall[i];
         
            float wallTop = wallPos.y + (wallHeight / 2f);
            float wallBottom = wallPos.y - (wallHeight / 2f);
            float wallLeft = wallPos.x - (wallWidth);
            float wallRight = wallPos.x + (wallWidth);

            float xSegment = nextBall.transform.position.x - (nextBall.lastPosX* Time.deltaTime * nextBall.speed);
           

            if (nextBall.transform.position.x >= wallLeft && xSegment < wallLeft && nextBall.transform.position.y < wallTop && nextBall.transform.position.y > wallBottom)
            {
                if (nextBall.lastPosX > 0)
                {
                    nextBall.lastPosX = -nextBall.lastPosX;
                    nextBall.transform.position = new Vector2(wallLeft-0.1f  , nextBall.transform.position.y);
                }
            }          
        }
        transform.position = new Vector2(wallPos.x, wallPos.y);
    }

}
