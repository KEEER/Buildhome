using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedManager : MonoBehaviour {

    public GameObject textObjPrefab;
    public Vector2 offset;
    private List<Bed> beds;
    private List<Vector2> bedPosition;//for fast search
    private OverallTileManager tileManager;
    Dictionary<string, int> playerBeds =
            new Dictionary<string, int>();
    // Use this for initialization
    void Start () {
        beds = new List<Bed>();
        bedPosition = new List<Vector2>();
        tileManager = gameObject.GetComponent<OverallTileManager>();
	}
    public int howManyBedTheSnakeHas(string snakeName)
    {
        if (!playerBeds.ContainsKey(snakeName))
            return -1;
        return playerBeds[snakeName];
    }
    public Vector2 getRespawnPosition(string playerName)
    {
        if (!playerBeds.ContainsKey(playerName) || playerBeds[playerName] < 1)
            return new Vector2(-1,-1);
        List<Vector2> avaliable = new List<Vector2>() ;
        foreach(Bed bed in beds)
            if(bed.getOwnerName().Equals(playerName))
                avaliable.Add(bed.getPosition());

        return avaliable[Random.Range(0, avaliable.Count)];
    }
	public void registerBed(Vector2 position,string owner)
    {
        if(position.x > -1 && position.y > -1 && position.x < Constants.MAP_SIZE && position.y < Constants.MAP_SIZE)
        {
            bedPosition.Add(position);
            GameObject go = Instantiate(tileManager.avaliableSpritePrefabs[Constants.SPRITE_BED], position, transform.rotation);
            GameObject namego = Instantiate(textObjPrefab, position + offset, transform.rotation);
            namego.GetComponent<TextMesh>().text = owner + "'s bed";
            namego.transform.parent = go.transform;
            beds.Add(new Bed(go,position,owner));
            //bug may occur
            if (!playerBeds.ContainsKey(owner))
                playerBeds.Add(owner, 1);
            else playerBeds[owner]++;
        }
    }
    public void destroyBed(Vector2 position)
    {
        if(!bedPosition.Contains(position))
        {
            //Debug.LogError("something's going really wrong.");
            return;
        }
        int index = bedPosition.IndexOf(position);
        if (!playerBeds.ContainsKey(beds[index].getOwnerName()))
            playerBeds.Add(beds[index].getOwnerName(), 0);
        else playerBeds[beds[index].getOwnerName()]--;
        Debug.Log("destroy....");
        beds[index].destroyGO();
        Destroy(beds[index]);
        beds.RemoveAt(index);
        bedPosition.RemoveAt(index);
        
    }
    public bool isHereMyBed(Vector2 position,string playerName)
    {
        if (!bedPosition.Contains(position))
        {
            return false;
        }
        int index = bedPosition.IndexOf(position);
        if(beds[index].getOwnerName().Equals(playerName))
            return true;
        return false;
    }
    public bool isHereABed(Vector2 position)
    {
        if (!bedPosition.Contains(position))
        {
            return false;
        }
        return true;
    }
    // Update is called once per frame
    void Update () {
		
	}
}
