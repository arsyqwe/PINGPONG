using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    public Transform ball;
    public Transform rightWall;
    public Transform leftWall;
    ///////////////////////////////////////////////
    public float gameTime;
    public float ballSpeed;
    public float ballMaxY;
    public float ballMinY;
    public float ballMaxX;
    public float ballMinX;
    private float ballHalfWidth;
    private float ballHalfHeight;
    ///////////////////////////////////////////////
    private float leftWallWidth;
    private float leftWallHeight;
    private float rightWallWidth;
    private float rightWallHeight;
    ///////////////////////////////////////////
    public Vector2 direction = new Vector2(-1f, 0);
    ///////////////////////////////////////////////
    public float wallSpeed;
    ///////////////////////////////////////////////
    public Key leftUp;
    public Key leftDown;
    public bool isRightWallAI;
    public bool isLeftWallAI;
    public Key upKey;
    public Key downKey;
    ///////////////////////////////////////////////
    public TextMeshProUGUI rightScoreText;
    public TextMeshProUGUI leftScoreText;
    public static int leftScore = 0;
    public static int rightScore = 0;
    public float estY;

    void Start()
    {
        direction = direction.normalized;

        leftScore = 0;
        rightScore = 0;
        UpdateScoreUI(true);
        UpdateScoreUI(false);

        leftWallWidth = leftWall.GetComponent<SpriteRenderer>().bounds.size.x;
        leftWallHeight = leftWall.GetComponent<SpriteRenderer>().bounds.size.y;
        
        rightWallWidth = rightWall.GetComponent<SpriteRenderer>().bounds.size.x;
        rightWallHeight = rightWall.GetComponent<SpriteRenderer>().bounds.size.y;

        ballHalfWidth = ball.GetComponent<SpriteRenderer>().bounds.extents.x;
        ballHalfHeight = ball.GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    void Update()
    {
        ballUpdate();
        wallUpdate(true);
        wallUpdate(false);
        if (Keyboard.current.digit1Key.isPressed)
        {
            Application.targetFrameRate = 15;
        }
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

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 start = ball.position;
        Vector3 end = start + (Vector3)(direction * 3f);
        Gizmos.DrawLine(start, end);
    }

    void ballUpdate()
    {
        Vector2 currentPos = ball.position;
        Vector2 nextPos = currentPos + direction * ballSpeed * Time.deltaTime;

        if (gameTime > 3000)
        {
            ballSpeed += 0.05f * Time.deltaTime;
        }

        if (nextPos.y + ballHalfHeight > ballMaxY && direction.y > 0)
        {
            float limitY = ballMaxY - ballHalfHeight; 
            float overshoot = nextPos.y - limitY;     
            
            direction.y = -direction.y;             
            nextPos.y = limitY - overshoot;          
        }
        
        
        if (nextPos.y - ballHalfHeight < ballMinY && direction.y < 0)
        {
            float limitY = ballMinY + ballHalfHeight; 
            float overshoot = limitY - nextPos.y;     
            
            direction.y = -direction.y;             
            nextPos.y = limitY + overshoot;         
        }

        float leftWallFace = leftWall.position.x + (leftWallWidth / 2f) + ballHalfWidth;     
        float rightWallFace = rightWall.position.x - (rightWallWidth / 2f) - ballHalfWidth;

        float leftWallTop = leftWall.position.y + (leftWallHeight / 2f) + ballHalfHeight;
        float leftWallBottom = leftWall.position.y - (leftWallHeight / 2f) - ballHalfHeight;
        
        float rightWallTop = rightWall.position.y + (rightWallHeight / 2f) + ballHalfHeight;
        float rightWallBottom = rightWall.position.y - (rightWallHeight / 2f) - ballHalfHeight;

        Vector2 leftTop = new Vector2(leftWallFace, leftWallTop);
        Vector2 leftBottom = new Vector2(leftWallFace, leftWallBottom);
        Vector2 rightTop = new Vector2(rightWallFace, rightWallTop);
        Vector2 rightBottom = new Vector2(rightWallFace, rightWallBottom);

        bool isHit = false;

        if (direction.x < 0)
        {
            if (LineSegmentIntersection(currentPos, nextPos, leftBottom, leftTop, out Vector2 hitPoint))
            {
                direction = Vector2.Reflect(direction, Vector2.right);
                nextPos = hitPoint + direction * 0.1f;
                isHit = true;
            }
        }
        else if (direction.x > 0)
        {
            if (LineSegmentIntersection(currentPos, nextPos, rightBottom, rightTop, out Vector2 hitPoint))
            {
                direction = Vector2.Reflect(direction, Vector2.left);
                nextPos = hitPoint + direction * 0.1f;
                isHit = true;
            }
        }

        if (isHit == false && nextPos.x < ballMinX) 
        {
            rightScore++;
            UpdateScoreUI(false);
            ResetBall(1f);
            return;
        }
        if (isHit == false && nextPos.x > ballMaxX)
        {
            leftScore++;
            UpdateScoreUI(true);
            ResetBall(-1f);
            return;
        }

        ball.position = nextPos;
        gameTime++;
    }
    
    void wallUpdate(bool isLeftWall)
    {
        Transform currentWall = isLeftWall ? leftWall : rightWall;
        Vector2 wallPos = currentWall.position;

        float currentWallHeight = isLeftWall ? leftWallHeight : rightWallHeight;

        float wallUpperLimit = ballMaxY - (currentWallHeight / 2f);
        float wallLowerLimit = ballMinY + (currentWallHeight / 2f);

        if (isLeftWall)
        {
            if (isLeftWallAI == false)
            {
                if (wallPos.y < wallUpperLimit && Keyboard.current[leftUp].isPressed)
                    wallPos.y += Time.deltaTime * wallSpeed;
                
                if (wallPos.y > wallLowerLimit && Keyboard.current[leftDown].isPressed)
                    wallPos.y -= Time.deltaTime * wallSpeed;
            }
            else
            {
                if (direction.x < 0)
                {
                    float distance = Math.Abs(wallPos.x - ball.position.x);
                    float velocity = Math.Abs(direction.x * ballSpeed);
                    float timeToReach = distance / velocity;
                    float aiPred = ball.transform.position.y + (timeToReach * ballSpeed * direction.y);
                    estY = Mathf.Clamp(aiPred, wallLowerLimit, wallUpperLimit);
                }
                else estY = 0;

                if (wallPos.y + 0.1f < estY) wallPos.y += Time.deltaTime * wallSpeed;
                else if (wallPos.y - 0.1f > estY) wallPos.y -= Time.deltaTime * wallSpeed;
            }
        }
        else
        {
            if (isRightWallAI == false)
            {
                if (wallPos.y < wallUpperLimit && Keyboard.current[upKey].isPressed)
                    wallPos.y += Time.deltaTime * wallSpeed;
                
                if (wallPos.y > wallLowerLimit && Keyboard.current[downKey].isPressed)
                    wallPos.y -= Time.deltaTime * wallSpeed;
            }
            else
            {
                if (direction.x > 0)
                {
                    float distance = Math.Abs(wallPos.x - ball.transform.position.x);
                    float velocity = Math.Abs(direction.x * ballSpeed);
                    float timeToReach = distance / velocity;
                    float aiPred = ball.transform.position.y + (timeToReach * ballSpeed * direction.y);
                    estY = Mathf.Clamp(aiPred, wallLowerLimit, wallUpperLimit);
                }
                else estY = 0;

                if (wallPos.y + 0.1f < estY) wallPos.y += Time.deltaTime * wallSpeed;
                else if (wallPos.y - 0.1f > estY) wallPos.y -= Time.deltaTime * wallSpeed;
            }
        }  

        if (wallPos.y > wallUpperLimit) 
        {
            wallPos.y = wallUpperLimit;
        }
        else if (wallPos.y < wallLowerLimit)
        {
            wallPos.y = wallLowerLimit;
        } 
        currentWall.position = new Vector2(wallPos.x, wallPos.y);
    }

    void ResetBall(float dirX)
    {
        float randomY = Random.Range(-3f, 3f);
        ball.position = new Vector3(0f, randomY, 0f);
        direction = new Vector2(dirX, Random.Range(-0.8f, 0.8f)).normalized;
        gameTime = 0;
        ballSpeed = 6f;
    }

    bool LineSegmentIntersection(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2, out Vector2 intersectionPoint)
    {
        Vector2 line1 = end1 - start1;
        Vector2 line2 = end2 - start2;
        float determinant = line1.x * line2.y - line1.y * line2.x;

        if (determinant == 0f)
        {
            intersectionPoint = Vector2.zero;
            return false;
        }

        float rat2 = ((start2.x - start1.x) * line1.y - (start2.y - start1.y) * line1.x) / determinant;
        float rat1 = ((start2.x - start1.x) * line2.y - (start2.y - start1.y) * line2.x) / determinant;

        if (0 <= rat2 && rat2 <= 1 && 0 <= rat1 && rat1 <= 1)
        {
            intersectionPoint = start1 + rat1 * line1;
            return true;
        }

        intersectionPoint = Vector2.zero;
        return false;
    }
}