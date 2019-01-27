using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour {

    List<Vector2Int> homeBody,permenant;
    OverallTileManager tileManager;
    BedManager bedManager;
    List<int> direction;
    int perimeter = 0;
    private int[,,] directions;
    private int[,] searchDirections;
    float maxArea;
    bool isManual;
    Vector2 bedPoint;
    public void HMBegin(int perimeter,bool useManual)
    {
        homeBody = new List<Vector2Int>();
        direction = new List<int>();
        this.perimeter = perimeter;
        isManual = useManual;
    }
    public void HMPoint(int x, int y)
    {
        homeBody.Add(new Vector2Int(x, y));
    }
    public void HMPoint(int x,int y,int direction)
    {
        homeBody.Add(new Vector2Int(x, y));
        this.direction.Add(direction);
    }
    public void HMBedPoint(Vector2 position)
    {
        bedPoint = position;
    }
    public void HMEnd(string playerName)
    {
        if(!isManual)
        {
            int index = (int)(perimeter / 2);
            maxArea = (Mathf.Pow((float)perimeter, 2) / (4 * Mathf.PI));
            if (index < 0)
            {
                Debug.LogError("Error: Not long enough");
                return;
            }
            int fx, fy;
            fx = fy = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    fx = homeBody[index].x + directions[direction[index], i, 0] * j;
                    fy = homeBody[index].y + directions[direction[index], i, 1] * j;
                    if (homeBody.Contains(new Vector2Int(fx, fy))) continue;
                    Debug.Log("SEARCHING... @ " + fx + " , " + fy);
                    if (checkRoom(fx, fy))
                    {
                        Debug.Log("found indoor point @ " + fx + " , " + fy);

                        BFSFill(fx, fy);
                        bedManager.registerBed(new Vector2(fx, fy), playerName);

                        return;
                    }
                }

            }
        }
        else
        {
            BFSFill((int)bedPoint.x, (int)bedPoint.y);
            bedManager.registerBed(bedPoint, playerName);
        }
        Debug.Log("Illegal home.");
    }
    private void BFSFill(int fx, int fy)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        bool[,] hasVisisted = new bool[Constants.MAP_SIZE, Constants.MAP_SIZE];
        hasVisisted[fx, fy] = true;
        queue.Enqueue(new Vector2Int(fx, fy));
        Instantiate(tileManager.avaliableSpritePrefabs[Constants.SPRITE_FLOOR], new Vector3(fx, fy, 0), transform.rotation);
        Vector2Int front;
        int nx, ny;
        while (queue.Count > 0)
        {
            front = queue.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                nx = front.x + searchDirections[i, 0];
                ny = front.y + searchDirections[i, 1];
                if (nx > -1 && ny > -1 && nx < Constants.MAP_SIZE && ny < Constants.MAP_SIZE
                    && !homeBody.Contains(new Vector2Int(nx, ny)) && !permenant.Contains(new Vector2Int(nx, ny))
                    && !hasVisisted[nx, ny])
                {
                    queue.Enqueue(new Vector2Int(nx, ny));
                    hasVisisted[nx, ny] = true;
                    Instantiate(tileManager.avaliableSpritePrefabs[Constants.SPRITE_FLOOR], new Vector3(nx, ny, 0), transform.rotation);
                }
            }
        }
    }
    private bool checkRoom(int fx, int fy)
    {
        int cumulatedArea = 0;
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        bool[,] hasVisisted = new bool[Constants.MAP_SIZE, Constants.MAP_SIZE];
        hasVisisted[fx, fy] = true;
        queue.Enqueue(new Vector2Int(fx, fy));
        Vector2Int front;
        int nx, ny;
        while(queue.Count > 0)
        {
            front = queue.Dequeue();
            if (cumulatedArea > maxArea)
            {
                Debug.Log("get cumulated Area: " + cumulatedArea);
                return false;
            }
                
            for(int i = 0;i < 4;i++)
            {
                nx = front.x + searchDirections[i,0];
                ny = front.y + searchDirections[i, 1];
                if(nx > -1 && ny > -1 && nx < Constants.MAP_SIZE && ny < Constants.MAP_SIZE 
                    && !homeBody.Contains(new Vector2Int(nx,ny)) && !permenant.Contains(new Vector2Int(nx, ny))
                    && !hasVisisted[nx,ny])
                {
                    queue.Enqueue(new Vector2Int(nx, ny));
                    hasVisisted[nx, ny] = true;
                    cumulatedArea++;
                }
            }
        }
        Debug.Log("get cumulated Area: "+cumulatedArea);
        if (cumulatedArea <= maxArea)
            return true;
        return false;
    }

    // Use this for initialization
    void Start () {
        bedManager = GetComponent<BedManager>();
        permenant = new List<Vector2Int>();
        tileManager = gameObject.GetComponent<OverallTileManager>();
        searchDirections = new int[4, 2] { {0,1}, {1,0}, {0,-1}, {-1,0} };
        directions = new int[4, 2, 2] { { { -1, 0 }, { 1, 0 } }, { { 0, 1 }, { 0, -1 } }, { { 1, 0 }, { -1, 0 } }, { { 0, -1 }, { 0, 1 } } };
        // homeBody = new List<Vector2Int>();
    }
}
