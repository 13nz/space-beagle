using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


// Background generation:
// https://deep-fold.itch.io/space-background-generator


public class MapGeneration : MonoBehaviour 
{
    [Header("Backgrounds")]
    [SerializeField] GameObject background;
    [SerializeField] Sprite[] backgroundImages;

    [Header("Rooms")]
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] int scaleX;
    [SerializeField] int scaleY;
    [SerializeField] TileBase[] tiles;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Grid mainGrid;
    [SerializeField] GameObject room;
    public static Transform playerSpawnPos;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI lvl;
    [SerializeField] TextMeshProUGUI scr;
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite heart;
    [SerializeField] GameObject mainMenu;

  
    TileBase borderTile;
    //[SerializeField] GameObject exitDoor;

    public GameObject spawn;

    bool exitRoom = false;

    public static bool ready;
    bool gotPlayerPos;

    //[SerializeField] int scale;
    [SerializeField] PauseScreen pauseScreen;
    
    Stack<Generator.Directions> directions;

    int direction; // 0 & 1 == Right, 2 & 3 == Left, 4 == down
    bool finished = false;

    public GameObject[,] roomArray;
    public List<Vector2> loadedRooms;

    void Start()
    {
        Time.timeScale = 1;

        if (PlayerData.level > 1)
        {
            mainMenu.SetActive(false);
        }
        
        borderTile = tiles[Random.Range(0, tiles.Length)];
        width = 3 + PlayerData.level;
        height = 3 + PlayerData.level;
/* 
        SpriteRenderer sr = background.GetComponent<SpriteRenderer>();
        int rand = Random.Range(0, backgroundImages.Length);
        sr.sprite = backgroundImages[rand];
 */
        background.GetComponent<SpriteRenderer>().sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];

        ready = false;
        gotPlayerPos = false;

        //directions = new Stack<Generation.Directions>();
        roomArray = new GameObject[width, height];
        transform.position = new Vector2(0, 0);
        transform.position = new Vector2(Random.Range(0, width), 0);

        
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseScreen.Setup();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Game");
        }

        lvl.text = "Level: " + PlayerData.level.ToString();
        scr.text = "Score: " + PlayerData.score.ToString();

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < PlayerData.lives)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

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
                    exitRoom = true;
                    Destroy(GetRoom(transform.position));
                    CreateRoom(Generator.Directions.LR);
                    exitRoom = false;
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
        initRoom.Init(dir, exitRoom);
        
        initRoom.transform.parent = tilemap.transform;
        int x = (int)transform.position.x;
        int y = -(int)transform.position.y;

        roomArray[x, y] = newRoom;
        loadedRooms.Add(new Vector2(x, y));
        if (!gotPlayerPos)
        {
            GameObject go = Instantiate(spawn, new Vector3(newRoom.transform.localPosition.x + scaleX / 2, newRoom.transform.position.y + scaleY / 2, 0), Quaternion.identity);
            playerSpawnPos = go.transform;
            int spawnX = (int)newRoom.transform.localPosition.x + scaleX / 2;
            int spawnY = (int)newRoom.transform.localPosition.y + scaleY / 2;
            playerSpawnPos.transform.position = new Vector2(spawnX, spawnY);
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
        int borderWidth = width * scaleX + 1 ;
        int borderHeight = height * scaleY + 1;

        int d = (int)Mathf.Sqrt(scaleX * scaleX + scaleY * scaleY);

        int gridX = Mathf.RoundToInt(local.x - 1); //  - scaleX / 2
        int gridY = Mathf.RoundToInt(local.y + scaleY - 1); //  + scaleY / 2

        int endX = gridX + borderWidth ;
        int endY = gridY - borderHeight ;

        tilemap.ClearAllTiles();

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

    public TileBase GetTile()
    {
        return borderTile;
    }

}