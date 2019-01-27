using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour{
    GameObject[,] representitive;
    List<Vector2> doors;
    int[,] mass;
    public CollisionManager()
    {
        doors = new List<Vector2>();
        mass = new int[Constants.MAP_SIZE, Constants.MAP_SIZE];
        representitive = new GameObject[Constants.MAP_SIZE, Constants.MAP_SIZE];
        for(int i = 0;i < Constants.MAP_SIZE;i++)
        {
            for(int j = 0;j < Constants.MAP_SIZE;j++)
            {
                mass[i, j] = 0;
            }
        }
        Debug.Log("done initiation.");
    }
    string playerName = "";
    public void register(GameObject representitive, int x, int y, int givenMass, int doorInfo, string playerName)
    {
        //Debug.Log("#register @ " + x + " , " + y);
        if (doorInfo == Constants.SPRITE_DOOR)
        {
            this.playerName = playerName;
            doors.Add(new Vector2(x, y));
        }
        Debug.Log("given mass " + givenMass);
        mass[x, y] = givenMass;
        this.representitive[x, y] = representitive;
    }
    public int getMass(Vector2 position,string givenPlayerName)
    {
        Debug.Log("GET MASS: " + position);
        if(doors.Contains(position))
        {
            Debug.Log("there is a door @ " + position);
            if (playerName == givenPlayerName)
                return 0;
            else return mass[(int)position.x, (int)position.y];
        }
        return mass[(int)position.x, (int)position.y];
    }
    public void deleteBlock(Vector2 position,string givenPlayerName)
    {
        if (doors.Contains(position))
        {
            
            if (playerName != givenPlayerName)
            {
                Destroy(representitive[(int)position.x, (int)position.y]);
                mass[(int)position.x, (int)position.y] = 0;
            }
            else return;
        }
        Destroy(representitive[(int)position.x, (int)position.y]);
        mass[(int)position.x, (int)position.y] = 0;
    }
}
