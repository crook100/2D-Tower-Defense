using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	public int hp;
	public int maxHp;
	public float damage;

	public int trapType;

	void Start () 
	{
		maxHp = hp;
	}
	
	void Update () 
	{
		int tmp = maxHp/3;
		if(hp >=  tmp*2)
		{
			GetComponentInChildren<SpriteRenderer>().color = Color.green;
		}

		if(hp >  tmp && hp <  tmp*2)
		{
			GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
		}

		if(hp <  tmp)
		{
			GetComponentInChildren<SpriteRenderer>().color = Color.red;
		}

		if(trapType == 1)
		{
			if(GetComponent<RagePixelSprite>().GetCurrentCellIndex() == 0)
			{
				GetComponent<PolygonCollider2D>().enabled = true;
			}else{
				GetComponent<PolygonCollider2D>().enabled = false;
			}
		}

		if(hp <=0)
		{
			Destroy(gameObject);
		}
	}
}
