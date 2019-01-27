using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallTileManager : MonoBehaviour {

    // Use this for initialization
    GameObject[][] tileMapGameObject;

    int[][] tileMap;
    public GameObject[] avaliableSpritePrefabs;
	void Start () {
        tileMapGameObject = new GameObject[Constants.MAP_SIZE][];
        tileMap = new int[Constants.MAP_SIZE][];
        for(int i = 0;i < Constants.MAP_SIZE; i++)
        {
            tileMapGameObject[i] = new GameObject[Constants.MAP_SIZE];
            tileMap[i] = new int[Constants.MAP_SIZE];
            for(int j = 0;j < Constants.MAP_SIZE; j++)
            {
                int x = Random.Range(0, 200);
                if (x < 5)
                    tileMap[i][j] = Constants.SPRITE_WOOD;
                else if (10 <= x && x <= 16)
                    tileMap[i][j] = Constants.SPRITE_STONE;
                else if (17 <= x && x <= 20)
                    tileMap[i][j] = Constants.SPRITE_DOOR;
                else continue;
                tileMapGameObject[i][j] = GameObject.Instantiate(avaliableSpritePrefabs[tileMap[i][j]], position: new Vector3(i, j, 0), rotation: transform.rotation);
            }
        }
	}
    float frameTime = 1f,
        lastTimeMark = 0f;
	// Update is called once per frame
	void Update () {
		if(Time.time - lastTimeMark > frameTime)
        {
            //deploy(100, 100, Random.Range(1, Constants.MAX_SPRITE_ID));
            lastTimeMark = Time.time;
        }
	}
    /*public void deploy(Vector2 position, int objectID,bool isBody)
    {
        //Debug.Log("deploy OID " + objectID + " @ " + position);
        if (objectID > Constants.MAX_SPRITE_ID)
            return;
        tileMap[(int)position.x][(int)position.y] = objectID;
        if (isBody) tileMap[(int)position.x][(int)position.y] *= 10;
        tileMap[(int)position.x][(int)position.y] = objectID;
        Destroy(tileMapGameObject[(int)position.x][(int)position.y]);
        tileMapGameObject[(int)position.x][(int)position.y] = GameObject.Instantiate(avaliableSpritePrefabs[objectID], 
            position: new Vector3((int)position.x, (int)position.y, 0), 
            rotation: transform.rotation);
    }
    public void deploy(Vector2 position, int objectID, float stayTime)
    {
        //Debug.Log("deploy OID " + objectID + " @ " + position);
        if (objectID > Constants.MAX_SPRITE_ID)
            return;
        Destroy(tileMapGameObject[(int)position.x][(int)position.y]);
        tileMapGameObject[(int)position.x][(int)position.y] = GameObject.Instantiate(avaliableSpritePrefabs[objectID],
            position: new Vector3((int)position.x, (int)position.y, 0),
            rotation: transform.rotation);
        Destroy(tileMapGameObject[(int)position.x][(int)position.y],stayTime);
    }
    public void deploy(int i, int j, int objectID, bool isBody)
    {
        if (objectID > Constants.MAX_SPRITE_ID)
            return;
        if(tileMapGameObject[i][j] != null)
        Destroy(tileMapGameObject[i][j].gameObject);

        tileMap[i][j] = objectID;
        if (isBody) tileMap[i][j] *= 10;
       // GameObject.DestroyImmediate(tileMapGameObject[i][j].gameObject);
        tileMapGameObject[i][j] = GameObject.Instantiate(avaliableSpritePrefabs[objectID],
            position: new Vector3(i, j, 0),
            rotation: transform.rotation);
    }*/
    public int getBlock(Vector2 position)
    {
        //if (position.x >= Constants.MAP_SIZE || position.y >= Constants.MAP_SIZE || position.x > -1 || position.y > -1)
         //   return 0;
        return tileMap[(int)position.x][(int)position.y];
    }
    public void eat(Vector2 position)
    {
        tileMap[(int)position.x][(int)position.y] = 0;
        Destroy(tileMapGameObject[(int)position.x][(int)position.y]);

    }
    /*
    public void deploy(int i, int j, int objectID, float stayTime)
    {
        if (objectID > Constants.MAX_SPRITE_ID)
            return;
        if(tileMapGameObject[i][j] != null)
            Destroy(tileMapGameObject[i][j].gameObject);
        if (tileMapGameObject[i][j] != null)
            GameObject.DestroyImmediate(tileMapGameObject[i][j].gameObject);
        tileMapGameObject[i][j] = GameObject.Instantiate(avaliableSpritePrefabs[objectID],
            position: new Vector3(i, j, 0),
            rotation: transform.rotation);
        Destroy(tileMapGameObject[i][j], stayTime);
    }*/
}
