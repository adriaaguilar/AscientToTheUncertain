using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbreteSesamo : MonoBehaviour
{
    private Animator llaveAnim;
    private GameObject Porton;
    private Animator portonAnim;

    // Start is called before the first frame update
    void Start()
    {
        llaveAnim = GetComponent<Animator>();

        Porton = GameObject.Find("Porton");
        portonAnim = Porton.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            llaveAnim.SetBool("Desaparece", true);
            Destroy(gameObject, 0.9f);

            portonAnim.SetBool("seAbre", true);
            Destroy(Porton, 1.5f);

        }

    }
}
