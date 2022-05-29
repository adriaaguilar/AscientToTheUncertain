using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using TMPro;

public class ManejadorCasa : MonoBehaviour
{
    [Header("Jugadores")]
    [SerializeField] private string obtJugadorPrefab;
    [SerializeField] private Transform posicion;

    // Start is called before the first frame update
    public static ManejadorCasa instancia;

    private void Awake()
    {
        instancia = this;
    }
    void Start()
    {
        Debug.Log("Nombre de la sala: "+PhotonNetwork.CurrentRoom.Name);

        if (PhotonNetwork.InRoom)
        {
            GameObject jugadorObj =
            PhotonNetwork.Instantiate(obtJugadorPrefab,
            posicion.position,
            Quaternion.identity);

            jugadorObj.GetComponent<personaje>().Inicializar(PhotonNetwork.LocalPlayer);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
