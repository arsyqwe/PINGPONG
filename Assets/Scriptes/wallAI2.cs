using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class WallAI2 : MonoBehaviour
{
   [NonSerialized] public float wallWidth;
   [NonSerialized] public float wallHeight;
    public float wallSpeed;
    public Ball ball;
    public float estY;
    public bool isLeftWall;
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
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 wallPos = transform.position;
        if (isLeftWall)
        {
            if (ball.lastPosX < 0)
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

            float leftWall = wallPos.x + (wallWidth);
            float wallRight = wallPos.x - (wallWidth);
            float wallTop = wallPos.y + (wallHeight / 2f);
            float wallBottom = wallPos.y - (wallHeight / 2f);
            float xSegment = ball.transform.position.x - (ball.lastPosX * Time.deltaTime * ball.speed);

            if (ball.transform.position.x <= leftWall && xSegment >= leftWall && ball.transform.position.y < wallTop && ball.transform.position.y > wallBottom)
            {
                if (ball.lastPosX < 0)
                {
                    ball.lastPosX = Math.Abs(ball.lastPosX);
                    ball.transform.position = new Vector2(leftWall + 0.1f, ball.transform.position.y);
                }
            }
            if (Math.Abs(ball.transform.position.x) > ball.maxX)
            {
                float randomY = Random.Range(-3f, 3f);
                float dirX;


                if (ball.transform.position.x < ball.minX)
                {
                    dirX = 1f;
                }
                else
                {
                    dirX = -1f;
                }
                if (ball.transform.position.x > 0)
                {
                    leftScore++;
                    UpdateScoreUI(true);
                }
                else
                {
                    rightScore++;
                    UpdateScoreUI(false);
                }

                ball.transform.position = new Vector3(0f, randomY, 0f);

                float dirY = Random.Range(-0.8f, 0.8f);
                Vector2 newDir = new Vector2(dirX, dirY).normalized;

                ball.lastPosX = newDir.x;
                ball.lastPosY = newDir.y;
                ball.gameTime = 0;
                ball.speed = 6f;

            }
        }
        else
        {
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
            float wallTop = wallPos.y + (wallHeight / 2f);
            float wallBottom = wallPos.y - (wallHeight / 2f);
            float wallLeft = wallPos.x - (wallWidth);
            float wallRight = wallPos.x + (wallWidth);
            float xSegment = ball.transform.position.x - (ball.lastPosX * Time.deltaTime * ball.speed);

            if (ball.transform.position.x >= wallLeft && xSegment <= wallLeft && ball.transform.position.y < wallTop && ball.transform.position.y > wallBottom)
            {
                if (ball.lastPosX > 0)
                {
                    ball.lastPosX = -ball.lastPosX;
                    ball.transform.position = new Vector2(wallLeft - 0.1f, ball.transform.position.y);
                }
            }
        }
        if (wallPos.y + 0.1f < estY)
        {
            wallPos.y += Time.deltaTime * wallSpeed;
        }
        else if (wallPos.y - 0.1f > estY)
        {
            wallPos.y -= Time.deltaTime * wallSpeed;
        }


        transform.position = new Vector2(wallPos.x, wallPos.y);
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
