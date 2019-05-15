using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public bool isActive;
    public Vector3 hidePos;
    public Vector3 showPos;

    public Text hidePrompt;

	// Use this for initialization
	void Start () {
        isActive = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (isActive) {
            hidePrompt.text = "[H]  Hide Panel";
            if (GetComponent<RectTransform>().anchoredPosition.x < showPos.x) {
                transform.Translate(Vector3.right * 400 * Time.deltaTime);
            }
        }

        if (!isActive){
            hidePrompt.text = "[H]  Show Panel";
            if (GetComponent<RectTransform>().anchoredPosition.x > hidePos.x) {
                transform.Translate(Vector3.left * 400 * Time.deltaTime);
            }
        }

        if (Input.GetKeyDown(KeyCode.H)) {
            if (isActive == true)
                isActive = false;
            else
                isActive = true;
        }



    }
}
