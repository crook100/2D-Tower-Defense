using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {

	public float life;
	public bool dying;
	public float speed;
	public bool healCastle;
    public bool giant;

	public Transform blood;
	public Transform headPos;
	public Transform realHeadPos;

	public AudioClip[] clips;

	// Use this for initialization
	void Start () 
	{
		realHeadPos = transform.Find("realHead");
		if(Random.Range(0,10) == 6)
		{
			healCastle = true;
			transform.Find("Gem").gameObject.SetActive(true);
		}

		GetComponent<IRagePixel>().PlayNamedAnimation("WALK", true);
		Physics2D.IgnoreLayerCollision(8, 8);

        if (giant)
        {
            speed = speed / 2;
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!dying)
		{
			transform.Translate(-speed * Time.deltaTime, 0, 0);
		}

		if(life <=0 && !dying)
		{
			dying = true;
			Die ();
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.transform.tag=="Arrow")
		{
			if(!col.transform.GetComponent<Arrow>().placed)
			{
				col.GetComponent<Arrow>().placed = true;
				life -= col.GetComponent<Arrow>().dmg;

				Instantiate(blood, headPos.position, Quaternion.identity);

				if(life > 0)
				{
					GetComponent<AudioSource>().clip = clips[0];
					GetComponent<AudioSource>().volume = 0.5f;

                    if (giant)
                    {
                        GetComponent<AudioSource>().pitch = Random.Range(0.4f, 0.8f);
                        GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 0), ForceMode2D.Impulse);
                    }
                    else {
                        GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
                        GetComponent<Rigidbody2D>().AddForce(new Vector2(25, 0), ForceMode2D.Impulse);
                    }
                    GetComponent<AudioSource>().Play();
				}
				return;
			}
		}

		if(col.transform.tag=="Castle")
		{
			GetComponent<Rigidbody2D>().AddForce(new Vector2(25, 0), ForceMode2D.Impulse);
			GetComponent<AudioSource>().clip = clips[0];
			GetComponent<AudioSource>().Play();

            if (giant)
            {
                GetComponent<AudioSource>().pitch = Random.Range(0.4f, 0.8f);
                col.GetComponent<CastleLife>().TakeDamage(6);
            }
            else {
                GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
                col.GetComponent<CastleLife>().TakeDamage(3);
            }
        }

		if(col.transform.tag=="Trap")
		{
			GetComponent<Rigidbody2D>().AddForce(new Vector2(20, 0), ForceMode2D.Impulse);
			GetComponent<AudioSource>().clip = clips[0];
			GetComponent<AudioSource>().pitch = Random.Range(0.8f,1.2f);
			GetComponent<AudioSource>().Play();
			col.GetComponent<Trap>().hp -= 1;
			life -= col.GetComponent<Trap>().damage;
		}
	}

	void Die () 
	{
		GetComponent<AudioSource>().clip = clips[1];
        if (giant)
        {
            GetComponent<AudioSource>().pitch = Random.Range(0.4f, 0.8f);
        }
        else
        {
            GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
        }
        GetComponent<AudioSource>().Play();

		GameObject.FindWithTag("MainCamera").GetComponent<Main>().AddMoney(Mathf.FloorToInt(1f+(GameObject.FindWithTag("MonsterSpawn").GetComponent<MonsterSpawner>().currentMonsterLife)));

		Transform myAddMoney = (Transform)Instantiate(GameObject.FindWithTag("MainCamera").GetComponent<Main>().addMoneyPrefab, realHeadPos.position, Quaternion.identity);
		myAddMoney.GetComponent<TextMesh>().text = "$"+(Mathf.FloorToInt(1f+(GameObject.FindWithTag("MonsterSpawn").GetComponent<MonsterSpawner>().currentMonsterLife)));

		this.GetComponent<Collider2D>().enabled = false;
		this.GetComponent<Rigidbody2D>().isKinematic = true;
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (healCastle)
		{
			GameObject.FindWithTag("Castle").GetComponent<CastleLife>().AddHealth(10);
		}

		transform.Find("Gem").GetComponent<CallDestroy>().beginFade = true;
		transform.Find("Gem").GetComponent<CallDestroy>().Timeout = 0;
		transform.Find("Gem").GetComponent<CallDestroy>().Invoke("DestroyNow", 1);

		GetComponent<CallDestroy>().Timeout = 9;
		GetComponent<CallDestroy>().beginFade = true;
		GetComponent<CallDestroy>().Invoke("DestroyNow", 10);
		this.transform.tag = "Untagged";
		GetComponent<IRagePixel>().PlayNamedAnimation("DIE", true);
	}
}
