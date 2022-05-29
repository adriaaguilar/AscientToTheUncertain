using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class networkManager : MonoBehaviourPunCallbacks
{
    public int maximoJugadores = 5;
    public static networkManager instancia;

    private void Awake(){
        instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectados al servidor principal");
        PhotonNetwork.JoinLobby();
    }

    public void CrearRoom(string nombre)
    {
        RoomOptions opciones = new RoomOptions
        {
            MaxPlayers = (byte)maximoJugadores
        };

        PhotonNetwork.CreateRoom(nombre, opciones);
        Debug.Log("Se ha creado correctamente la sala: ");
        Debug.Log($"nombre = {nombre}");

        CambiarEscena("Juego");
    }

    public void UnirseRoom(string nombre)
    {
        PhotonNetwork.JoinRoom(nombre);
        Debug.Log("Se ha unido a la sala: ");
        Debug.Log($"nombre = {nombre}");

        CambiarEscena("Juego");
    }

    public void CambiarEscena(string escena)
    {
        PhotonNetwork.LoadLevel(escena);
    }

    void Update()
    {
        
    }
}