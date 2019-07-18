using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleManager : MonoBehaviour
{

    private float paddle1Ran = -1;
    private float paddle2Ran = -1;
    private float paddle3Ran = -1;
    private float paddle4Ran = -1;

    private int paddle1 = -1;
    private int paddle2 = -1;
    private int paddle3 = -1;
    private int paddle4 = -1;
    private int roll;

    private int result = -1;
    private BoardManager boardManager;
    private paddleColor color;
    private paddleColor2 color2;
    private paddleColor3 color3;
    private paddleColor4 color4;

    void Awake()
    {
        boardManager = GameObject.FindObjectOfType<BoardManager>();
        color = GameObject.FindObjectOfType<paddleColor>();
        color2 = GameObject.FindObjectOfType<paddleColor2>();
        color3 = GameObject.FindObjectOfType<paddleColor3>();
        color4 = GameObject.FindObjectOfType<paddleColor4>();
    }

    public void UpdateRoll(int r)
    {
        roll = r;
    }


    private void OnMouseDown()
    {
 
        if (roll==1)
        {
            paddle1Ran = Random.Range(0, 100);
            if (paddle1Ran > 50)
            {
                paddle1 = 1;
            }
            else
            {
                paddle1 = 0;
            }

            paddle2Ran = Random.Range(0, 100);
            if (paddle2Ran > 50)
            {
                paddle2 = 1;
            }
            else
            {
                paddle2 = 0;
            }

            paddle3Ran = Random.Range(0, 100);
            if (paddle3Ran > 50)
            {
                paddle3 = 1;
            }
            else
            {
                paddle3 = 0;
            }

            paddle4Ran = Random.Range(0, 100);
            if (paddle4Ran > 50)
            {
                paddle4 = 1;
            }
            else
            {
                paddle4 = 0;
            }
            result = paddle1 + paddle2 + paddle3 + paddle4;
            boardManager.UpdateResult(result);//passing result to boardManger
            color.UpdateColor(paddle1);
            color2.UpdateColor(paddle2);
            color3.UpdateColor(paddle3);
            color4.UpdateColor(paddle4);
        }
    }     
}
