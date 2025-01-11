using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGenerator : MonoBehaviour
{
    
    private int[,] _maze; //maze grid
    
    public GameObject exitPrefab; //exit prefab
    public Tilemap wallsTilemap; //walls tilemap
    public Tilemap floorTilemap; //floor tilemap
    public TileBase wallTile;
    public TileBase floorTile; 
    public GameObject player; 
    public GameObject keyPrefab;
    public GameObject keyText;
    public int width = 50; //maze width
    public int height = 50; //maze height

    
    
    
    void Start()
    {
        
        GenerateMaze();
        DrawMaze();
        PlaceExit();
        PlaceKey();
        PlacePlayer();
        
    }

    
    
    
    void PlaceKey()
    {
        
        //find a random tile that player can reach
        List<Vector2Int>pathTiles = new List<Vector2Int>();
        
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (_maze[x, y] == 0) // 0 means a tile that player can walk on
                {
                   pathTiles.Add(new Vector2Int(x, y));
                }
            }
        }

        if (pathTiles.Count > 0)
        {
            
            //pick a random tile where player can walk for the key
            Vector2Int keyPosition = pathTiles[Random.Range(0,pathTiles.Count)];
            
            Vector3 worldPosition = wallsTilemap.CellToWorld(new Vector3Int(keyPosition.x, keyPosition.y, 0)) + new Vector3(0.5f, 0.5f, 0);
            
            Instantiate(keyPrefab, worldPosition, Quaternion.identity);
            
        }

    }

    public void ResetMaze()
    {
        
        //clear current tiles
        wallsTilemap.ClearAllTiles();
        floorTilemap.ClearAllTiles();

        //regenerate maze
        GenerateMaze();
        DrawMaze();

        //place exit again
        PlaceExit();
        
        //regenerate the key
        PlaceKey();

        //place the player again
        PlacePlayer();
        
    }
    
    void CarvePath(int x, int y)
    {
        
        //random direction order
        int[] directions = { 0, 1, 2, 3 };
        
        //set current cell as the path
        _maze[x, y] = 0;
        
        ShuffleArray(directions);

        //go through all directions
        foreach (int direction in directions)
        {
            int nx = x, ny = y;

            //determine next cell based on direction
            switch (direction)
            {
                case 0: nx = x + 2; break; //right
                case 1: nx = x - 2; break; //left
                case 2: ny = y + 2; break; //up
                case 3: ny = y - 2; break; //down
            }

            //check if next cell is a wall
            if (nx > 0 && ny > 0 && nx < width - 1 && ny < height - 1 && _maze[nx, ny] == 1)
            {
                
                //make path between current cell and next cell
                _maze[(x + nx) / 2, (y + ny) / 2] = 0;
                
                CarvePath(nx, ny); //recursively make the next path
                
            }
        }
    }
    
    
    void GenerateMaze()
    {
        //initialize maze grid(1 = wall, 0 = path)
        _maze = new int[width, height];

        //fill the maze with walls
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _maze[x, y] = 1; //set all as walls
            }
        }

        //make paths using depth first search
        CarvePath(1, 1);
        
    }


    void DrawMaze()
    {
        //clear tiles
        wallsTilemap.ClearAllTiles();
        floorTilemap.ClearAllTiles();

        //draw the maze tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_maze[x, y] == 1)
                {
                    wallsTilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
                else
                {
                    floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
                }
            }
        }
    }

    
    void PlaceExit()
    {
        //position for the exit
        int exitY = height - 2; //one tile from the top
        int exitX = width - 2; //top right part
        
        
        //check if player can walk the path
        if (_maze[exitX, exitY] == 0)
        {
            //convert tile position
            Vector3 centeredPosition = wallsTilemap.CellToWorld(new Vector3Int(exitX, exitY, 0)) + new Vector3(0.5f, 0.5f, 0);
            
            GameObject exitInstance = Instantiate(exitPrefab, centeredPosition, Quaternion.identity);
            
            AssignWinPanelManager(exitInstance);
            
        }
        else
        {

            //look for the closest tile player can walk on if we couldn't place exit
            bool placed = false;
            
            for (int x = width - 2; x > width / 2 && !placed; x--) //search right
            {
                
                for (int y = height - 2; y > height / 2 && !placed; y--) //search top 
                {
                    
                    // check if player can walk path
                    if (_maze[x, y] == 0) 
                    {
                        
                        Vector3 centeredPosition = wallsTilemap.CellToWorld(new Vector3Int(x, y, 0)) + new Vector3(0.5f, 0.5f, 0);

                        GameObject exitInstance = Instantiate(exitPrefab, centeredPosition, Quaternion.identity);
                        
                        AssignWinPanelManager(exitInstance);
                        
                        placed = true;
                        
                    }
                }
            }
            
        }
    }

    
    void AssignWinPanelManager(GameObject exitInstance)
    {
        ExitScript exitScript = exitInstance.GetComponent<ExitScript>();
        
        if (exitScript != null)
        {
            
            //dynamically assign WinPanelManager script
            WinPanelManager panelManager = FindObjectOfType<WinPanelManager>();
            
            if (panelManager != null)
            {
                exitScript.winPanelManager = panelManager;
            }

            //dynamically assign keyText
            if (keyText != null)
            {
                exitScript.keyText = keyText;
            }

        }

    }
    
    void ShuffleArray(int[] array)
    {
        
        for (int i = 0; i < array.Length; i++)
        {
            int rnd = Random.Range(0, array.Length);
            int tempArray = array[i];
            array[i] = array[rnd];
            array[rnd] = tempArray;
            
        }
        
    }
    
    void PlacePlayer()
    {
        //look for valid position for the player on the bottom left part of the map
        int startX = 1; 
        int startY = 1;

        //check if player can walk the path
        if (_maze[startX, startY] == 0) 
        {
            //center player
            Vector3 centeredPosition = wallsTilemap.CellToWorld(new Vector3Int(startX, startY, 0)) + new Vector3(0.5f, 0.5f, 0);
            
            player.transform.position = centeredPosition;
            
        }
        else
        {

            //search for a near path if we failed to place player before
            bool placed = false;
            
            for (int x = 1; x < width / 2 && !placed; x++) 
            {
                
                for (int y = 1; y < height / 2 && !placed; y++) 
                {
                    
                    //check if player can walk the path
                    if (_maze[x, y] == 0)
                    {
                        
                        //center player
                        Vector3 centeredPosition = wallsTilemap.CellToWorld(new Vector3Int(x, y, 0)) + new Vector3(0.5f, 0.5f, 0);
                        
                        player.transform.position = centeredPosition;
                        
                        placed = true;
                        
                        
                        
                    }
                }
            }
        }
    }
    
    
}