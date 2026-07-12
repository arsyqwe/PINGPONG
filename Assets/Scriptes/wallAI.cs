using System;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class wallAI : MonoBehaviour
{
    public float wallWidth;
    public float wallHeight;
    public float wallSpeed = 6f;
    public Ball ball;
   
    public float estY;



    void Start()
    {
        
        wallWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        wallHeight = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    
    void LateUpdate()
    {
        Vector2 wallPos= transform.position;
        if (ball.lastPosX > 0)
        {
            float distance = (Math.Abs(wallPos.x - ball.transform.position.x));
            float velocity = (Math.Abs(ball.lastPosX * ball.speed));
            float timeToReach = distance / velocity;
            float aiPred = ball.transform.position.y + (timeToReach * ball.speed * ball.lastPosY);
            estY = Math.Clamp(aiPred, ball.minY / 1.30f, ball.maxY / 1.30f);
        }
        else
        {
            estY = 0;
        }

        if (wallPos.y + 0.1f < estY)
        {
            wallPos.y += Time.deltaTime * wallSpeed;
        }
        else if (wallPos.y - 0.1f > estY)
        {
            wallPos.y -= Time.deltaTime * wallSpeed;
        }


        float wallTop = wallPos.y + (wallHeight / 2f);
        float wallBottom = wallPos.y - (wallHeight / 2f);
        float wallLeft = wallPos.x - (wallWidth);
        float wallRight = wallPos.x + (wallWidth);
        float xSegment = ball.transform.position.x - (ball.lastPosX * Time.deltaTime * ball.speed);

        if (ball.transform.position.x >= wallLeft &&  xSegment <= wallLeft && ball.transform.position.y < wallTop && ball.transform.position.y > wallBottom)
        {
            if (ball.lastPosX > 0)
            {
                ball.lastPosX = -ball.lastPosX;
                ball.transform.position = new Vector2(wallLeft -0.1f, ball.transform.position.y);
            }
        }
        transform.position = new Vector2(wallPos.x, wallPos.y);
    }
   
    
}
