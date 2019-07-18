using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; }
    private bool[,] allowedMoves { set; get; }

    public Chessman[,] Chessmans { set; get; }
    private Chessman selectedChessman;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    private int selectionX2 = -1;
    private int selectionY2 = -1;
    private int paddleResult = -1;
    private int move = 0;
    private int step = 0;
    private int whiteChessPoint = 0;
    private int blackChessPoint = 0;

    private PaddleManager paddleManager;

    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman;

    public bool isWhiteTurn = true;
    public int roll = 1; // 0 for false, 1 for true
    public bool throwAgain = false;


    void Awake()
    {
        paddleManager = GameObject.FindObjectOfType<PaddleManager>();
    }

    private void Start()
    {
        Instance = this;
        SpawnAllChessmans();
        Debug.Log("White start first, please click the leftmost paddle to roll.");
        Debug.Log("If you made a mistake on select piece, click outside chessboard to cancel the selection.");
    }

    private void Update()
    {
        if(whiteChessPoint == 5)
        {
            Debug.Log("White chess wins. Game ended.");
        }
        else if(blackChessPoint == 5)
        {
            Debug.Log("Black chess wins. Game ended.");
        }
        else
        {
            UpdateSelection();

            if (move != 0)
            {
                roll = 0;
            }
            else
            {
                roll = 1;
            }

            paddleManager.UpdateRoll(roll);

            if (Input.GetMouseButtonDown(0))
            {
                if (selectionX >= 0 && selectionY >= 0 && selectionX < 10 && selectionY < 3)
                {
                    
                    if (selectedChessman == null)
                    {
                        // Select the chessman
                        SelectChessman(selectionX, selectionY);
                        selectionX2 = selectionX;
                        selectionY2 = selectionY;
                        Debug.Log("You selected " +Chessmans[selectionX, selectionY]+ " at "+ selectionX + ", " + selectionY);
                    }
                    else
                    {
                        // Move the chessman
                        
                        if (move != 0)
                        {

                            if (selectionX2 + move < 10)
                            {
                                Chessman c = Chessmans[selectionX2 + move, selectionY2];
                                if (c != null && c.isWhite != isWhiteTurn)
                                {

                                    MoveChessman(selectionX2 + move, selectionY2);
                                    selectedChessman = c;
                                    MoveChessmanToNotNull(selectionX2, selectionY2);
                                    selectedChessman = null;
                                }
                                else if (c != null && c.isWhite == isWhiteTurn)
                                {
                                    Debug.Log("Can't land on your own piece. Please select a valid chess.");
                                    selectedChessman = null;
                                }
                                else
                                {
                                    MoveChessman(selectionX2 + move, selectionY2);
                                    
                                    selectedChessman = null;
                                }


                            }
                            else if(selectionY2 - 1>=0)
                            {
                                Chessman c = Chessmans[selectionX2 + move - 10, selectionY2 - 1];
                                if (c != null && c.isWhite != isWhiteTurn)
                                {
                                    MoveChessman(selectionX2 + move - 10, selectionY2 - 1);
                                    selectedChessman = c;
                                    MoveChessmanToNotNull(selectionX2, selectionY2);
                                    selectedChessman = null;
                                }
                                else if (c != null && c.isWhite == isWhiteTurn)
                                {
                                    Debug.Log("Can't land on ur own piece. Please select a vaild chess.");
                                    selectedChessman = null;
                                }
                                else
                                {
                                    MoveChessman(selectionX2 + move - 10, selectionY2 - 1);
                                    
                                    selectedChessman = null;
                                }
                            }
                            else
                            {
                                Chessman c = selectedChessman;
                                if (c.isWhite)
                                {
                                    whiteChessPoint++;
                                }
                                else
                                {
                                    blackChessPoint++;
                                }
                                
                                activeChessman.Remove(c.gameObject);
                                Destroy(c.gameObject);
                                Debug.Log("White has " + whiteChessPoint + " points now.");
                                Debug.Log("Black has " + blackChessPoint + " points now.");
                            }
                        }


                        if (isWhiteTurn)
                        {
                            Debug.Log("White turn now.");
                        }
                        else
                        {
                            Debug.Log("Black turn now.");
                        }
                    }
                }
                else
                {
                    selectedChessman = null;
                }
            }
        }
        
    }

    private void SelectChessman(int x, int y)
    {
        if (Chessmans[x, y] == null)
        {
            Debug.Log("Selected null.");
            return;
        }
            
            

        if (Chessmans[x, y].isWhite != isWhiteTurn)
        {
            Debug.Log("Not your turn.");
            return;
        }

        selectedChessman = Chessmans[x, y];

    }

    private void MoveChessman(int x, int y)
    {
        if (selectedChessman.PossibleMove(x, y))
        {

            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = GetTileCenter(x, y);
            selectedChessman.SetPosition(x, y);

            Chessmans[x, y] = selectedChessman;

            if (!throwAgain)
            {
                isWhiteTurn = !isWhiteTurn;
            }

        }
        move = 0;
        selectedChessman = null;
    }

    private void MoveChessmanToNotNull(int x, int y)
    {
        if (selectedChessman.PossibleMove(x, y))
        {

            selectedChessman.transform.position = GetTileCenter(x, y);
            selectedChessman.SetPosition(x, y);

            Chessmans[x, y] = selectedChessman;

        }
        move = 0;
        selectedChessman = null;
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,25.0f,LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }


    private void SpawnChessman(int index, int x, int y)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], GetTileCenter(x, y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        Chessmans[x, y] = go.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);
    }

    private void SpawnAllChessmans()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[10, 3];

        // Spawn the white team!


        for (int i = 0; i < 10; i+=2)
            SpawnChessman(0, i, 2);

        // Spawn the Black team!


        for (int i = 1; i < 10; i+=2)
            SpawnChessman(1, i, 2);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    public void UpdateResult(int result)
    {
        paddleResult = result;

        if(paddleResult==0)
        {
            move = 6; //4 black
            step = 6;
            throwAgain = true;
            Debug.Log("You can move " + move + ". and reroll the paddle.");
        }
        else if (paddleResult == 1)
        {
            move = 1; //1 white
            step = 1;
            throwAgain = true;
            Debug.Log("You can move " + move + ". and reroll the paddle.");
        }
        else if (paddleResult == 2)
        {
            move = 2; //2 white
            step = 2;
            throwAgain = false;
            Debug.Log("You can move " + move + ".");
        }
        else if (paddleResult == 3)
        {
            move = 3; //3 white
            step = 3;
            throwAgain = false;
            Debug.Log("You can move " + move + ".");
        }
        else if (paddleResult == 4)
        {
            move = 4; //4 white
            step = 4;
            throwAgain = true;
            Debug.Log("You can move " + move + ". and reroll the paddle.");
        }
    }
}
