using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stttar : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void Update()
    {
        UN = Screen.width / 16;
    }
    int UN;
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 1 * UN, Screen.height / 2 - 2 * UN, 6 * UN, 2 * UN), "Buildhome");
        if (GUI.Button(new Rect(Screen.width / 2 - 3 * UN, Screen.height / 2 - UN, 6 * UN, 2 * UN), "S T A R T"))
        {
            Application.LoadLevel(1);
        }
    }
}
