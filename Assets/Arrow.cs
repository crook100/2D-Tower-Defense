using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public bool placed;
	public float dmg;

	// Use this for initialization
	void Start () 
	{
		GetComponent<AudioSource>().pitch = Random.Range(0.8f,1.2f);
		GetComponent<AudioSource>().volume = Random.Range(0.0f,0.1f);
		GetComponent<AudioSource>().Play();

		Physics2D.IgnoreLayerCollision(9, 9);
		Physics2D.IgnoreLayerCollision(9, 10);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( (transform.localRotation.z >= -0.7 && transform.localRotation.z <= -0.5) )
		{
			StartCoroutine("FixTorque");
		}
	}

	public IEnumerator FixTorque()
	{
		GetComponent<ConstantForce2D>().torque = 500;
		yield return new WaitForSeconds(0.3f);
		GetComponent<ConstantForce2D>().torque = 0;
	}

	void DestroyNow()
	{
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.transform.tag=="Arrow" && col.GetComponent<Arrow>().placed)
		{
			Invoke("DestroyNow", 3);
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.transform.tag=="Ground")
		{
			this.GetComponent<ConstantForce2D>().enabled = false;
			this.GetComponent<Collider2D>().isTrigger = true;
			this.GetComponent<Rigidbody2D>().isKinematic = true;
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            placed = true;
		}
	}
}
