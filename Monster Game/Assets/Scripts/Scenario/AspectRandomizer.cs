//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class AspectRandomizer : MonoBehaviour {

	private SpriteRenderer rend;
	private int numb;
	private int prueba;
	//private string objectName;
	//private Sprite ver1, ver2, ver3;
	//private Sprite[] visualsprt;
	//public int biome; // Bioma 1 : bioma bosque principal, Bioma 2 : Cementon, 

	void Start () {
		rend = GetComponent<SpriteRenderer>();
		numb = Random.Range(1,7);
		// Elegir Textura
		switch (this.gameObject.tag){ // Primero Identifica cual es el tag del objeto
			//Luego toma una textura basandose en el numero aleatorio anterior
			case "Bush":
			switch(numb){
				case 1: rend.sprite = Resources.Load<Sprite>("Bush1"); break;
				case 2: rend.sprite = Resources.Load<Sprite>("Bush2"); break;
				case 3: rend.sprite = Resources.Load<Sprite>("Bush3"); break;
				case 4: rend.sprite = Resources.Load<Sprite>("Bush1"); break;
				case 5: rend.sprite = Resources.Load<Sprite>("Bush2"); break;
				case 6: rend.sprite = Resources.Load<Sprite>("Bush3"); break;
				default: print ("Aspect Randomizer No Texture Bush");
				break;
			}
			break;
			case "Rock":
			switch(numb){
				case 1: rend.sprite = Resources.Load<Sprite>("Rock1"); break;
				case 2: rend.sprite = Resources.Load<Sprite>("Rock2"); break;
				case 3: rend.sprite = Resources.Load<Sprite>("Rock3"); break;
				case 4: rend.sprite = Resources.Load<Sprite>("Rock1"); break;
				case 5: rend.sprite = Resources.Load<Sprite>("Rock2"); break;
				case 6: rend.sprite = Resources.Load<Sprite>("Rock3"); break;
				default: print ("Aspect Randomizer No Texture Rock");
				break;
			}
			break;
			case "Wide Tree":
			switch(numb){
				case 1: rend.sprite = Resources.Load<Sprite>("WideTree1"); break;
				case 2: rend.sprite = Resources.Load<Sprite>("WideTree2"); break;
				case 3: rend.sprite = Resources.Load<Sprite>("WideTree3"); break;
				case 4: rend.sprite = Resources.Load<Sprite>("WideTree4"); break;
				case 5: rend.sprite = Resources.Load<Sprite>("WideTree5"); break;
				case 6: rend.sprite = Resources.Load<Sprite>("WideTree6"); break;
				default: print ("Aspect Randomizer No Texture Wide Tree");
				break;
			}
			break;
			case "Thin Tree":
			switch(numb){
				case 1: rend.sprite = Resources.Load<Sprite>("ThinTree1"); break;
				case 2: rend.sprite = Resources.Load<Sprite>("ThinTree2"); break;
				case 3: rend.sprite = Resources.Load<Sprite>("ThinTree3"); break;
				case 4: rend.sprite = Resources.Load<Sprite>("ThinTree4"); break;
				case 5: rend.sprite = Resources.Load<Sprite>("ThinTree5"); break;
				case 6: rend.sprite = Resources.Load<Sprite>("ThinTree6"); break;
				default: print ("Aspect Randomizer No Texture Thin Tree");
				break;
			}
			break;
			case "Big Rock":
			switch(numb){
				case 1: rend.sprite = Resources.Load<Sprite>("BigRock1"); break;
				case 2: rend.sprite = Resources.Load<Sprite>("BigRock2"); break;
				case 3: rend.sprite = Resources.Load<Sprite>("BigRock3"); break;
				case 4: rend.sprite = Resources.Load<Sprite>("BigRock1"); break;
				case 5: rend.sprite = Resources.Load<Sprite>("BigRock2"); break;
				case 6: rend.sprite = Resources.Load<Sprite>("BigRock3"); break;
				default: print ("Aspect Randomizer No Texture Thin Tree");
				break;
			}
			break;
			case "Big Bush":
			switch(numb){
				case 1: rend.sprite = Resources.Load<Sprite>("BigBush1"); break;
				case 2: rend.sprite = Resources.Load<Sprite>("BigBush2"); break;
				case 3: rend.sprite = Resources.Load<Sprite>("BigBush3"); break;
				case 4: rend.sprite = Resources.Load<Sprite>("BigBush1"); break;
				case 5: rend.sprite = Resources.Load<Sprite>("BigBush2"); break;
				case 6: rend.sprite = Resources.Load<Sprite>("BigBush3"); break;
				default: print ("Aspect Randomizer No Texture Thin Tree");
				break;
			}
			break;
			default: print ("Aspect Randomizer No Texture All");
			break;
		}
	}

	private void LateUpdate(){
		Destroy(this);
	}
}
