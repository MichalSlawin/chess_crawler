using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public string color; // black or white
    private bool available = false;
    private bool occupied = false;
    private bool destroyed = false;

    public bool Available { get => available; set => available = value; }
    public bool Occupied { get => occupied; set => occupied = value; }
    public bool Destroyed { get => destroyed; set => destroyed = value; }

    // Start is called before the first frame update
    void Start()
    {
        if(!color.Equals("white") && !color.Equals("black") && !color.Equals("golden"))
        {
            throw new System.ArgumentException("field color must be black, white or golden", "color");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
