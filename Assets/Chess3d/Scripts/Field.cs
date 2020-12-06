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

    public void DestroyField()
    {
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
            this.Destroyed = true;
            Destroy(this.gameObject, 5f);
        }
    }
}
