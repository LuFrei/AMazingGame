using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElseIfTest : MonoBehaviour {

    public bool isOn;
    public int num;


	// Use this for initialization
	void Start () {
        if (!isOn) {
            Debug.Log("It's off!");
        } else if (isOn && num == 3) {
            Debug.Log("It's on, and its 3");
        } else if (num > 2){
            Debug.Log("Unspecified, but greater than 2");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
