using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightScript : MonoBehaviour
{
    Light luzFuego;
    float LuzFloat;
    public float minFloat = 3f, maxFloat = 5f;
    // Start is called before the first frame update
    void Start()
    {
        luzFuego = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        LuzFloat = Random.Range(minFloat, maxFloat);
        luzFuego.intensity = LuzFloat;
    }
}
