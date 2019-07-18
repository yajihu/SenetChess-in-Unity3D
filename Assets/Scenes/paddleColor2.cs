using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paddleColor2 : MonoBehaviour
{
    private int color;

    // Update is called once per frame
    void Update()
    {

    }

    /*
    private void OnMouseDown()
    {

    }
    */
    public void UpdateColor(int paddle)
    {
        color = paddle;
        //Debug.Log(paddleResult);
        if (color == 1)
        {
            transform.rotation = Quaternion.Euler(90, 0, 0);

        }
        else
        {
            transform.rotation = Quaternion.Euler(270, 0, 0);

        }
    }
}
