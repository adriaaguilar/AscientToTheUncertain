using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using TMPro;

public class ManejadorJuego : MonoBehaviourPunCallbacks
{

    [Header("Jugadores")]
    [SerializeField] private string obtJugadorPrefab;
    [SerializeField] private Transform[] posiciones;

    /*[Header("Enemigos")]
    [SerializeField] private string obtEnemigoPrefab;
    [SerializeField] private Transform[] zonas;*/

    public static ManejadorJuego instancia;

    private void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        OnJoinedRoom();
    }

    void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
            else
            {
                PhotonNetwork.CurrentRoom.IsVisible = true;
                PhotonNetwork.CurrentRoom.IsOpen = true;
            }
        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            GameObject jugadorObj =
            PhotonNetwork.Instantiate(obtJugadorPrefab,
            posiciones[Random.Range(0, posiciones.Length)].position,
            Quaternion.identity);

            jugadorObj.GetComponent<personaje>().Inicializar(PhotonNetwork.LocalPlayer);
        }
    }
}
