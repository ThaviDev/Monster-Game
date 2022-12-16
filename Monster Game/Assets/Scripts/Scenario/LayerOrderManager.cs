//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class LayerOrderManager : MonoBehaviour {
	private Renderer myRenderer;
	[SerializeField]
	private int height = 4000;
	[SerializeField]
	private int offset = 0;	
	[SerializeField]
	private bool isItAlive = false;	
	private float timer;
	private float timerMax = .1f;

	private void Awake(){
		myRenderer = gameObject.GetComponent<Renderer>();
	}
	
	private void LateUpdate () {
		timer -= Time.deltaTime;
		if (timer <= 0f){
			timer = timerMax;
			myRenderer.sortingOrder = (int)(height - transform.position.y * 5 - offset);
			if (isItAlive == false){
				Destroy(this);
			}
		}
	}
}
