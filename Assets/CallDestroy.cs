using UnityEngine;
using System.Collections;

public class CallDestroy : MonoBehaviour {

	public bool DestroyOnStart;
	public float Timeout;

	public bool fade;
	public bool beginFade;

	public bool ragePixelSprite;

	Color myColor = Color.white;

	// Use this for initialization
	void Start () 
	{
		if(ragePixelSprite)myColor = Color.white;
		if(!ragePixelSprite) myColor = GetComponent<TextMesh>().color;
		
		if(DestroyOnStart)
		{
			Invoke("DestroyNow", Timeout);
		}
		Timeout -= 1;
	}

	public void DestroyNow () 
	{
		Destroy(gameObject);
	}

	// Update is called once per frame
	void Update () 
	{
		if(fade && beginFade)
		{
			Timeout -= Time.deltaTime;
		}

		if(Timeout <= 0 && fade)
		{
			myColor -= new Color(0, 0, 0, 1*Time.deltaTime);

			if(ragePixelSprite)
			{
				if(GetComponent<IRagePixel>() != null)
				{
					GetComponent<IRagePixel>().SetTintColor(myColor);
				}else{
					GetComponentInChildren<IRagePixel>().SetTintColor(myColor);
				}
			}else{
				GetComponent<TextMesh>().color = myColor;
			}
		}

	}
}
