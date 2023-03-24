using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MapGeneration : MonoBehaviour 
{
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] int scaleX;
    [SerializeField] int scaleY;
    [SerializeField] TileBase borderTile;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Grid mainGrid;
    public static Transform playerSpawnPos;

    public GameObject spawn;

    public static bool ready;
    bool gotPlayerPos;

    //[SerializeField] int scale;
    [SerializeField] GameObject room;
    Stack<Generator.Directions> directions;

    int direction; // 0 & 1 == Right, 2 & 3 == Left, 4 == down
    bool finished = false;

    public GameObject[,] roomArray;
    public List<Vector2> loadedRooms;

    void Start()
    {
        ready = false;
        gotPlayerPos = false;

        //directions = new Stack<Generation.Directions>();
        roomArray = new GameObject[width, height];
        transform.position = new Vector2(0, 0);
        transform.position = new Vector2(Random.Range(0, width), 0);

        /* if (transform.position.x > scaleX)
        {
            if (transform.position.x >= width - scaleX)
            {
                CreateRoom(Generator.Directions.L);
                
            }
            CreateRoom(Generator.Directions.LR);
        }
        else
        {
            CreateRoom(Generator.Directions.R);
        } */
        
        CreateRoom(Generator.Directions.LR);

        if (transform.position.x == 0)
        {
            direction = 0;
        } 
        else if (transform.position.x == width - 1)
        {
            direction = 2;
        } 
        else
        {
            direction = Random.Range(0, 4);
        }
    }

    void Update()
    {
        
        if (!finished)
        {
            if (direction == 0 || direction == 1) // Right
            {
                if (transform.position.x < width - 1)
                {
                    transform.position += Vector3.right;
                    CreateRoom(Generator.Directions.LR);
                    direction = Random.Range(0, 5);
                    if (direction == 2)
                    {
                        direction = 1;
                    }
                    else if (direction == 3)
                    {
                        direction = 4;
                    }
                }
                else
                {
                    direction = 4;
                }
            }
            else if (direction == 2 || direction == 3) // Left
            {
                if (transform.position.x > 0)
                {
                    transform.position += Vector3.left;
                    CreateRoom(Generator.Directions.LR);
                    direction = Random.Range(0, 5);
                    if (direction == 0)
                    {
                        direction = 2;
                    }
                    else if (direction == 1)
                    {
                        direction = 4;
                    }
                }
                else
                {
                    direction = 4;
                }
            }
            else if (direction == 4) // Down
            {
                if (transform.position.y > -height + 1)
                {
                    Destroy(GetRoom(transform.position));
                    //Generator.Directions dir = (Random.Range(0, 2) == 0) ? Generator.Directions.LRB : Generator.Directions.LRTB;
                    CreateRoom(Generator.Directions.LRTB);
                    transform.position += Vector3.down;
                    int rand = Random.Range(0, 4);
                    if (rand == 0)
                    {
                        if (transform.position.y > -height + 1)
                        {
                            CreateRoom(Generator.Directions.LRTB);
                            transform.position += Vector3.down;
                        }
                        else
                        {
                            direction = 4;
                            return;
                        }
                    }

                    Generator.Directions dir2 = (Random.Range(2, 4) == 3) ? Generator.Directions.LRT : Generator.Directions.LRTB;
                    CreateRoom(Generator.Directions.LRTB);
                    if (transform.position.x == 0)
                        direction = 0;
                    else if (transform.position.x == width - 1)
                        direction = 2;
                    else
                        direction = Random.Range(0, 4);
                }
                else
                {
                    Destroy(GetRoom(transform.position));
                    CreateRoom(Generator.Directions.LR);
                    FillMap();
                    //mainGrid.transform.position = transform.position;
                    //mainGrid.transform.SetParent(transform);
                    //mainGrid.transform.position = new Vector2(0, 0);
                    transform.localPosition = new Vector2(0, 0);
                    //mainGrid.transform.localPosition = transform.localPosition;
                    RenderBorder(transform.localPosition);
                    finished = true;
                    ready = true;
                }
            }

        }   
    }

    GameObject GetRoom(Vector2 pos)
    {
        return roomArray[(int)pos.x, -(int)pos.y];
    }


    void CreateRoom(Generator.Directions dir)
    {
        // 0, 1 > Right
        // 2, 3 > Left
        // 4 > Down
        //Generator.Directions dir;
        GameObject newRoom = Instantiate(room, new Vector3(transform.position.x * scaleX, transform.position.y * scaleY, 0), Quaternion.identity) as GameObject;
        var initRoom = newRoom.GetComponent<Generator>();
        initRoom.Init(dir);
        initRoom.transform.parent = transform;
        int x = (int)transform.position.x;
        int y = -(int)transform.position.y;

        roomArray[x, y] = newRoom;
        loadedRooms.Add(new Vector2(x, y));
        if (!gotPlayerPos)
        {
            Instantiate(spawn, new Vector3(newRoom.transform.position.x / 2, newRoom.transform.position.y / 2, 0), Quaternion.identity);
            playerSpawnPos = newRoom.transform;
            gotPlayerPos = true;
        }
        //directions.Push(dir);
    }

    void FillMap()
    {
        for(int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(!loadedRooms.Contains(new Vector2(x, y)))
                {
                    transform.position = new Vector2(x, -y);
                    CreateRoom(Generator.Directions.LRT);
                }
            }
        }
    }

    void RenderBorder(Vector2 local)
    {
        int borderWidth = width * scaleX - (width - 2);
        int borderHeight = height * scaleY - (height - 2);
/* 
        int max;
        int min;

        if (scaleX > scaleY)
        {
            max = scaleX;
            mainGrid = scaleY;
        }
        else
        {
            max = scaleY;
            min = scaleX;
        }

        var Center = ((max - min) / 2) + min;
        var Extent = (max - min) / 2;
 */

 int d = (int)Mathf.Sqrt(scaleX * scaleX + scaleY * scaleY);

        int gridX = Mathf.RoundToInt(local.x - scaleX / 2 - 3); //  - scaleX / 2
        int gridY = Mathf.RoundToInt(local.y + scaleY / 2 - 2); //  + scaleY / 2

        int endX = gridX + borderWidth ;
        int endY = gridY - borderHeight ;

        // FIX SCALING

        tilemap.ClearAllTiles();
        /* for (int x = 0; x < borderWidth; x++)
        {
            for (int y = 0; y < borderHeight; y++)
            {
                if (x == 0 || x == borderWidth - 1 || y == borderHeight - 1 || y == 0)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), borderTile);
                }
            }
        } */

        for (int x = gridX; x < endX; x++)
        {
            for (int y = gridY; y > endY; y--)
            {
                if (x == gridX || x == endX - 1 || y == gridY || y == endY + 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), borderTile);
                }
            }
        }
    }

}