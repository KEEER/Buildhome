using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bed : MonoBehaviour{
    public Bed(GameObject representitive,Vector2 position,string owner)
    {
        this.owner = owner;
        this.representitive = representitive;
        this.position = position;
    }
    public void destroyGO()
    {
        Destroy(representitive);
    }

    public string getOwnerName()
    {
        return owner;
    }
    public Vector2 getPosition()
    {
        return position;
    }
    GameObject representitive;
    Vector2 position;
    string owner;
}
