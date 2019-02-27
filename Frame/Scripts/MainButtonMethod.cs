using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainButtonMethod : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    this.transform.GetComponent<Button>().onClick.AddListener(OnClick); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        Debug.LogError("enter the button");
        SceneManager.LoadSceneAsync("Login");
    }
}
