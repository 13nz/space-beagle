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
    [SerializeField] TileBase groundTile;
    [SerializeField] Tilemap groundTilemap;
    [SerializeField] TileBase caveTile;
    [SerializeField] Tilemap caveTilemap;

    [SerializeField] float seed;

    int [,] map;
    // Start is called before the first frame update
    void Start()
    {
        perlinHeightList = new int[width];
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Generate(); 
        }
    }

    void Generate()
    {
        seed = Time.time; 
        ClearMap();
        map = GenerateArray(width, height, true);
        map = TerrainGeneration(map);
        SmoothMap(smoothAmount);
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

            for (int y = 0; y < perlinHeight; y++)
            {
                // Perlin generation
                //int caveVal = Mathf.RoundToInt(Mathf.PerlinNoise((x * mod) + seed, (y * mod) + seed));
                //map[x, y] = (caveVal == 1) ? 2 : 1;

                // Cellular automata
                map[x, y] = (pseudoRandom.Next(1, 100) < randomFillPercent) ? 1 : 2;

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
                else if (map[x, y] == 2)
                {
                    caveTilemap.SetTile(new Vector3Int(x, y, 0), caveTile); 
                }
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
                for (int y = 0; y < perlinHeightList[x]; y++) // loop through perlin heights
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == perlinHeightList[x] - 1)
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
                            map[x, y] = 2;
                        }
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
}
