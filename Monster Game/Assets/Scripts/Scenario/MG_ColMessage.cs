using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_ColMessage : MonoBehaviour
{
    //Este funciona como un objeto con un colider, ya que se desruye el objeto cuando el jugador choca con este
    MG_GameManager Gman;
    void Start()
    {
        Gman = FindObjectOfType<MG_GameManager>();
    }
    void OnTriggerEnter2D(Collider2D col){
        if (col.tag == "Player"){
            //Sumar variable para mostrar el siguiente mensaje, acceder a la funcion del mensaje y autodestruirse
            Gman.colMessageNum += 1;
            Gman.ColidersMessage();
            Destroy(this.gameObject);
        }
    }
}
