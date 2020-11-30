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
            if(IsFieldAvailable(field, startingPosition) && !field.Occupied && !field.Destroyed)
            {
                availableFields.Add(field);
            }
        }

        return availableFields;
    }

    private bool IsFieldAvailable(Field field, Vector3 startingPosition)
    {
        Vector3 fieldPosition = field.transform.position;
        Vector3 diff = fieldPosition - startingPosition;
        if ((Mathf.Abs(diff.x) <= 0.1 && Mathf.Abs(diff.z) <= moveDistance) || (Mathf.Abs(diff.z) <= 0.1 && Mathf.Abs(diff.x) <= moveDistance))
        {
            bool blocked = false;
            Piece[] pieces = Piece.FindObjectsOfType<Piece>();
            foreach (Piece piece in pieces)
            {
                if (piece.tag == "ComputerControllable" && !piece.Equals(this) && !piece.IsDead)
                {
                    Vector3 piecePosition = piece.occupiedField.transform.position;
                    Vector3 pieceDiff = piecePosition - startingPosition;
                    if ((Mathf.Abs(diff.x) <= 0.1 && Mathf.Abs(pieceDiff.x) <= 0.1) || (Mathf.Abs(diff.z) <= 0.1 && Mathf.Abs(pieceDiff.z) <= 0.1))
                    {
                        if ((piecePosition.x < startingPosition.x && piecePosition.x > fieldPosition.x) || (piecePosition.x > startingPosition.x && piecePosition.x < fieldPosition.x)
                            || (piecePosition.z < startingPosition.z && piecePosition.z > fieldPosition.z) || (piecePosition.z > startingPosition.z && piecePosition.z < fieldPosition.z))
                        {
                            blocked = true;
                            break;
                        }

                    }
                }
            }
            if (!blocked) return true;
        }
        return false;
    }
    
    public override Field GetComputerFieldToMove()
    {
        List<Field> fields = GetAvailableFields();

        int index = Random.Range(0, fields.Count);

        if (fields.Count == 0) return null;
        return fields[index];
    }

    public override Piece GetComputerPieceToAttack()
    {
        Piece[] pieces = Piece.FindObjectsOfType<Piece>();

        Vector3 startingPosition = occupiedField.transform.position;
        foreach (Piece piece in pieces)
        {
            if (piece.tag == "PlayerControllable" && IsFieldAvailable(piece.occupiedField, startingPosition))
            {
                return piece;
            }
        }
        return null;
    }
}
