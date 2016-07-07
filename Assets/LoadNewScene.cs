using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadNewScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneManager.LoadScene("Level1");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
