using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMousePointer : MonoBehaviour 
{//INICIO PLAYER MOUSE POINTER SCRIPT
    public Transform aimTransform;
    public static GameObject camObj;
    public static Camera cam;
    private float angleOfObj;
    private void Awake()
    {//Inicio Awake
        aimTransform = transform.Find("Aim");
        camObj = GameObject.FindGameObjectWithTag("MainCamera");
        cam = camObj.GetComponent<Camera>();
    }//Fin Awake

    private void Update(){
        Vector3 mouseposition = GetMouseWorldPosition();
        //aimTransform.LookAt(mouseposition);
        Vector3 aimDirection = (mouseposition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3 (0,0, angle);
        angleOfObj = angle;
    }

    // Get Mouse Position in World with Z = 0f
    public static Vector3 GetMouseWorldPosition(){
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, cam);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera){
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    /*
    public void OnTriggerEnter2D(Collider2D col)
    {//Inicio OnTriggerEnter2D
        if (col.tag == "Rock")
        {
            Destroy(col.gameObject);
        }
    }//Fin OnTriggerEnter2D
    /*
    public void OnCollisionEnter2D(Collision2D col)
    {//Inicio OnTriggerEnter2D
        if (col.gameObject.tag == "Rock")
        {
            Destroy(col.gameObject);
        }
    }//Fin OnTrigggerEnter2D
    */

}//FIN PLAYER MOUSE POINTER SCRIPT
