/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartHeal : MonoBehaviour
{

    [SerializeField] private int healAmount;

    HeartsHealthVisual visual;
    
    private void OnTriggerEnter2D(Collider2D collider) {
        personaje player = collider.GetComponent<personaje>();
        if (player != null) {
            // We hit the Player
            player.Heal(healAmount);

            if (!gameObject.CompareTag("CorazonDorado"))
            {
                return;
            }
            else
            {
                player.Sumacorazon(visual);
            } 
        }
    }
}