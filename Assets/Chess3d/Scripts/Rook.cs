using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override List<Field> GetAvailableFields()
    {
        List<Field> availableFields = new List<Field>();
        Field[] fields = Field.FindObjectsOfType<Field>();

        Vector3 startingPosition = occupiedField.transform.position;
        foreach (Field field in fields)
        {
            Vector3 diff = field.transform.position - startingPosition;
            if ((Mathf.Abs(diff.x) <= 0.1 && Mathf.Abs(diff.z) <= moveDistance) || (Mathf.Abs(diff.z) <= 0.1 && Mathf.Abs(diff.x) <= moveDistance))
            {
                if(!field.Occupied)
                {
                    availableFields.Add(field);
                }
                    
            }
        }

        return availableFields;
    }
    
    public override Field GetComputerFieldToMove()
    {
        List<Field> fields = GetAvailableFields();

        int index = Random.Range(0, fields.Count);

        return fields[index];
    }

    public override Piece GetComputerPieceToAttack()
    {
        Piece[] pieces = Piece.FindObjectsOfType<Piece>();

        Vector3 startingPosition = occupiedField.transform.position;
        foreach (Piece piece in pieces)
        {
            if(piece.tag == "PlayerControllable")
            {
                Vector3 diff = piece.occupiedField.transform.position - startingPosition;
                if ((Mathf.Abs(diff.x) <= 0.1 && Mathf.Abs(diff.z) <= moveDistance) || (Mathf.Abs(diff.z) <= 0.1 && Mathf.Abs(diff.x) <= moveDistance))
                {
                    return piece;
                }
            }
        }
        return null;
    }
}
