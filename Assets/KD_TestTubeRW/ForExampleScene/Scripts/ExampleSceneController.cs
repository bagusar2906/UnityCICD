﻿using UnityEngine;

public class ExampleSceneController : MonoBehaviour
{
    // Holds the objects that can be displayed at stage location
    public GameObject[] displayObjects;
    // Holds the materials for the info panel to change to
    public Material[] displayMat;
    // Holds the info panel GameObject to change the material onto
    public GameObject infoPanel;

    // Counter for current item in the array that is being displayed
    private int currObj;

    // The location for the revealed object to spawn
    public GameObject spawnLoc;
    public GameObject spawnLoc2;
    // Bool that checks to see if the display needs to rotate
    public bool doRotate;
    // Float to set how fast the revealed object rotates
    public float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        currObj = 0;
        GameObject temp = Instantiate(displayObjects[currObj], spawnLoc2.transform.position, Quaternion.identity);
        temp.transform.parent = spawnLoc2.transform;
        infoPanel.GetComponent<Renderer>().material = displayMat[currObj];
    }

    // Update is called once per frame
    void Update()
    {
        if (doRotate)
        {
            spawnLoc.transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed);
        }
    }

    // This function takes a string "dir" which is either A or B. 
    // If A it adds 1 to currObj, if B it subtracts. It then checks to see if the object at displayObjects[currObj] + 1 or - 1 is legal and if so replaces the displayed object with that one
    // by destroying the one attached to the spawnLoc and instantiating a new one based on what is housed in displayObject[currObj]

    public void NewObjectDisplay(string dir)
    {
        if(dir == "A")
        {
            if (currObj < (displayObjects.Length-1))
            {
                currObj += 1;
                foreach(Transform obj in spawnLoc.transform)
                {
                    GameObject.Destroy(obj.gameObject);
                }
                foreach (Transform obj in spawnLoc2.transform)
                {
                    GameObject.Destroy(obj.gameObject);
                }
                // Correction made for the Test Tube as the pivot is not at the base due to placing it where it would be better used to handle
                if (currObj == 0)
                {
                    GameObject temp = Instantiate(displayObjects[currObj], spawnLoc2.transform.position, Quaternion.identity);
                    temp.transform.parent = spawnLoc2.transform;
                    temp.transform.rotation = spawnLoc2.transform.rotation;
                }
                else
                {
                    GameObject temp = Instantiate(displayObjects[currObj], spawnLoc.transform.position, Quaternion.identity);
                    temp.transform.parent = spawnLoc.transform;
                    temp.transform.rotation = spawnLoc.transform.rotation;
                }
               
                infoPanel.GetComponent<Renderer>().material = displayMat[currObj];
            }
        }
        if(dir == "B")
        {
            if (currObj > 0)
            {
                currObj -= 1;
                foreach (Transform obj in spawnLoc.transform)
                {
                    GameObject.Destroy(obj.gameObject);
                }
                foreach (Transform obj in spawnLoc2.transform)
                {
                    GameObject.Destroy(obj.gameObject);
                }
                // Correction made for the Test Tube as the pivot is not at the base due to placing it where it would be better used to handle
                if (currObj == 0)
                {
                    GameObject temp = Instantiate(displayObjects[currObj], spawnLoc2.transform.position, Quaternion.identity);
                    temp.transform.parent = spawnLoc2.transform;
                    temp.transform.rotation = spawnLoc2.transform.rotation;
                }
                else
                {
                    GameObject temp = Instantiate(displayObjects[currObj], spawnLoc.transform.position, Quaternion.identity);
                    temp.transform.parent = spawnLoc.transform;
                    temp.transform.rotation = spawnLoc.transform.rotation;
                }

                infoPanel.GetComponent<Renderer>().material = displayMat[currObj];
            }
        }
    }

    // This functions quits the experience

    public void QuiteExperience()
    {
        Application.Quit();
    }




}
