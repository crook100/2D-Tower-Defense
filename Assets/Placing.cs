using UnityEngine;
using System.Collections;

public class Placing : MonoBehaviour {

	public bool canPlace;

	public bool isTrap;

	public int archerNumber;
	public Transform Object;

	public int cost;

	// Use this for initialization
	void Start () 
	{
	
	}

	void OnTriggerEnter2D (Collider2D col) 
	{
		if(!isTrap && col.transform.tag=="Placeable")
		{
			canPlace = true;
		}

		if(isTrap && col.transform.tag=="NotPlaceable")
		{
			canPlace = true;
		}

	}

	void OnTriggerExit2D (Collider2D col) 
	{
		if(!isTrap && col.transform.tag=="Placeable")
		{
			canPlace = false;
		}

		if(isTrap && col.transform.tag=="NotPlaceable")
		{
			canPlace = false;
		}

	}

	// Update is called once per frame
	void Update () 
	{
		if(canPlace)
		{
			GetComponent<IRagePixel>().SetTintColor(Color.white);
		}else{
			GetComponent<IRagePixel>().SetTintColor(Color.red);
		}
	}
}
