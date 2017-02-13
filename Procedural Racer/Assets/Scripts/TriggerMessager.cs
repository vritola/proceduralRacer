using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attaches to a gameobject, must include a 2D collider as trigger.
public class TriggerMessager : MonoBehaviour
{
	public GameObject gameManager; //reference to object containing random terrain script
	private NewRandomTerrain levelscript;

	void Awake()
	{
		levelscript = gameManager.GetComponent<NewRandomTerrain> ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			Debug.Log ("Midpoint triggered");

			//Call functions to generate more level and remove unnecessary platforms
			levelscript.GenerateMore ();
			levelscript.RemovePrevious ();
		}
	}
}
