using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_PickUp : MonoBehaviour {
	private MG_Inventory inventory;
	public GameObject itemUI;
	private void Start ()
	{// Beggin Start
		inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<MG_Inventory>();
	}// End Start
	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag("PlayerHands")){
			for (int i = 0; i < inventory.slots.Length; i++)
			{
				if (inventory.isFull[i] == false){
					//-- Item puede ser aniadido al inventario
					print ("Objeto fue agarrado");
					print (inventory.isFull[i]);
					inventory.isFull[i] = true;
					print (inventory.isFull[i]);
					Instantiate(itemUI, inventory.slots[i].transform, false);
					Destroy(gameObject);
					break;
				}
			}
		}
	}
}
