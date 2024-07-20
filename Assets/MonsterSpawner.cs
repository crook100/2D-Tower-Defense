using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour {

    public Transform monster;
    public Transform giant;

    public int enemyCount;
	public int spawned;

	public int curWave;

	public int enemyCountIncrease;
	public float lifeIncrease;

	public float currentMonsterLife = 1f;

	public float delay;


	// Use this for initialization
	void Start () 
	{
	//	StartCoroutine("Spawn");
	}

	public IEnumerator Spawn()
	{
		curWave += 1;
		currentMonsterLife += lifeIncrease;
		for(int i = 0; i < enemyCount; i++)
		{
			spawned ++;

            if (Random.Range(0, 8) == 5)
            {
                Transform myMonster = (Transform)Instantiate(giant, transform.position, Quaternion.identity);
                myMonster.GetComponent<Monster>().life = currentMonsterLife * 2;
                myMonster.parent = GameObject.FindWithTag("MainCamera").GetComponent<Main>().monsters;
            }
            else {
                Transform myMonster = (Transform)Instantiate(monster, transform.position, Quaternion.identity);
                myMonster.GetComponent<Monster>().life = currentMonsterLife;
                myMonster.parent = GameObject.FindWithTag("MainCamera").GetComponent<Main>().monsters;
            }

            yield return new WaitForSeconds(delay);
		}
	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}
