using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {


	public int money;

	public Transform addMoneyPrefab;

	public Texture2D[] textures;

	public Transform archerPlacing1;
	public Transform archerPlacing2;
	public Transform trapPlacing;
	public Vector3 mousePos;

	public Transform selectedObj;

	bool spawningWave;

	public float placeBuffer;

	public bool androidBuild;

	public Transform defaultPlace;

	public Font font;

	public Transform waveTimer;

	public Transform arrows;
	public Transform monsters;

	public Transform reSelectedObj;
	public bool showRearm;

	// Use this for initialization
	void Start () 
	{

	}

	public void AddMoney(int much)
	{
		money += much;
	}

	public void RemoveMoney(int much)
	{
		money -= much;
	}

	void OnGUI()
	{
		float decimo = Screen.width/10;

		GUI.color = Color.black;
		GUI.Label(new Rect(5, 0, 100, 100), "$"+money);
		GUI.color = Color.white;

		if(showRearm)
		{
			if(GUI.Button(new Rect(Screen.width/2-(decimo/2), Screen.height-decimo, decimo, decimo), textures[3]))
			{
				if(money-15 >= 0)
				{
					RemoveMoney(15);
					reSelectedObj.GetComponent<Trap>().hp = reSelectedObj.GetComponent<Trap>().maxHp;

					reSelectedObj = null;
					showRearm = false;

				}
			}
		}

		if(GameObject.FindWithTag("Castle").GetComponent<CastleLife>().gameOver)
		{
			if(GUI.Button(new Rect(Screen.width/2-100, Screen.height/2-30, 200, 60), "Game Over \r\n Click to restart."))
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		}else{
			if(GUI.Button(new Rect(50, 0, decimo, decimo), textures[0]))
			{
				if(money-50 >= 0)
				{
					placeBuffer = 0.1f;
					mousePos = defaultPlace.position;
					selectedObj = (Transform)Instantiate(archerPlacing1, defaultPlace.position, Quaternion.identity);
				}
			}

			if(GUI.Button(new Rect(decimo+50, 0, decimo, decimo), textures[1]))
			{
				if(money-100 >= 0)
				{
					placeBuffer = 0.1f;
					mousePos = defaultPlace.position;
					selectedObj = (Transform)Instantiate(archerPlacing2, defaultPlace.position, Quaternion.identity);
				}
			}

			if(GUI.Button(new Rect(decimo*2+50, 0, decimo, decimo), textures[2]))
			{
				if(money-30 >= 0)
				{
					placeBuffer = 0.1f;
					mousePos = defaultPlace.position;
					selectedObj = (Transform)Instantiate(trapPlacing, defaultPlace.position, Quaternion.identity);
				}
			}

            if (GUI.Button(new Rect(decimo * 3 + 50, 0, decimo, decimo), "TimeScale Change"))
            {
                if (Time.timeScale == 3)
                {
                    Time.timeScale = 1f;
                }
                else {
                    Time.timeScale = 3f;
                }
                
            }
        }

		if(androidBuild)GUI.skin.font = font;

		if(selectedObj && androidBuild)
		{
			GUI.color = Color.black;
			GUI.Label(new Rect(Screen.width/2, Screen.height/2, Screen.width, 80), "Drag and Drop to Place the archer.");
			GUI.color = Color.white;
		}
	}

	public IEnumerator NextWave()
	{
		AddMoney(Mathf.FloorToInt(5+(GameObject.FindWithTag("MonsterSpawn").GetComponent<MonsterSpawner>().enemyCount/2)));

		Transform myAddMoney = (Transform)Instantiate(addMoneyPrefab, new Vector3(-101, 10, -9), Quaternion.identity);
		myAddMoney.GetComponent<TextMesh>().text = "$"+(Mathf.FloorToInt(5+(GameObject.FindWithTag("MonsterSpawn").GetComponent<MonsterSpawner>().enemyCount/2)));
		myAddMoney.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
		Instantiate(waveTimer, new Vector3(-114, 25, -8), Quaternion.identity);
		yield return new WaitForSeconds(3);
		Debug.Log ("new wave comes...");
		GameObject.FindWithTag("MonsterSpawn").GetComponent<MonsterSpawner>().enemyCount += GameObject.FindWithTag("MonsterSpawn").GetComponent<MonsterSpawner>().enemyCountIncrease;
		GameObject.FindWithTag("MonsterSpawn").GetComponent<MonsterSpawner>().spawned = 0;
		GameObject.FindWithTag("MonsterSpawn").GetComponent<MonsterSpawner>().StartCoroutine("Spawn");

		for(int i = 0; i < GameObject.FindGameObjectsWithTag("Archer").Length; i++)
		{
			GameObject.FindGameObjectsWithTag("Archer")[i].GetComponent<IRagePixel>().PlayNamedAnimation("SHOOT");
			yield return new WaitForSeconds(Random.Range(0.1f,0.3f));
		}
		yield return null;
	}

	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			
			if(hit.collider != null)
			{
				if(hit.transform.GetComponent<Trap>() != null)
				{
					if(hit.transform.GetComponent<Trap>().hp < (hit.transform.GetComponent<Trap>().maxHp/3)*2)
					{
						reSelectedObj = hit.transform;
						showRearm = true;
						return;
					}
				}
			}
			reSelectedObj = null;
			showRearm = false;
		}

		if(androidBuild && Input.touchCount > 0 && GUIUtility.hotControl == 0)
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			
			if(hit.collider != null)
			{
				if(hit.transform.GetComponent<Trap>() != null)
				{
					if(hit.transform.GetComponent<Trap>().hp < (hit.transform.GetComponent<Trap>().maxHp/3)*2)
					{
						reSelectedObj = hit.transform;
						showRearm = true;
						return;
					}
				}
			}
			reSelectedObj = null;
			showRearm = false;
		}


		if(placeBuffer>0)placeBuffer -= Time.deltaTime;

		if(spawningWave)
		{
			if(GameObject.FindGameObjectsWithTag("Monster").Length > 1)
			{
				spawningWave = false;
			}
		}

		if(GameObject.FindWithTag("MonsterSpawn").GetComponent<MonsterSpawner>().spawned == GameObject.FindWithTag("MonsterSpawn").GetComponent<MonsterSpawner>().enemyCount)
		{
			if(GameObject.FindGameObjectsWithTag("Monster").Length <= 0 && !spawningWave)
			{
				for(int i = 0; i < GameObject.FindGameObjectsWithTag("Archer").Length; i++)
				{
					GameObject.FindGameObjectsWithTag("Archer")[i].GetComponent<IRagePixel>().PlayNamedAnimation("IDLE");
				}
				spawningWave = true;
				StartCoroutine("NextWave");
			}
		}

		if(selectedObj && placeBuffer <= 0)
		{
			selectedObj.position = mousePos;

			if(!androidBuild)
			{
				float mousex = (Input.mousePosition.x);
				float mousey = (Input.mousePosition.y);
				mousePos = Camera.main.ScreenToWorldPoint(new Vector3 (mousex, mousey, (Mathf.Abs (Camera.main.transform.position.z) )-5 ));
			}

			if(androidBuild)
			{
				if (Input.touchCount > 0)
				{
					Vector2 touchPos = Input.GetTouch(0).position;
					mousePos = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x-100, touchPos.y, (Mathf.Abs (Camera.main.transform.position.z) )-5));
				}
			}

			if(!androidBuild && Input.GetMouseButtonDown(0) )
			{
				if(selectedObj.GetComponent<Placing>().canPlace)
				{
					RemoveMoney(selectedObj.GetComponent<Placing>().cost);
					Instantiate(selectedObj.GetComponent<Placing>().Object, mousePos, Quaternion.identity);

					if(GameObject.FindGameObjectsWithTag("Archer").Length <= 1 && !selectedObj.GetComponent<Placing>().isTrap)
					{
						GameObject.FindWithTag("MonsterSpawn").GetComponent<MonsterSpawner>().StartCoroutine("Spawn");
					}
					
				}
				selectedObj.GetComponent<CallDestroy>().DestroyNow();
				return;
			}

			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && androidBuild)
			{
				if(selectedObj.GetComponent<Placing>().canPlace)
				{
					RemoveMoney(selectedObj.GetComponent<Placing>().cost);
					Instantiate(selectedObj.GetComponent<Placing>().Object, mousePos, Quaternion.identity);

					if(GameObject.FindGameObjectsWithTag("Archer").Length <= 1 && !selectedObj.GetComponent<Placing>().isTrap)
					{
						GameObject.FindWithTag("MonsterSpawn").GetComponent<MonsterSpawner>().StartCoroutine("Spawn");
					}

				}
				selectedObj.GetComponent<CallDestroy>().DestroyNow();
				return;
			}
		}
	}
}
