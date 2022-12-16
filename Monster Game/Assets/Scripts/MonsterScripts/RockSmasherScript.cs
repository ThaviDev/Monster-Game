using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSmasherScript : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Rock" || col.gameObject.tag == "Big Rock" ){
            Destroy(col.gameObject);
        }
    }
}
