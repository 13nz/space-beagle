using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// https://blog.unity.com/technology/procedural-patterns-you-can-use-with-tilemaps-part-i

public class Generator : MonoBehaviour
{
    [Header("Terrain")]
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float smooth;
    int[] perlinHeightList;

    [Header("Cave")]
    // Perlin
    //[Range(0, 1)]
    //[SerializeField] float mod;

    // Cellular automata
    [Range(0, 100)]
    [SerializeField] int randomFillPercent;
    [SerializeField] int smoothAmount;

    [Header("Tiles")]
    //[SerializeField] TileBase[] tiles;
    [SerializeField] TileBase groundTile;
    [SerializeField] Tilemap groundTilemap;
    [SerializeField] TileBase caveTile;
    [SerializeField] Tilemap caveTilemap;
    [SerializeField] Tilemap decorationsTilemap;
    [SerializeField] TileBase decoTile;
    

    private Dictionary<int[,], GameObject> tileDict;

    [SerializeField] float seed;
    public enum Directions {
        R,
        L,
        LR,
        LRT,
        LRB,
        LRTB
    }

    [SerializeField] GameObject enemy;
    [SerializeField] GameObject door;
    [SerializeField] GameObject[] collectables;
    [SerializeField] GameObject heart;

    private int enemyCount = 0;
    private int collectablesCount = 0;

    public Directions direction;

    public int [,] map;
    // Start is called before the first frame update
    void Start()
    {
        MapGeneration mapgen = transform.parent.parent.GetChild(1).GetComponent<MapGeneration>();
        groundTile = mapgen.GetTile();
        //tileDict = new Dictionary<int[,], Vector2>;
        perlinHeightList = new int[width];
        Generate();
    }

    public Generator Init(Directions dir, bool isExit)
    {
        direction = dir;
        if (isExit && door != null)
        {
            StartCoroutine(AddDoor());
        }
        
        return this;
    }

    // Update is called once per frame
    void Update()
    {
        /* if (Input.GetKeyDown(KeyCode.Space))
        {
            Generate(); 
        } */
    }

    void Generate()
    {
        seed = Random.Range(-10000, 10000); 
        ClearMap();
        map = GenerateArray(width, height, true);
        map = TerrainGeneration(map);
        SmoothMap(smoothAmount);
        FixDirection();
        AddEnemies();
        AddCollectables();
        if (Random.Range(0, 5) == 1)
        {
            AddHeart();
        }
        
        RenderMap(map, groundTilemap, caveTilemap, groundTile, caveTile);
        
    }

