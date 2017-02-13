using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRandomTerrain : MonoBehaviour
{
	public GameObject platform;	//prefab to be instantiated

	public GameObject leveltrigger; //object that triggers level generation when collided with
	public GameObject invisiblewall; //prevents player from reversing too far

	public Vector2 startLocation = new Vector2 (0, 0); //start location preference

	//level randomization preferences
	public float maxPlatformAngle = 40; //maximum (negative or positive) z rotation of platform in degrees
	public float minPlatformMultiplier = 0.2f; //smallest multiplier that the platform can be scaled to, relative to platform's original size
	public float maxPlatformMultiplier = 1.2f; //largest platform scale multiplier
	public int platformAmount = 50; //the amount of platforms to be created and removed at a time

	private Queue<GameObject> activeplatforms = new Queue<GameObject>(); //queue containing currently active platforms. Enqueued and dequeued when needed.
	private Vector2 nextLocation; //location where the next platform will be instantiated

	void Start ()
	{
		//make a flat start platform
		GameObject startplatform = Instantiate (platform, startLocation, Quaternion.identity);
		activeplatforms.Enqueue (startplatform);

		//get next location from hierarchy of instantiated prefab->sprite->empty child object at the corner
		nextLocation = startplatform.transform.GetChild(0).transform.GetChild(0).gameObject.transform.position;

		//generate two sets of new platforms
		GenerateMore ();
		GenerateMore ();
	}

	//function that generates set amount of platforms, scaled and rotated randomly within constraints, positioned at the corner of their predecessor.
	public void GenerateMore()
	{
		Debug.Log ("Level generate function called");

		for (int i=0; i < platformAmount; i++)
		{
			//instantiate new platform, get a reference to it and place to queue
			GameObject nextplatform = Instantiate (platform, nextLocation, Quaternion.identity); 
			activeplatforms.Enqueue (nextplatform);

			//Rotate and scale the platform randomly
			nextplatform.transform.Rotate (0.0f, 0.0f, Random.Range(-40.0f, 40.0f));
			nextplatform.transform.localScale = new Vector3 (Random.Range (0.2f, 1.2f),1,1);

			//get next location from hierarchy of instantiated prefab->sprite->empty child object at the corner
			nextLocation = nextplatform.transform.GetChild(0).transform.GetChild(0).gameObject.transform.position;

			//move the trigger roughly in the middle of the new set
			if (i == (platformAmount / 2))
			{
				leveltrigger.transform.position = nextLocation;
			}
		}
	}

	public void RemovePrevious()
	{
		for (int i = 0; i < platformAmount; i++) //delete set amount of unnecessary platforms
		{
			GameObject disposablePlatform = activeplatforms.Dequeue ();

			//move the invisble wall to the last platform, before it's destroyed
			if(i == 49)
			{
				invisiblewall.transform.position = disposablePlatform.transform.GetChild(0).transform.GetChild(0).gameObject.transform.position;
				invisiblewall.transform.Translate (30f, 0f, 0f, Space.World); //and a little further, so that the empty void behind is not visible
				invisiblewall.transform.rotation = Quaternion.identity; //rotate straight, in case the last one happens to have a big rotation
			}

			Destroy(disposablePlatform); //destroy the gameobject
		}
		Resources.UnloadUnusedAssets (); //frees memory
	}


}
