using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;

public class Spikes : MonoBehaviourPun
{
    [SerializeField] private int damageAmount;

    private  void OnTriggerEnter2D(Collider2D collider)
    {
        

        personaje player = collider.GetComponent<personaje>();

        
        IA_Patrulla2 enemigo = collider.GetComponent<IA_Patrulla2>();

        IA_Jefe jefe = collider.GetComponent<IA_Jefe>();
        
        
        if(player != null)
        {
            //We hit the player
            Vector3 knockbackDir = (player.GetPosition() - transform.position).normalized;
            player.DamageKnockback(knockbackDir, 0.2f, damageAmount);
        }
        if(enemigo != null){
            Vector3 knockbackDir = (enemigo.GetPosition() - transform.position).normalized;
            enemigo.DamageKnockback(knockbackDir, 0.2f, damageAmount);
        }
        if(jefe != null){
            Vector3 knockbackDir = (jefe.GetPosition() - transform.position).normalized;
            jefe.DamageKnockback(knockbackDir, 0.2f, damageAmount);
        }
    }
}
