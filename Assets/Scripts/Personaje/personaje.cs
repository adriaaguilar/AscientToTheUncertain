using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;
using Cinemachine;

public class personaje : MonoBehaviourPun
{
    [SerializeField] private int damageAmount;
    private Collider2D colliderEspada;
    
    private GameObject sonidos;

    //inventory part
    private UI_Inventory uiInventory;

    private Inventory inventory;
    //
    private int id;
    private string nombre;
    private string color;
    private Color Setcolor;
    private int interactuar;

    [Header("Acciones")]
    [SerializeField] private Player photonJugador;
    private GameObject currentTeleporter;
    
    private float moveSpeed;
    float secondsCounter;
    float secondsToCount;
    float ataqueAnimCounter;
    float ataqueAnimToCount;
    float ataqueCounter;
    float ataqueToCount;
    bool teleport;
    bool atacar;
    bool ataqueCOUNT;
    Vector2 direccion;
    string direccion_ataque;
    float moviminetox;
    float moviminetoy;
    private CinemachineVirtualCamera cam;

    private Rigidbody2D rb;

    //public Animator animacionAtaque;
    private Animator transicionCasa;
    void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        else
        {
            uiInventory = GameObject.FindGameObjectWithTag("UI_Inventory").GetComponent<UI_Inventory>();
            inventory = new Inventory(UseItem);
            uiInventory.SetPlayer(this);
            uiInventory.SetInventory(inventory);
        }

        colliderEspada = gameObject.transform.Find("Ataque").GetComponent<Collider2D>();
        colliderEspada.enabled = false;
        transicionCasa = GameObject.FindGameObjectWithTag("TransicionCirculo").GetComponent<Animator>();
        moveSpeed = 1f;
        interactuar = 0;
        secondsCounter = 0f;
        secondsToCount = 1f;
        ataqueAnimCounter = 0f;
        ataqueAnimToCount = 0.5f;
        ataqueCounter = 0f;
        ataqueToCount = 1.2f;
        teleport = false;
        atacar = false;
        ataqueCOUNT = false;
        direccion = new Vector2(0, 0);
        direccion_ataque = "null";

        rb = gameObject.GetComponent<Rigidbody2D>();

