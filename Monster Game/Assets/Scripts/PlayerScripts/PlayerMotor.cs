//using System.Collections;
//using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{//INICIO DE SCRIPT PLAYER MOTOR
    
    // IG se refiere a "In general" a un valor general
    [Header("Movement")]
    public float movementSpeedIG;// El valor que utiliza el jugador todo momento para moverse
    private float normalMovementSpeed;// El valor del movimiento normal
    private float sprintMovementSpeed;// El valor del movimiento mientras corre
    private float crouchMovementSpeed;// El valor del movimiento cuando se agacha
    private float staminaIG;// El valor de la stamina
    private float axisX;// El valor X del jugador
    private float axisY;// El valor Y del jugador
    [Header("Amount Variables")]
    public float pistasAmount;// Las pistas encontradas por el jugador
    private float time_ = 0;//, time_FootstepSound = 0;

    [Header("Bool Variables")]
    public bool isOnBush = false;// Sí el Jugador está dentro de un arbusto
    public bool isDead;// Sí el jugador perdió
    public bool isCrouching;// Sí el jugador está agachado
    bool isWalking_Snd;
    bool isTakingObj = false;
    bool isPuttingObjDown = false;
    public bool isSliding; //--Bool si se esta deslizando el jugador
    [Header("GameObjects")]
    public GameObject visualPlayerActor;// El Objeto Visual del jugador
    AudioManager audioMan;
    [Header("Colliders")]
    public Collider2D PickUpItemCol; // El colisionador para tomar objetos
    //[Header("Transforms")]
    public RectTransform staminaBar_Transform;// El Transform de La barra de stamina del jugador
    //bool canGetRock;// Sí el jugador puede tomar una roca

    [Header("Script References")]
    public Animator anim;// El animador del jugador
    public PlayerMousePointer mousePointerScrpt;
    public RockSpawner RckSpawnerScrpt;
    public MG_Inventory inventory;
    public MG_GameManager gameManager;
    [Header("UI")]
    public Text pistasTextNum;



    //public Text staminaText;

    void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        normalMovementSpeed = 3;
        sprintMovementSpeed = 5.5f;
        crouchMovementSpeed = 1.5f;
        //anim = GetComponentInChildren<Animator>();
        staminaIG = 1;
        isDead = false;
        mousePointerScrpt = FindObjectOfType<PlayerMousePointer>();
        //audioMan = FindObjectOfType<AudioManager>(); // Ya estoy utilizando un codigo raro para acceder a esto
    }

    void Update()
    {//INICIO DE UPDATE
        //----Variables Constantes
        //Tiempo general
        time_ -= Time.deltaTime;
        //Axis de movimiento
        axisX = Input.GetAxisRaw("Horizontal");
        axisY = Input.GetAxisRaw("Vertical");
        pistasTextNum.text = pistasAmount + " / 5";

        //-----Movmiento >>>

        Vector3 mov = new Vector3(axisX,axisY,0);

        transform.position = Vector3.MoveTowards(
            transform.position,
            transform.position + mov,
            movementSpeedIG * Time.deltaTime
            );

        //-- Sonido Pasos jugador

        if (Mathf.Abs(axisX) > 0 || Mathf.Abs(axisY) > 0){
            if (isWalking_Snd == false){
                FindObjectOfType<AudioManager>().Play("PlayerSteps");
                isWalking_Snd = true;
            }
        } else {
            isWalking_Snd = false;
            FindObjectOfType<AudioManager>().Stop("PlayerSteps");
        }

        //-----Cambio de Velocidades >>>

        staminaBar_Transform.localScale = new Vector2(staminaIG, 1f); // Escala de la barra de stamina
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (staminaIG > .005)
            {
                staminaIG -= Time.deltaTime*0.45f; //Se gasta la stamina en una escala de 25
                movementSpeedIG = sprintMovementSpeed; // Cambia la velocidad a correr
            }
            else{
                movementSpeedIG = normalMovementSpeed; // Cambia la velocidad a caminar
            }
            //----Reducir velocidad cuando se esconde
            if (isOnBush == true){
            movementSpeedIG = normalMovementSpeed;
            }
        }
        else
        {
            if (staminaIG <= 1f && staminaIG > .10f){
                staminaIG += Time.deltaTime * .10f; //Se regenera la stamina
            }
            else if (staminaIG <= .10f){
                staminaIG += Time.deltaTime * .10f/5; //Se regenera la stamina muy lento Cuando esta regenerando desde un punto bajo
            }
            movementSpeedIG = normalMovementSpeed;
            //----Reducir velocidad cuando se esconde
            if (isOnBush == true){
            movementSpeedIG = crouchMovementSpeed;
        }
        }

        //----Cambio de lados en dirreccion Animacion >>>

        if (isCrouching == false){
            if (axisX < 0){
                visualPlayerActor.transform.localScale = new Vector3 (1f,1f,1f);
            }
            if (axisX > 0){
                visualPlayerActor.transform.localScale = new Vector3 (-1f,1f,1f);
            }
        }

        //Variable de ANIM, si el jugador se mueve verticalmente

        if (axisX < 0 || axisX > 0){
            anim.SetBool("MovingHor", true);
        }
        else {
            anim.SetBool("MovingHor", false);
        }
        

        //----Cambio de Animacion en movimiento >>>
        //anim.SetFloat("X", Mathf.Abs(axisX));// Valor Absoluto en axisX, solo necesito que sepa que se mueve, no a donde----Ya no aplica
        anim.SetFloat("X", axisX);
        anim.SetFloat("Y", axisY);

        //----Tomar roca >>>

        //-- Checar si presiona click izquierdo
        if (Input.GetButtonDown("Fire2")){
            isTakingObj = true;
            time_ = 0.5f;
        }
        if (isTakingObj == true)
        {
            //-- Animar tomando roca con sus tiempos
            anim.SetBool("PickUp", true);
            // Detener al jugador
            movementSpeedIG = 0f;
            if (time_ <= 0.1f){
                //-- Activar el colisionador
                PickUpItemCol.enabled = true;
                transform.Translate(.001f,.001f,0f); //-- Mover el colisionador un milimetro de nada para que funcione...
            }
            if (time_ <= 0f){
                //-- Desactivar el colisionador
                PickUpItemCol.enabled = false;
                anim.SetBool("PickUp", false);
                isTakingObj = false;
            }
        }

        //----Dejar roca en el suelo >>>

        //--Checar si presiona click derecho
        if (Input.GetButtonDown("Fire1") && isPuttingObjDown == false){
			for (int i = 0; i < inventory.slots.Length; i++)
			{
				if (inventory.isFull[i] == true){
					//-- El slot deja de estar lleno
					inventory.isFull[i] = false;
                    //-- Destruir los hijos del slot
                    if(inventory.slots[i].transform.childCount != 0){
                        Destroy(inventory.slots[i].transform.GetChild(0).gameObject);
                    }
                    //-- El Script puede acceder a el poner utilizar el objeto
                    isPuttingObjDown = true;
                    time_ = 0.2f;
					break;
				}
			}
        }
        if(isPuttingObjDown == true){
            //--Animar tomando roca con sus tiempos
            anim.SetBool("PutDown", true);
            //--Detener al jugador
            movementSpeedIG = 0f;

            if (time_ <= -0.1f){
                //Aparecer Roca con otro script
                //mousePointerScrpt.SpawnRock();
                //this.GetComponent<RockSpawner>().SpawnRock(); ///RELATIVO

                RckSpawnerScrpt.SpawnRock();
                anim.SetBool("PutDown", false);
                isPuttingObjDown = false;
            }
        }
        //----Agacharse >>

        if (Input.GetKey(KeyCode.C)){
            anim.SetBool("IsCrouching", true);
            movementSpeedIG = crouchMovementSpeed;
            isCrouching = true;
        } else {
            anim.SetBool("IsCrouching", false);
            isCrouching = false;
        }

        //----Deslizarse por Slope >>

        if (isSliding == true){
            transform.Translate (new Vector3(0f,-0.1f,0f));
        }

    }//FIN DEL UPDATE

    public void PlaySoundPlayer(int num){
        /* num = el numero de la cancion o sonido que se pondra
        1 = Ambiente1, 2 = Persecucion1Mons1, 3 = BushWalk
        */
        switch (num)
        {
            case 1:
            audioMan.Play("Ambient1");
            
            break;
            default:
            break;
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {//INICIO DE ON TRIGGER ENTER 2D
        if (col.tag == "Bush" || col.tag == "Big Bush"){
            //print("estoy escondido");
            movementSpeedIG = crouchMovementSpeed;
            isOnBush = true;
        }
        if(col.tag == "Rock"){
            print("estoy intentando agarrar una roca");
        }
        if (col.tag == "Slope Wall"){
            isSliding = true;
        }
        if (col.tag == "Finish"){
            gameManager.ExitFunctionGameplay();
        }
    }//FIN DE ON TRIGGER ENTER 2D

    public void OnCollisionEnter2D(Collision2D col)
    {//INICIO DE ON COLLISION ENTER 2D
        if (col.gameObject.tag == "Monster")
        {
            //-- El monstruo aclanzo al jugador
            gameManager.MonsterReachedPlayer();
        }
        /*if (col.gameObject.tag == "Rock")
        {
            if (canGetRock == true){
                PickUp(isRock:true,isBush:false);
            }
            //print("Jugador Toco Roca");
        }*/
    }//FIN DE CON COLLISION ENTER 2D

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Bush" || col.tag == "Big Bush"){
            //Deja de estar escondido en el arbusto
            isOnBush = false;
        }
        if (col.tag == "Slope Wall"){
            isSliding = false;
        }
    }

}//FIN DE SCRIPT PLAYER MOTOR
