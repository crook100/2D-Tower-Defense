using UnityEngine;
using System.Collections;

public class Archer : MonoBehaviour {

	public float shootDelay;
	public Transform arrow;
	public int arrowSpeed;
	public Transform shootPoint;

	public int shootAtFrame = 4;

	// Use this for initialization
	void Start () 
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, -5);

		if(GameObject.FindGameObjectsWithTag("Monster").Length > 0)
		{
			GetComponent<IRagePixel>().PlayNamedAnimation("SHOOT");
		}else{
			GetComponent<IRagePixel>().PlayNamedAnimation("IDLE");
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.transform.tag=="Void")
		{
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if(shootDelay > 0)
		{
			shootDelay -= Time.deltaTime;
		}

		if(GetComponent<RagePixelSprite>().GetCurrentCellIndex() == shootAtFrame && shootDelay <= 0)
		{
			Transform myArrow = (Transform)Instantiate(arrow, shootPoint.position, arrow.transform.rotation);
			myArrow.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(Random.Range(arrowSpeed-400,arrowSpeed+400), 0), ForceMode2D.Force);
			myArrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, Random.Range(5, 50)), ForceMode2D.Force);
			myArrow.parent = GameObject.FindWithTag("MainCamera").GetComponent<Main>().arrows;
			shootDelay = 1;
		}
	}
}
