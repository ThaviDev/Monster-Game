using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_PistaObj : MonoBehaviour {
	public PlayerMotor playerScrpt;
	private Material material;
	private Color materialTintColor;
	private float tintFadeSpeed;
	private Renderer rend;
	private float alphaGlowVal;
	private bool isIncreasingGlow;

	void Awake (){
		materialTintColor = new Color (1,1,0,1);
		//SetMaterial(transform.Find("Body").GetComponent<MeshRenderer>().material);

	}
	void Start () {
		rend = GetComponent<Renderer>();
		playerScrpt = FindObjectOfType<PlayerMotor>();
		alphaGlowVal = 0f;
		isIncreasingGlow = true;
	}
	void Update (){
		rend.material.SetFloat("_TintAlphaValue", alphaGlowVal);
		if (alphaGlowVal >= 1){
			isIncreasingGlow = false;
		}
		if (isIncreasingGlow == false){
			alphaGlowVal -= Time.deltaTime;
		}
		if (alphaGlowVal <= 0){
			isIncreasingGlow = true;
		}
		if (isIncreasingGlow == true){
			alphaGlowVal += Time.deltaTime;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player"){
			playerScrpt.pistasAmount += 1;
			Destroy(gameObject);
		}
	}
}
