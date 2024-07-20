using UnityEngine;
using System.Collections;

public class CastleLife : MonoBehaviour {

	public Transform bar;
	public float HP;

	public Color color;
	public Transform lifeUp;
    public Transform lifeDown;
    public Transform lifeDown2;


    public bool gameOver;

	// Use this for initialization
	void Start () 
	{
		color = bar.GetComponentInChildren<SpriteRenderer>().color;
	}

	public void TakeDamage(float dmg)
	{
		if(HP-dmg >= 0)
		{
			HP -= dmg;
		}else{
			HP = 0;
		}
        if (dmg == 3)
        {
            Instantiate(lifeDown, bar.position, Quaternion.identity);
        }
        else {
            Instantiate(lifeDown2, bar.position, Quaternion.identity);
        }
    }

	public void AddHealth(float dmg)
	{
		if(HP+dmg <= 100)
		{
			HP += dmg;
		}else{
			HP = 100;
		}
		Instantiate(lifeUp, bar.position, Quaternion.identity);
	}

	void GameOver()
	{
		for(int i = 0; i< GameObject.FindGameObjectsWithTag("Archer").Length; i++)
		{
			GameObject.FindGameObjectsWithTag("Archer")[i].GetComponent<IRagePixel>().PlayNamedAnimation("IDLE");
		}

		for(int i = 0; i< GameObject.FindGameObjectsWithTag("Monster").Length; i++)
		{
			GameObject.FindGameObjectsWithTag("Monster")[i].GetComponent<Monster>().speed = 0;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if(HP <= 0 && !gameOver)
		{
			gameOver = true;
			GameOver();
		}
		if(HP/100 >=0)bar.transform.localScale = new Vector3(HP/100, 1, 1);
		color.r = (1-HP/100);
		color.g = (HP/100);
		bar.GetComponentInChildren<SpriteRenderer>().color = color;
	}
}
