using System;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public GameObject tile;
    public Sprite[] tiles;

    // Board
    public float spacing;
    public float tileOffsetY;

    public TextAsset template;
    public Vector3 startingPos;
    private String[,] typeBoard;
    private GameObject[,] gameBoard;

    // Constants
    string NULL = "null";
    string TILE = "tile";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        string line = template.text;

        int columns = -1;
        int rows = 1;

        while (line != "")
        {
            if (line[0] == '\n')
            {
                rows += 1;
            }

            if (rows == 1)
            {
                columns += 1;
            }

            line = line.Substring(1);
        }

        Vector3 startingPos = new Vector3((1 - columns) * spacing / 2, (1 - rows) * spacing / 2, 0);
        gameBoard = new GameObject[rows, columns];
        typeBoard = new String[rows, columns];

        line = template.text + " ";

        for (int row = rows - 1; row >= 0; row--)
        {
            for (int column = 0; column < columns; column++)
            {
                if (line[0] == '1')
                {
                    int x = UnityEngine.Random.Range(0, tiles.Length);
                    GameObject tempTile = Instantiate(tile, new Vector3(column * spacing + startingPos.x,
                                                                        row * spacing + startingPos.y - tileOffsetY,
                                                                        0), Quaternion.identity, gameObject.transform);
                    tempTile.GetComponent<SpriteRenderer>().sprite = tiles[x];
                    gameBoard[row, column] = tempTile;
                    typeBoard[row, column] = TILE;
                }
                else
                {
                    typeBoard[row, column] = NULL;
                }
                line = line.Substring(1);

            }
            line = line.Substring(1);
            if (line != "")
            {
                line = line.Substring(1);
            }
        }

        bool posFound = false;
        while (!posFound)
        {
            int xRand = UnityEngine.Random.Range(0, rows);
            int yRand = UnityEngine.Random.Range(0, rows);
            print(xRand);
            print(yRand);

            if (typeBoard[xRand, yRand] == TILE)
            {
                startingPos = new Vector3(gameBoard[xRand, yRand].transform.position.x,
                                          gameBoard[xRand, yRand].transform.position.y,
                                          0);
                posFound = true;
            }
        }
        
        
        PlayerScript player = (PlayerScript) FindFirstObjectByType(typeof(PlayerScript));
        player.transform.position = startingPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
