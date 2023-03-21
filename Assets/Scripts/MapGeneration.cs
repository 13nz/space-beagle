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

    //[SerializeField] int scale;
    [SerializeField] GameObject room;
    Stack<Generator.Directions> directions;

    int direction; // 0 & 1 == Right, 2 & 3 == Left, 4 == down
    bool finished = false;

    public GameObject[,] roomArray;
    public List<Vector2> loadedRooms;

    void Start()
    {
        int borderWidth = width * scaleX + 1;
        int borderHeight = height * scaleY + 1;

        tilemap.ClearAllTiles();
        for (int x = 0; x < borderWidth; x++)
        {
            for (int y = 0; y < borderHeight; y++)
            {
                if (x == 0 || x == borderWidth - 1 || y == borderHeight - 1 || y == 0)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), borderTile);
                }
            }
        }

        //directions = new Stack<Generation.Directions>();
        roomArray = new GameObject[width, height];
        transform.position = new Vector2(Random.Range(0, width), 0);
        if (transform.position.x > scaleX)
        {
            if (transform.position.x > width - scaleX)
            {
                CreateRoom(Generator.Directions.L);
            }
            CreateRoom(Generator.Directions.LR);
        }
        else
        {
            CreateRoom(Generator.Directions.R);
        }
        
        //CreateRoom(Generator.Directions.LR);

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
                    Generator.Directions dir = (Random.Range(0, 2) == 0) ? Generator.Directions.LRB : Generator.Directions.LRTB;
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
                    finished = true;
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
        int x = (int)transform.position.x;
        int y = -(int)transform.position.y;

        roomArray[x, y] = newRoom;
        loadedRooms.Add(new Vector2(x, y));
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

}