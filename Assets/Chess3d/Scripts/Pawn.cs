using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override List<Field> GetAvailableFields()
    {
        List<Field> fields = new List<Field>();

        return fields;
    }
}
