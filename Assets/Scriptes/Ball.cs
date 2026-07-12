using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float gameTime;
    public float speed;
    public float lastPosX;
    public float lastPosY;
    public float maxY;
    public float minY;
    public float maxX;
    public float minX;
    public Vector2 startDirection = new Vector2(-1f, 0f);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPosX = startDirection.x;
        lastPosY = startDirection.y;
        float hipo = Mathf.Sqrt((lastPosX * lastPosX) + (lastPosY * lastPosY));
        if (hipo > 1)
        {
            lastPosX = lastPosX / hipo;
            lastPosY = lastPosY / hipo;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 ballPos = transform.position;
        if (gameTime > 3000)
        {
            speed += 0.0003f;
        }
        ballPos.x += lastPosX * Time.deltaTime * speed;
        ballPos.y += lastPosY * Time.deltaTime * speed;
        if (ballPos.y > maxY && lastPosY > 0)
        {
            lastPosY = -lastPosY;
            ballPos.y = maxY;
        }
        if (ballPos.y < minY && lastPosY < 0)
        {
            lastPosY = -lastPosY;
            ballPos.y = minY;
        }
        transform.position = new Vector2(ballPos.x, ballPos.y);
        gameTime++;
       // Debug.Log(speed);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 start = transform.position;
        Vector3 end = 3f * new Vector3(lastPosX, lastPosY, 0f) + start;

        Gizmos.DrawLine(start, end);

    }
}
