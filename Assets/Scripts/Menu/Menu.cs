using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Menu : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public TMP_InputField inpCorreo;
    public TMP_InputField inpPassword;

    [Header("Pantallas")]
    [SerializeField] private GameObject menuPrincipal;
    [SerializeField] private GameObject crearRoom;
    [SerializeField] private GameObject unirseRoom;
    [SerializeField] private GameObject login;

    [Header("Login")]
    [SerializeField] private Button btnEntrar;

    [Header("Menu principal")]
    [SerializeField] private Button btnCrearRoom;
    [SerializeField] private Button btnUnirseRoom;

    [Header("Pantalla unirse")]
    [SerializeField] private RectTransform listadoSalas;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private Button btnUnirse;
    [SerializeField] private Button btnAtras;

    private string nombre;
    private List<GameObject> roomElementos = new List<GameObject>();
    private List<RoomInfo> listaRooms = new List<RoomInfo>();

    void Start()
    {
        btnCrearRoom.interactable = false;
        btnUnirseRoom.interactable = false;

        if (PhotonNetwork.InRoom){
            PhotonNetwork.CurrentRoom.IsVisible = true;
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }

    public override void OnConnectedToMaster()
    {
        btnCrearRoom.interactable = true;
        btnUnirseRoom.interactable = true;

        Debug.Log("Se ha conectado correctamente");
    }

    void SetPantalla(GameObject screen){
        menuPrincipal.SetActive(false);
        crearRoom.SetActive(false);
        unirseRoom.SetActive(false);
        login.SetActive(false);

        screen.SetActive(true);

        if (screen == unirseRoom){
            ActualizarUnirseRoom();
        }
    }

    private GameObject CrearRoomBotton()
    {
        GameObject obj = Instantiate(roomPrefab, listadoSalas.transform);
        roomElementos.Add(obj);
        return obj;
    }
    void ActualizarUnirseRoom()
    {
        foreach (GameObject b in roomElementos)
        {
            b.SetActive(false);
        }

        for (int x=0; x < listaRooms.Count; x++)
        {
            GameObject boton = x >= roomElementos.Count ? CrearRoomBotton() : roomElementos[x];
            boton.SetActive(true);

            boton.transform.Find("Nombre").GetComponent<Text>().text = listaRooms[x].Name;
            boton.transform.Find("jugadoresVFX").GetComponent<Text>().text = listaRooms[x].PlayerCount + "/" + listaRooms[x].MaxPlayers;

            Button b1 = boton.GetComponent<Button>();
            string nombre = listaRooms[x].Name;
            b1.onClick.RemoveAllListeners();
            b1.onClick.AddListener(() => { OnUnirseRoomBotton(nombre); });
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        listaRooms = roomList;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnCorreoCambia(TMP_InputField inpCorreo)
    {
        string correo = inpCorreo.text;
        Debug.Log($"correo = {correo}");
    }

    public void OnPasswordCambia(TMP_InputField inpPassword)
    {
        string password = inpPassword.text;
        Debug.Log($"password = {password}");
    }

    public void OnLoginClicked()
    {
        setNombre();
    }

    public void OnRegistroClicked()
    {
        Application.OpenURL("https://juegoprueba46.000webhostapp.com/");
    }

    public void OnCrearRoomClicked(){
        SetPantalla(crearRoom);
    }

    public void OnUnirseRoomClicked(){
        SetPantalla(unirseRoom);
    }

    public void OnAtrasClicked(){
        SetPantalla(menuPrincipal);
    }

    public void OnLogOutClicked()
    {
        inpCorreo.text = "";
        inpPassword.text = "";
        PhotonNetwork.NickName = null;
        SetPantalla(login);
        Debug.Log($"Nombre del jugador = {PhotonNetwork.NickName}");

    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnCrearRoomBotton(TMP_InputField inpSalaNombre)
    {
        networkManager.instancia.CrearRoom(inpSalaNombre.text);
    }

    public void OnUnirseRoomBotton(string nombre)
    {
        networkManager.instancia.UnirseRoom(nombre);
    }
    public void OnUnirseRoomBottonFind(TMP_InputField inpSalaNombre)
    {
        networkManager.instancia.UnirseRoom(inpSalaNombre.text);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void setNombre()
    {
        StartCoroutine(datos());
    }

    IEnumerator datos()
    {
        UnityWebRequest conectionNombre = UnityWebRequest.Get("https://juegoprueba46.000webhostapp.com/modelos/login.php?correo=" + inpCorreo.text+"&password="+inpPassword.text);
        yield return conectionNombre.SendWebRequest();

        nombre = conectionNombre.downloadHandler.text;

        if (nombre == "404")
        {
            Debug.Log("Usuario o contraseña incorrectos");
        }
        else
        {
            Debug.Log("Log Correcto, entrando...");
            Debug.Log($"Nombre del jugador = {nombre}");
            PhotonNetwork.NickName = nombre;

            SetPantalla(menuPrincipal);
        }
    }

    void Update()
    {
        
    }
}
