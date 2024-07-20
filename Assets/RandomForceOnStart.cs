using UnityEngine;
using System.Collections;

public class RandomForceOnStart : MonoBehaviour {

	public float minX;
	public float maxX;

	public float minY;
	public float maxY;

	// Use this for initialization
	void Start () 
	{
		GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY)), ForceMode2D.Force);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
