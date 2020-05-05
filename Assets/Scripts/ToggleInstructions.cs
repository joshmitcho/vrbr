using UnityEngine;
using UnityEngine.UI;

public class ToggleInstructions : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Image>().enabled = GyroControls.showIcons;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
