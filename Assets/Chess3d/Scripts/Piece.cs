using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public string color; // black or white

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if(!color.Equals("white") && !color.Equals("black"))
        {
            throw new System.ArgumentException("color must be black or white", "color");
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public abstract List<Field> GetAvailableFields(Field startingField);

    
}