    public int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x,y] = (empty) ? 0 : 1;
            }
        }

        return map;
    }

    public int[,] TerrainGeneration(int[,] map)
    {
        // random values according to seed
        System.Random pseudoRandom = new System.Random(seed.GetHashCode()); 

        int perlinHeight;
        for (int x = 0; x < width; x++)
        {
            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smooth, seed) * height / 2);
            perlinHeight += height / 2;
            perlinHeightList[x] = perlinHeight;

            for (int y = 0; y < height; y++)
            {
                // Perlin generation
                //int caveVal = Mathf.RoundToInt(Mathf.PerlinNoise((x * mod) + seed, (y * mod) + seed));
                //map[x, y] = (caveVal == 1) ? 2 : 1;

                // Cellular automata
                map[x, y] = (pseudoRandom.Next(1, 140) < randomFillPercent) ? 1 : 2;
                if (y < height / 2 + 2 && y > height / 2 - 2)
                {
                    map[x, y] = 0;
                }

            }
        }

        return map;
    }

    public void RenderMap(int[,] map, Tilemap groundTilemap, Tilemap caveTilemap, TileBase groundTile, TileBase caveTilebase)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x,y] == 1)
                {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), groundTile );
                }
                // Add decorations
                if (y > 0)
                {
                    if (map[x, y] == 0 && map[x, y - 1] == 1 && (Random.Range(0, 10) == 1))
                    {
                        decorationsTilemap.SetTile(new Vector3Int(x, y, 0), decoTile);
                    }
                }
                /* else if (map[x, y] == 2)
                {
                    caveTilemap.SetTile(new Vector3Int(x, y, 0), caveTile); 
                } */
            }
        }
    }

    void ClearMap()
    {
        groundTilemap.ClearAllTiles();
        caveTilemap.ClearAllTiles();
    }

    // Cellular automata generation
    public void SmoothMap(int smoothAmount)
    {
        for (int i = 0; i < smoothAmount; i++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++) // loop through perlin heights
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    {
                        map[x, y] = 1;
                    }
                    else 
                    {
                        int surrounding = GetSurroundingCount(x, y);
                        if (surrounding > 4)
                        {
                            map[x, y] = 1;
                        } 
                        else if (surrounding < 4)
                        {
                            map[x, y] = 0;
                        }
                    }
                }
            }
        }
    }

    public void FixDirection()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (direction == Directions.R)
                {
                    if (x >= width / 2 && y <= height / 2 + 2 && y >= height / 2 - 2)
                    {
                        map[x, y] = 0;
                    }
                }
                else if (direction == Directions.L)
                {
                    if (x <= width / 2 && y <= height / 2 + 2 && y >= height / 2 - 2)
                    {
                        map[x, y] = 0;
                    }
                }
                else if (direction == Directions.LR)
                {
                    if ((x <= width / 2 - 2 || x >= width / 2 + 2) && y <= height / 2 + 2 && y >= height / 2 - 2)
                    {
                        map[x, y] = 0;
                    }
                }
                else if (direction == Directions.LRT)
                {
                    if ((y >= height / 2 && x <= width / 2 + 2 && x >= width / 2 - 2) || ( y <= height / 2 + 2 && y >= height / 2 - 2))
                    {
                        map[x, y] = 0;
                    }
                }
                else if (direction == Directions.LRB)
                {
                    if ((y >= height / 2 && x <= width / 2 + 2 && x >= width / 2 - 2) ||  (y <= height / 2 + 2 && y >= height / 2 - 2))
                    {
                        map[x, y] = 0;
                    }
                }
                else if (direction == Directions.LRTB)
                {
                    //if ((((y < height / 2 - 2 || y > height / 2 + 2) && x < width / 2 + 2 && x > width / 2 - 2)) ||  (y < height / 2 + 2 && y > height / 2 - 2))
                    if (((y >= height / 2 - 2 && y <= height / 2 + 2 && (x <= width / 2 || x >= width - width / 2)) || ((y <= height / 2 || y >= height - height / 2) && x <= width / 2 + 2 && x >= width / 2 - 2)) )
                    {
                        map[x, y] = 0;
                    }
                }

            }
        }
    }

    public int GetSurroundingCount(int gridX, int gridY)
    {
        int count = 0;

        for (int nx = gridX - 1; nx <= gridX + 1; nx++)
        {
            for (int ny = gridY - 1; ny <= gridY + 1; ny++)
            {
                // if we are inside map
                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    // exclude center tile
                    if (nx != gridX || ny != gridY)
                    {
                        if (map[nx, ny] == 1)
                        {
                            count++;
                        }
                    }

                }
            }
        }
        return count;
    }

    public IEnumerator AddDoor()
    {
        yield return new WaitUntil(() => door != null);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (y > 0)
                {
                    if (map[x, y] == 0 && map[x, y - 1] == 1)
                    {
                        GameObject exitDoor = Instantiate(door, new Vector3Int(x, y, 0), Quaternion.identity);
                        exitDoor.transform.parent = transform;
                        exitDoor.transform.localPosition = new Vector2(x, y);
                        yield break;
                    }
                }
                
            }
        }
    }

    public void AddEnemies()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (y > 0)
                {
                    if (map[x, y] == 0 && map[x, y - 1] == 1 && (Random.Range(0, 10) == 1) && enemyCount < 3 && GetSurroundingCount(x, y) < 4 )
                    {
                        GameObject en = Instantiate(enemy, new Vector2(x, y), Quaternion.identity);
                        en.transform.parent = transform;
                        en.transform.localPosition = new Vector2(x, y);
                        enemyCount++;
                    }
                }
                
            }
        }
    }

    public void AddCollectables()
    {
        for (int x = 1; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 0 && (Random.Range(0, 10) == 1) && collectablesCount < 1 && GetSurroundingCount(x, y) == 0)
                {
                    int r = Random.Range(0, collectables.Length);
                    GameObject go = Instantiate(collectables[r], new Vector2(x, y), Quaternion.identity);
                    go.transform.parent = transform;
                    go.transform.localPosition = new Vector2(x, y);
                    collectablesCount++;
                    x += 2;
                }
                
            }
        }
    }

    public void AddHeart()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 0 && (Random.Range(0, 50) == 1) && GetSurroundingCount(x, y) == 0)
                {
                    GameObject go = Instantiate(heart, new Vector2(x, y), Quaternion.identity);
                    go.transform.parent = transform;
                    go.transform.localPosition = new Vector2(x, y);
                    return;
                }
                
            }
        }
    }
}
