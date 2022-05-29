using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA_Patrulla_NPC : MonoBehaviour
{
    private NavMeshAgent agente;
    private Animator miAnimacion;
    public Collider2D colliderEnemigo;
    public float maxDistancia;
    public float minDistancia;
    private float tiempoEspera;
    public float inicioTiempoEspera;
    public Transform[] zonas;

    private int zonaAleatoria;


    public GameObject[] listaItems;

    // private Rigidbody2D rb2D;

    private void Awake()
    {
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

    }

    // Update is called once per frame
    void Update()
    {

        if (!GameObject.FindWithTag("Player"))
        {

            Patrulla();

        }
        else if (Vector2.Distance(GameObject.FindWithTag("Player").transform.position, transform.position) >= maxDistancia)
        {

            Patrulla();

        }
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

    public Vector3 GetPosition()
    {
        return transform.position;
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
