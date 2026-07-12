using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;
using TMPro;

public class Wall : MonoBehaviour
{
    public float wallWidth;
    public float wallHeight;
    public float wallSpeed = 6f;
    Ball[] balls;
    public bool isLeftWall;
    public Key upKey;
    public Key downKey;

    public TextMeshProUGUI rightScoreText;
    public TextMeshProUGUI leftScoreText;
    public static int leftScore = 0;
    public static int rightScore = 0;
    void Start()
    {
        if (isLeftWall)
        {
            leftScore = 0;
            rightScore = 0;
            UpdateScoreUI(true);
            UpdateScoreUI(false);
        }
        wallWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        wallHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        balls = FindObjectsOfType<Ball>();
    }

   
    void LateUpdate()
    {
        Vector2 wallPos = transform.position;
        
        if (wallPos.y < (balls[0].maxY / 1.29))
        {
            if (Keyboard.current[upKey].isPressed)
            {
                wallPos.y += Time.deltaTime * wallSpeed;
            }
        }
        if (wallPos.y > (balls[0].minY / 1.29))
        {
            if (Keyboard.current[downKey].isPressed)
            {
                wallPos.y -= Time.deltaTime * wallSpeed;
            }
        }
        for (int i = 0; i < balls.Length; i++)
        {

            Ball nextBall = balls[i];
            if (isLeftWall)
            {
                float leftWall = wallPos.x + (wallWidth);
                float wallRight = wallPos.x - (wallWidth);
                float wallTop = wallPos.y + (wallHeight / 2f);
                float wallBottom = wallPos.y - (wallHeight / 2f);
                float xSegment = nextBall.transform.position.x - (nextBall.lastPosX * nextBall.speed * Time.deltaTime);

                

                if (nextBall.transform.position.x <= leftWall && xSegment >= leftWall && nextBall.transform.position.y < wallTop && nextBall.transform.position.y > wallBottom)
                {
                    if (nextBall.lastPosX < 0)
                    {
                        nextBall.lastPosX = Math.Abs(nextBall.lastPosX);
                        nextBall.transform.position = new Vector2(leftWall + 0.1f, nextBall.transform.position.y);
                    }
                }
                if (Math.Abs(nextBall.transform.position.x) > nextBall.maxX)
                {
                    float randomY = Random.Range(-3f, 3f);
                    float dirX;


                    if (nextBall.transform.position.x < nextBall.minX)
                    {
                        dirX = 1f;
                    }
                    else
                    {
                        dirX = -1f;
                    }
                    if (nextBall.transform.position.x > 0)
                    {
                        leftScore++;
                        UpdateScoreUI(true);
                    }
                    else
                    {
                        rightScore++;
                        UpdateScoreUI(false);
                    }
                    nextBall.transform.position = new Vector3(0f, randomY, 0f);
                    float dirY = Random.Range(-0.8f, 0.8f);
                    Vector2 newDir = new Vector2(dirX, dirY).normalized;
                    nextBall.lastPosX = newDir.x;
                    nextBall.lastPosY = newDir.y;
                    nextBall.gameTime = 0;
                    nextBall.speed = 6f;

                }
            }
            else
            {
                float wallTop = wallPos.y + (wallHeight / 2f);
                float wallBottom = wallPos.y - (wallHeight / 2f);
                float wallLeft = wallPos.x - (wallWidth);
                float wallRight = wallPos.x + (wallWidth);

                float xSegment = nextBall.transform.position.x - (nextBall.lastPosX * Time.deltaTime * nextBall.speed);
                Debug.Log("sağ raket" +xSegment);

                if (nextBall.transform.position.x >= wallLeft && xSegment < wallLeft && nextBall.transform.position.y < wallTop && nextBall.transform.position.y > wallBottom)
                {
                    if (nextBall.lastPosX > 0)
                    {
                        nextBall.lastPosX = -nextBall.lastPosX;
                        nextBall.transform.position = new Vector2(wallLeft - 0.1f, nextBall.transform.position.y);
                    }
                }

            }
        }
      
        transform.position = new Vector3(wallPos.x, wallPos.y, 0);
    }
    void UpdateScoreUI(bool isLeftPlayer)
    {
        if (isLeftPlayer && leftScoreText != null)
        {
            leftScoreText.text = "Score: " + leftScore.ToString();
        }
        else if (!isLeftPlayer && rightScoreText != null)
        {
            rightScoreText.text = "Score: " + rightScore.ToString();
        }
    }

}
