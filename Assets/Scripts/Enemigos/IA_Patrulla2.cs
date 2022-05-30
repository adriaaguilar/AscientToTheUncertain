using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CodeMonkey.Utils;
using System;
using Random = UnityEngine.Random;

public class IA_Patrulla2 : MonoBehaviour
{
    private NavMeshAgent agente;
    private Animator miAnimacion;
    public Collider2D colliderEnemigo;
    public float maxDistancia;
    public float minDistancia;
    private float tiempoEspera;
    public float inicioTiempoEspera;
    public event EventHandler OnItemListChanged;
    public Transform[] zonas;

    public int vida;
    private int zonaAleatoria;

    public int dañoEnemigo;
    public float velocidadAtaque = 1f;
    private float puedeAtacar = 1f;

    private bool Muerto = false;
    private List<Item> itemList;

    // private Rigidbody2D rb2D;


   
    private void Awake() {
        agente = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        agente.updateRotation = false;
        agente.updateUpAxis = false;
        tiempoEspera = inicioTiempoEspera;
        zonaAleatoria = Random.Range(0, zonas.Length);
        miAnimacion = GetComponent<Animator>();
        // rb2D = GetComponent<Rigidbody2D>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!GameObject.FindWithTag("Player")){

            Patrulla();

        }else if(Muerto){

            Muerto = false;
            Invoke("dropItem", 1.5f);

        }else if (vida <= 0)
        {

            HaMuerto();
        
        }else if (Vector2.Distance(GameObject.FindWithTag("Player").transform.position, transform.position) <= maxDistancia && Vector2.Distance(GameObject.FindWithTag("Player").transform.position, transform.position) >= minDistancia)
        {
            SeguirJugador();

        }
        else if (Vector2.Distance(GameObject.FindWithTag("Player").transform.position, transform.position) >= maxDistancia)
        {
            
            Patrulla();

        }
        else if (Vector2.Distance(GameObject.FindWithTag("Player").transform.position, colliderEnemigo.transform.position) < minDistancia)
        {
            atacarJugador();

        }
    }

    public void dropItem(){

        itemList = new List<Item>();
        AddItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1 });

        var itemIndex = Random.Range(0, itemList.Count);


        // Item duplicateItem = new Item { itemType = item.itemType, amount = 1 };

        // ItemWorld.DropItem(transform.position, duplicateItem);

        DropItem(transform.position, itemList[itemIndex]);
        
        
    }

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }
    public static ItemWorld DropItem(Vector3 dropPosition, Item item)
    {
        Vector3 randomDir = UtilsClass.GetRandomDir();
        Debug.Log(randomDir);
        ItemWorld itemWorld = SpawnItemWorld(dropPosition + randomDir * 0f, item);
        //itemWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 0.75f, ForceMode2D.Impulse);-->no se molt be quin impuls donar-li.

        return itemWorld;
    }

    public void HaMuerto(){

        /* rb2D.velocity = Vector2.zero;
        rb2D.angularVelocity = 0f; */
        
        miAnimacion.SetBool("seMueve", false);
        miAnimacion.SetBool("seMata", true);
        Destroy(gameObject, 1.5f);
        
    }

    public void SeguirJugador()
    {
        
        miAnimacion.SetBool("seMueve", true);
        miAnimacion.SetBool("meAtaca", false);
        miAnimacion.SetFloat("moverX", (GameObject.FindGameObjectWithTag("Player").transform.position.x - transform.position.x));
        miAnimacion.SetFloat("moverY", (GameObject.FindGameObjectWithTag("Player").transform.position.y - transform.position.y));
        // transform.position = Vector2.MoveTowards(transform.position, GameObject.FindWithTag("Player").transform.position, velocidad * Time.deltaTime);
        agente.SetDestination(GameObject.FindWithTag("Player").transform.position);
    }

    public void Patrulla()
    {
        
        miAnimacion.SetBool("seMueve", true);
        miAnimacion.SetFloat("moverX", (zonas[zonaAleatoria].transform.position.x - transform.position.x));
        miAnimacion.SetFloat("moverY", (zonas[zonaAleatoria].transform.position.y - transform.position.y));
        // transform.position = Vector2.MoveTowards(transform.position, zonas[zonaAleatoria].position, velocidad * Time.deltaTime);
        agente.SetDestination(zonas[zonaAleatoria].position);

        if (Vector2.Distance(transform.position, zonas[zonaAleatoria].position) <= 0.25f)
        {
            if (tiempoEspera <= 0)
            {
                zonaAleatoria = Random.Range(0, zonas.Length);
                tiempoEspera = inicioTiempoEspera;
                miAnimacion.SetBool("seMueve", true);
            }
            else
            {
                miAnimacion.SetBool("seMueve", false);
                tiempoEspera -= Time.deltaTime;
            }


        }
    }

    public void atacarJugador()
    {
        
        miAnimacion.SetBool("meAtaca", true);
        miAnimacion.SetFloat("moverX", (GameObject.FindGameObjectWithTag("Player").transform.position.x - transform.position.x));
        miAnimacion.SetFloat("moverY", (GameObject.FindGameObjectWithTag("Player").transform.position.y - transform.position.y));
        
    }

    public void DamageKnockback(Vector3 knockbackDir, float knockbackDistance, int damageAmount)
    {
        // transform.position += knockbackDir * knockbackDistance;
        vida -= damageAmount;
        miAnimacion.SetBool("seMueve", false);
        miAnimacion.SetBool("meAtaca", false);
        if (vida <= 0)
        {
            Muerto = true;

        }else{
            miAnimacion.Play("Daño");
        }
        
        
       
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void OnTriggerStay2D(Collider2D col) {
        personaje player = col.GetComponent<personaje>();
        if (player != null)
        {
            if(velocidadAtaque <= puedeAtacar){
                Vector3 knockbackDir = (player.GetPosition() - transform.position).normalized;
                player.DamageKnockback(knockbackDir, 0f, dañoEnemigo);
                puedeAtacar = 0f;
            }else{
                puedeAtacar +=Time.deltaTime;
            }
            
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.CompareTag("tejado_casa"))
        {
            Debug.Log("tocando tejado casa");

            SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
            rend.sortingOrder = -1;

            Debug.Log("Layer order: " + rend.sortingOrder);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "tejado_casa")
        {
            Debug.Log("saliendo tejado casa");

            SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
            rend.sortingOrder = 2;

            Debug.Log("Layer order: " + rend.sortingOrder);
        }
    }
}
