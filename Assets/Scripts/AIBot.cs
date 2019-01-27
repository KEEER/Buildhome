using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AIBot : MonoBehaviour
{

    public String playerName;
    public GameObject clock;
    private HomeManager homeManager;
    private CollisionManager collisionManager;
    private Vector3 formerLastBlock;
    private List<Vector4> snakeBody;
    private List<GameObject> body;
    private GameObject head;
    private Vector2 headPosition;
    private int headDirection;
    public OverallTileManager overallManagerHandle;
    //private SmoothFollow followerHandle;
    public GameObject managerObject;
    private int shield;
    BedManager bedManager;
    Vector2[] foreOneStep = new Vector2[4] { new Vector2(0, -1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0) };
    bool notUsingBed = true;
    Vector2 foreHeadPosition;

    void Start()
    {
        //initiation and evaluation
        shield = 0;
        bedManager = managerObject.GetComponent<BedManager>();
        collisionManager = managerObject.GetComponent<CollisionManager>();
        snakeBody = new List<Vector4>();
        body = new List<GameObject>();
        headPosition = new Vector2(40, 40);
        headDirection = Constants.HEAD_DIRECTION_RIGHT;
        overallManagerHandle = managerObject.GetComponent<OverallTileManager>();
        homeManager = managerObject.GetComponent<HomeManager>();
       // followerHandle = cameraObject.GetComponent<SmoothFollow>();
        //followerHandle.updateTargetPosition(headPosition);
       // followerHandle.updateSize(5);
        head = Instantiate(overallManagerHandle.avaliableSpritePrefabs[Constants.TO_SPRITE_ID_OFFSET], headPosition, transform.rotation);
        //TEST: with body
        /*
        addBody(Constants.SPRITE_WOOD);
        addBody(Constants.SPRITE_WOOD);
        addBody(Constants.SPRITE_WOOD);
        addBody(Constants.SPRITE_STONE);
        addBody(Constants.SPRITE_WOOD);
        addBody(Constants.SPRITE_WOOD);
        addBody(Constants.SPRITE_STONE);
        */
        //endtest
        Debug.Log(managerObject);
        makeASweetHome(headPosition + new Vector2(-2, 3));
    }
    public void addBody(int type)
    {
        //new parameters
        int fx, fy, fw, fz;
        if (snakeBody.Count == 0)
        {
            fx = (int)headPosition.x;
            fy = (int)headPosition.y;
            fw = headDirection;
        }
        else
        {
            fw = (int)snakeBody[snakeBody.Count - 1].w;
            fx = (int)snakeBody[snakeBody.Count - 1].x;
            fy = (int)snakeBody[snakeBody.Count - 1].y;
        }
        fz = type;
        //reversed calculation to find out the point where the new body should be
        switch (fw)
        {
            case Constants.HEAD_DIRECTION_RIGHT:
                fx--;
                break;
            case Constants.HEAD_DIRECTION_LEFT:
                fx++;
                break;

            case Constants.HEAD_DIRECTION_UP:
                fy--;
                break;
            case Constants.HEAD_DIRECTION_DOWN:
                fy++;
                break;
        }
        //add body
        snakeBody.Add(new Vector4(fx, fy, fz, fw));
        body.Add(Instantiate(overallManagerHandle.avaliableSpritePrefabs[(int)fz], new Vector3(fx, fy, 0), this.transform.rotation));
        //Debug.Log("body appended, @ " + snakeBody[snakeBody.Count-1]);
    }
    private void move()
    {

        //new perameters
        int fx, fy, fw, fz;
        fx = (int)headPosition.x;
        fy = (int)headPosition.y;
        switch (headDirection)
        {
            case Constants.HEAD_DIRECTION_RIGHT:
                fx++;
                break;
            case Constants.HEAD_DIRECTION_LEFT:
                fx--;
                break;

            case Constants.HEAD_DIRECTION_UP:
                fy++;
                break;
            case Constants.HEAD_DIRECTION_DOWN:
                fy--;
                break;
        }
        headPosition = new Vector2(fx, fy);
        for (int i = 0; i < snakeBody.Count; i++)
        {
            //update position
            fx = (int)snakeBody[i].x;
            fy = (int)snakeBody[i].y;
            switch ((int)snakeBody[i].w)
            {
                case Constants.HEAD_DIRECTION_RIGHT:
                    fx++;
                    break;
                case Constants.HEAD_DIRECTION_LEFT:
                    fx--;
                    break;

                case Constants.HEAD_DIRECTION_UP:
                    fy++;
                    break;
                case Constants.HEAD_DIRECTION_DOWN:
                    fy--;
                    break;
            }
            //Debug.Log(snakeBody[i].x + " , " + snakeBody[i].y + " ?= " + headPosition.x + " , " + headPosition.y);
            //update data
            snakeBody[i] = new Vector4(fx, fy, snakeBody[i].z, snakeBody[i].w);
        }
        for (int i = snakeBody.Count - 1; i >= 0; i--)
        {
            if (i == 0)
                snakeBody[i] = new Vector4(snakeBody[i].x, snakeBody[i].y, snakeBody[i].z, headDirection);
            else
                snakeBody[i] = new Vector4(snakeBody[i].x, snakeBody[i].y, snakeBody[i].z, snakeBody[i - 1].w);
            if (snakeBody[i].x == headPosition.x && snakeBody[i].y == headPosition.y)
            {
                // Debug.Log(i + " is at " + snakeBody[i]);
                //okay one
                int mass = i + 1;
                homeManager.HMBegin(mass, false);
                for (int j = 0; j <= i; j++)
                {
                    //Debug.Log("#j = " + j);
                    //Debug.Log("total length = " + snakeBody.Count);
                    //Debug.Log("position = " + snakeBody[j].x + " , " + snakeBody[j].y);
                    GameObject go = Instantiate(overallManagerHandle.avaliableSpritePrefabs[(int)snakeBody[j].z], new Vector2(snakeBody[j].x, snakeBody[j].y), Quaternion.Euler(0, 0, 0));
                    collisionManager.register(go, (int)snakeBody[j].x, (int)snakeBody[j].y, mass, (int)snakeBody[j].z, playerName);
                    homeManager.HMPoint((int)snakeBody[j].x, (int)snakeBody[j].y, (int)snakeBody[j].w);
                    //new things
                    if (snakeBody.Count <= 0) break;
                    if (body[j])
                        Destroy(body[j]);
                    body.RemoveAt(j);
                    snakeBody.RemoveAt(j--);
                    i--;
                }
                shield += 2;
                homeManager.HMEnd(playerName);
                break;
            }
        }

       // followerHandle.updateTargetPosition(headPosition);
    }
    protected void render()
    {
        //render head
        Vector3 destination = headPosition;
        Vector3 smoothed = Vector3.Lerp(head.transform.position, destination, 0.2f);
        head.transform.position = smoothed;
        Quaternion angle = Quaternion.Euler(0, 0, -headDirection * 90);
        Quaternion smoothedAngle = Quaternion.Lerp(head.transform.rotation, angle, 0.2f);
        head.transform.rotation = smoothedAngle;
        //render body
        for (int i = 0; i < snakeBody.Count; i++)
        {
            Vector3 destinationBody = new Vector3(snakeBody[i].x, snakeBody[i].y, 0);
            Vector3 smoothedBody = Vector3.Lerp(body[i].transform.position, destinationBody, 0.2f);
            body[i].transform.position = smoothedBody;
            //body[i].transform.Rotate(0, 0, 100 * Time.deltaTime);
        }

        //
    }

    public float frameTime = 0.1f, anotherTimer = 4f;
    float lastTimeMark, lastAnotherTimerMark;

    public float AILogicUpdateTime = 2f;
    public float AILogicLastTimemark;
    int status = 0;
    void Update()
    {
        if (!notUsingBed)
            Debug.Log("#it has been " + (Time.time - lastAnotherTimerMark));
        if (!notUsingBed && Time.time - lastAnotherTimerMark > anotherTimer)
        {
            Debug.Log("DESTROY");
            bedManager.destroyBed(foreHeadPosition);
            notUsingBed = true;
            lastAnotherTimerMark = Time.time;
        }
        //////
        ///AI PART
        if(Time.time - AILogicLastTimemark > AILogicUpdateTime)
        {
            int x = UnityEngine.Random.Range(0,100);
            if (x < 23)
                status = 0;//up
            else if (x < 46)
                status = 1;//right
            else if (x < 23 * 3)
                status = 2;//down
            else if (x < 23 * 4)
                status = 3;//left
            else status = 4;
            switch (status)
            {
                case 0:
                    headDirection = Constants.HEAD_DIRECTION_UP;
                    break;
                case 1:
                    headDirection = Constants.HEAD_DIRECTION_RIGHT;
                    break;
                case 2:
                    headDirection = Constants.HEAD_DIRECTION_DOWN;
                    break;
                case 3:
                    headDirection = Constants.HEAD_DIRECTION_LEFT;
                    break;
                case 4:
                    makeASweetHome(headPosition + new Vector2(-2, 3));
                    break;
            }
            AILogicLastTimemark = Time.time;
        }
        ///ENDAI
        ///
        render();
        if (frameTime > Time.time - lastTimeMark)
            return;
        ///slow update
        if (notUsingBed)
            move();
        updateLogic();

        lastTimeMark = Time.time;
    }

    private void updateLogic()
    {
        //bed
        foreHeadPosition = headPosition + foreOneStep[headDirection];
        if (bedManager.isHereMyBed(foreHeadPosition, playerName))
        {
            Debug.Log("Yay, I find my bed!");

        }
        else if (bedManager.isHereABed(foreHeadPosition) && notUsingBed)
        {
            //TODO generate bed breaker.
            notUsingBed = false;
            //bedManager.destroyBed(foreHeadPosition);
            Destroy(Instantiate(clock, foreHeadPosition, clock.transform.rotation), 4);
            lastAnotherTimerMark = Time.time;
        }
        //out
        if (foreHeadPosition.x < 0 || foreHeadPosition.y < 0 || foreHeadPosition.x >= Constants.MAP_SIZE || foreHeadPosition.y >= Constants.MAP_SIZE)
        {
            while (snakeBody.Count > 0) snakeBody.RemoveAt(0);
            headPosition = bedManager.getRespawnPosition(playerName);
           // cameraObject.transform.position = headPosition + new Vector2(cameraObject.GetComponent<SmoothFollow>().offset.x, cameraObject.GetComponent<SmoothFollow>().offset.y);
            shield += 5;

        }
        if (bedManager.howManyBedTheSnakeHas(playerName) < 0)
        {
            Debug.Log("GameOver!");
            //TODO: gameover
        }
        //wall
        if (shield <= 0)
        {
            int currentMass = collisionManager.getMass(headPosition, playerName);
            Debug.Log("#hit " + currentMass);
            while (currentMass > 0)
            {
                if (snakeBody.Count > 0)
                {
                    snakeBody.RemoveAt(snakeBody.Count - 1);
                    Destroy(body[body.Count - 1]);
                    body.RemoveAt(body.Count - 1);
                }
                else break;
                currentMass--;
            }

            if (currentMass > 0)
            {
                while (snakeBody.Count > 0) snakeBody.RemoveAt(0);
                headPosition = bedManager.getRespawnPosition(playerName);
               // cameraObject.transform.position = headPosition + new Vector2(cameraObject.GetComponent<SmoothFollow>().offset.x, cameraObject.GetComponent<SmoothFollow>().offset.y);
                shield += 5;

            }
            else collisionManager.deleteBlock(headPosition, playerName);
        }
        else shield--;
        //eat
        int eaten = overallManagerHandle.getBlock(headPosition);
        if (eaten > 0)
        {
            addBody(eaten);
            overallManagerHandle.eat(headPosition);
        }
       // followerHandle.updateSize((int)(snakeBody.Count * 0.3f) + 4);
    }
    public void makeASweetHome(Vector2 position)
    {
        homeManager.HMBegin(12, true);
        homeManager.HMBedPoint(position + new Vector2(1, 2));
        for (int i = 0; i <= 3; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                if (i == 0 || j == 0 || i == 3 || j == 3)
                {
                    int fx, fy, blockID;
                    fx = (int)position.x + i;
                    fy = (int)position.y + j;
                    homeManager.HMPoint(fx, fy);
                    if (i != 1 && i != 2)
                        blockID = Constants.SPRITE_WOOD;
                    else if (j < 3)
                        blockID = Constants.SPRITE_DOOR;
                    else blockID = Constants.SPRITE_STONE;
                    GameObject go = Instantiate(overallManagerHandle.avaliableSpritePrefabs[blockID], new Vector2(fx, fy), Quaternion.Euler(0, 0, 0));
                    collisionManager.register(go, fx, fy, 12, blockID, playerName);
                }
            }
        }
        homeManager.HMEnd(playerName);
    }
}