        if (!photonView.IsMine)
        {
            return;
        }
        else
        {
            cam = GameObject.FindGameObjectWithTag("virtualCamera").GetComponent<CinemachineVirtualCamera>();
            cam.Follow = gameObject.transform;

            sonidos = GameObject.Find("sonidos");
        }
    }

    //inventory method
    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:
                HeartsHealthVisual.heartsHealthSystemStatic.Heal(4);
                inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
                break;
            case Item.ItemType.ManaPotion:
                inventory.RemoveItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1 });
                break;
        }
    }
    //
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        else
        {
            photonView.RPC("changeAnimator", RpcTarget.AllBuffered, moviminetox, moviminetoy);

            if (interactuar == 1)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                
                    transicionCasa.Play("inicioCirculoAnim");

                    sonidos.GetComponent<sonidos_fondo>().switch_audio();

                    teleport = true;
                    Invoke("finTransicion", 2);
                    Invoke("teleportPlayer", 1);
                }

            }

            if (teleport == true)
            {
                secondsCounter += Time.deltaTime;
                if (secondsCounter >= secondsToCount)
                {
                    teleport = false;
                    secondsCounter = 0f;

                    photonView.RPC("switchSprite", RpcTarget.AllBuffered);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && atacar == false)
            {
                colliderEspada.enabled = true;

                atacar = true;
                Debug.Log("Ataca");
                photonView.RPC("changeAtaqueON", RpcTarget.AllBuffered);

                ataqueCOUNT = true;
            }

            if (ataqueCOUNT == true)
            {
                ataqueAnimCounter += Time.deltaTime;
                if (ataqueAnimCounter >= ataqueAnimToCount)
                {
                    ataqueCOUNT = false;
                    ataqueAnimCounter = 0f;
                    photonView.RPC("changeAtaqueOFF", RpcTarget.AllBuffered);
                }
            }

            if (atacar == true)
            {
                photonView.RPC("freeze", RpcTarget.AllBuffered);

                ataqueCounter += Time.deltaTime;
                if (ataqueCounter >= ataqueToCount)
                {
                    photonView.RPC("unfreeze", RpcTarget.AllBuffered);
                    ataqueCounter = 0f;
                    atacar = false;
                    Debug.Log("Deja de atacar");

                    colliderEspada.enabled = false;
                }
            }

        }
    }

    [PunRPC]
    public void freeze()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    [PunRPC]
    public void unfreeze()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void finTransicion()
    {
        transicionCasa.Play("finCirculoAnim");
    }

    public void teleportPlayer(){
        photonView.RPC("switchSprite", RpcTarget.AllBuffered);
        transform.position = currentTeleporter.GetComponent<teleporter>().GetDestination().position;

        var confiner = cam.GetComponent<CinemachineConfiner>();

        if (currentTeleporter.tag == "puerta_interior")
        {
            confiner.InvalidatePathCache();
            confiner.m_BoundingShape2D = GameObject.FindGameObjectWithTag("confinerUno").GetComponent<PolygonCollider2D>();

        }
        else if(currentTeleporter.tag == "puerta_exterior0"){

            confiner.InvalidatePathCache();
            confiner.m_BoundingShape2D = GameObject.FindGameObjectWithTag("confinerInterior0").GetComponent<PolygonCollider2D>();
           

        }
        else if (currentTeleporter.tag == "puerta_exterior1")
        {

            confiner.InvalidatePathCache();
            confiner.m_BoundingShape2D = GameObject.FindGameObjectWithTag("confinerInterior1").GetComponent<PolygonCollider2D>();


        }
        else if (currentTeleporter.tag == "puerta_exterior2")
        {

            confiner.InvalidatePathCache();
            confiner.m_BoundingShape2D = GameObject.FindGameObjectWithTag("confinerInterior2").GetComponent<PolygonCollider2D>();


        }
        else if (currentTeleporter.tag == "puerta_exterior3")
        {

            confiner.InvalidatePathCache();
            confiner.m_BoundingShape2D = GameObject.FindGameObjectWithTag("confinerInterior3").GetComponent<PolygonCollider2D>();


        }
        else if (currentTeleporter.tag == "puerta_exterior4")
        {

            confiner.InvalidatePathCache();
            confiner.m_BoundingShape2D = GameObject.FindGameObjectWithTag("confinerInterior4").GetComponent<PolygonCollider2D>();


        }

    }

    void FixedUpdate(){
        if (!photonView.IsMine)
        {
            return;
        }
        else
        {
            if (teleport == false)
            {
                moviminetox = Input.GetAxisRaw("Horizontal");
                moviminetoy = Input.GetAxisRaw("Vertical");

                direccion = new Vector2(moviminetox, moviminetoy).normalized;
                rb.MovePosition(rb.position + direccion * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "tejado_casa")
        {
            Debug.Log("tocando tejado casa");

            SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
            rend.sortingOrder = -1;

            Debug.Log("Layer order: "+ rend.sortingOrder);
        }

        if (col.gameObject.tag == "puerta_exterior0" || col.gameObject.tag == "puerta_exterior1" || col.gameObject.tag == "puerta_exterior2" || col.gameObject.tag == "puerta_exterior3" || col.gameObject.tag == "puerta_exterior4" || col.gameObject.tag == "puerta_interior")
        {
            currentTeleporter = col.gameObject;
            interactuar = 1;
        }

        if (col.gameObject.tag == "sonido_jefe")
        {
            sonidos.GetComponent<sonidos_fondo>().switch_audio_jefe(0);
        }

        ItemWorld itemWorld = col.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            else
            {
                // Touching Item
                inventory.AddItem(itemWorld.GetItem());
                itemWorld.DestroySelf();
            }
        }

        IA_Patrulla2 enemigo = col.GetComponent<IA_Patrulla2>();

        IA_Jefe jefe = col.GetComponent<IA_Jefe>();


        if (enemigo != null && colliderEspada.enabled)
        {
            Vector3 knockbackDir = (enemigo.GetPosition() - transform.position).normalized;
            enemigo.DamageKnockback(knockbackDir, 0.2f, damageAmount);
        }
        if (jefe != null && colliderEspada.enabled)
        {
            Vector3 knockbackDir = (jefe.GetPosition() - transform.position).normalized;
            jefe.DamageKnockback(knockbackDir, 0.2f, damageAmount);
        }
        
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "tejado_casa")
        {
            Debug.Log("saliendo tejado casa");

            SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
            rend.sortingOrder = 3;

            Debug.Log("Layer order: " + rend.sortingOrder);
        }

        if (col.gameObject.tag == "puerta_exterior0" || col.gameObject.tag == "puerta_exterior1" || col.gameObject.tag == "puerta_exterior2" || col.gameObject.tag == "puerta_exterior3" || col.gameObject.tag == "puerta_exterior4" || col.gameObject.tag == "puerta_interior")
        {
            currentTeleporter = null;
            interactuar = 0;
        }

        if (col.gameObject.tag == "sonido_jefe")
        {
            sonidos.GetComponent<sonidos_fondo>().switch_audio_jefe(1);
        }
    }

    public void Inicializar(Player NetworkPlayer)
    {
        id = NetworkPlayer.ActorNumber;
        nombre = NetworkPlayer.NickName;
        photonJugador = NetworkPlayer;

        Debug.Log($"nombreJugador = {nombre}");
        Debug.Log("Current layer: " + gameObject.layer);
        //photonView.RPC("setColor", RpcTarget.AllBuffered, nombre);
    }

    [PunRPC]
    void changeAnimator(float moviminetox, float moviminetoy)
    {
        Animator animator = gameObject.GetComponent<Animator>();

        animator.SetFloat("MovimientoX", moviminetox);
        animator.SetFloat("MovimientoY", moviminetoy);

        if (moviminetox != 0 || moviminetoy != 0)
        {
            animator.SetFloat("UltimoX", moviminetox);
            animator.SetFloat("UltimoY", moviminetoy);
        }

        if (moviminetox == 0 && moviminetoy == -1 || moviminetox == -1 && moviminetoy == -1 || moviminetox == 1 && moviminetoy == -1)
        {
            direccion_ataque = "frontal";
        }
        else if (moviminetox == -1 && moviminetoy == 0)
        {
            direccion_ataque = "izquierda";
        }
        else if (moviminetox == 0 && moviminetoy == 1 || moviminetox == -1 && moviminetoy == 1 || moviminetox == 1 && moviminetoy == 1)
        {
            direccion_ataque = "espalda";
        }
        else if (moviminetox == 1 && moviminetoy == 0)
        {
            direccion_ataque = "derecha";
        }
    }

    [PunRPC]
    void changeAtaqueON()
    {
        Debug.Log("Ataque ON");
        Animator animator = gameObject.GetComponent<Animator>();

        if (direccion_ataque == "frontal")
        {
            Debug.Log(direccion_ataque);
            animator.SetBool("ataqueFrontal", true);
        }else if (direccion_ataque == "espalda")
        {
            Debug.Log(direccion_ataque);
            animator.SetBool("ataqueEspalda", true);
        }
        else if (direccion_ataque == "izquierda")
        {
            Debug.Log(direccion_ataque);
            animator.SetBool("ataqueIzquierda", true);
        }
        else if (direccion_ataque == "derecha")
        {
            Debug.Log(direccion_ataque);
            animator.SetBool("ataqueDerecha", true);
        }
    }

    [PunRPC]
    void changeAtaqueOFF()
    {
        Debug.Log("Ataque OFF");
        Animator animator = gameObject.GetComponent<Animator>();

        animator.SetBool("ataqueFrontal", false);
        animator.SetBool("ataqueEspalda", false);
        animator.SetBool("ataqueIzquierda", false);
        animator.SetBool("ataqueDerecha", false);
    }

    [PunRPC]
    void switchSprite()
    {
        SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();

        if (rend.enabled == false)
        {
            rend.enabled = true;
        }
        else
        {
            rend.enabled = false;
        }
    }

    [PunRPC]
    void setColor(string nombre)
    {
        StartCoroutine(datos(nombre));
    }

    IEnumerator datos(string nombre)
    {
        UnityWebRequest conectionColor = UnityWebRequest.Get("https://juegoprueba46.000webhostapp.com/color.php?nombre=" + nombre);
        yield return conectionColor.SendWebRequest();

        color = conectionColor.downloadHandler.text;

        if (color == "red")
        {
            Setcolor = Color.red;
        }
        else if(color == "blue")
        {
            Setcolor = Color.blue;
        }
        else if (color == "green")
        {
            Setcolor = Color.green;
        }
        else if (color == "yellow")
        {
            Setcolor = Color.yellow;
        }
        else
        {
            Setcolor = Color.black;
        }

        Debug.Log($"ColorJugador = {Setcolor}");
        //gameObject.GetComponent<SpriteRenderer>().color = Setcolor;
    }

    public void DamageKnockback(Vector3 knockbackDir, float knockbackDistance, int damageAmount)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        else
        {
            //transform.position += knockbackDir * knockbackDistance;
            // DamageFlash();
            HeartsHealthVisual.heartsHealthSystemStatic.Damage(damageAmount);
        }
    }
    public void Heal(int healAmount)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        else
        {
            HeartsHealthVisual.heartsHealthSystemStatic.Heal(healAmount);
        }
        /*materialTintColor = new Color(0, 1, 0, 1f);
        material.SetColor("_Tint", materialTintColor);*/
    }

    public void Sumacorazon(HeartsHealthVisual visual)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        else
        {
            visual = GameObject.FindGameObjectWithTag("Corazones").GetComponent<HeartsHealthVisual>();
            visual.SumaVidas();
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
