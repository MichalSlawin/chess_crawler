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
        List<Field> availableFields = new List<Field>();
        Field[] fields = Field.FindObjectsOfType<Field>();

        Vector3 startingPosition = occupiedField.transform.position;
        foreach (Field field in fields)
        {
            Vector3 diff = field.transform.position - startingPosition;
            if (Mathf.Abs(diff.x) <= 0.1 && Mathf.Abs(diff.z) <= moveDistance && diff.z < 0)
            {
                availableFields.Add(field);
            }
        }

        if (availableFields.Count > 1)
        {
            Debug.Log(availableFields.Count);
            throw new System.Exception("Pawn cannot have more than 1 field available");
        }

        return availableFields;
    }

    public override Field GetComputerFieldToMove()
    {
        List<Field> fields = GetAvailableFields();
        Field fieldToMove = null;

        foreach(Field field in fields)
        {
            fieldToMove = field;
        }

        return fieldToMove;
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
                if (Mathf.Abs(diff.x) >= 1 && Mathf.Abs(diff.x) <= moveDistance && Mathf.Abs(diff.z) <= moveDistance && diff.z < 0)
                {
                    return piece;
                }
            }
        }
        return null;
    }
}
