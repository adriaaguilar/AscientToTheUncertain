using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sonidos_fondo : MonoBehaviour
{
    public GameObject sonido_exterior;
    public GameObject sonido_interior;
    public GameObject sonido_jefe;

    private int switch_sonido_int;

    // Start is called before the first frame update
    void Start()
    {
        switch_sonido_int = 0;

        sonido_exterior.GetComponent<AudioSource>().Play();
    }

    public void switch_audio()
    {
        if (switch_sonido_int == 0)
        {
            sonido_exterior.GetComponent<AudioSource>().Stop();
            sonido_interior.GetComponent<AudioSource>().Play();
            switch_sonido_int = 1;
        }
        else
        {
            sonido_interior.GetComponent<AudioSource>().Stop();
            sonido_exterior.GetComponent<AudioSource>().Play();
            switch_sonido_int = 0;
        }
    }

    public void switch_audio_jefe(int num)
    {
        if (num == 0)
        {
            sonido_exterior.GetComponent<AudioSource>().Stop();
            sonido_jefe.GetComponent<AudioSource>().Play();
        }
        else
        {
            sonido_jefe.GetComponent<AudioSource>().Stop();
            sonido_exterior.GetComponent<AudioSource>().Play();
        }
    }
}
