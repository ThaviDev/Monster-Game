using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RockSpawner : MonoBehaviour {
	public GameObject objRockPrefab; // Prefabricado de una roca
	public Transform spawnTrans;

	public void SpawnRock()
    {
        Instantiate(objRockPrefab,
		spawnTrans.position - new Vector3(0f,0.5f,0f), //--El 0.5 en Y es porque el pivote esta en la parte de abajo del objeto
		Quaternion.identity);
    }
}
