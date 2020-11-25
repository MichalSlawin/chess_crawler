using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    private float moveDistance = 2f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    public override List<Field> GetAvailableFields(Field startingField)
    {
        List<Field> availableFields = new List<Field>();
        Field[] fields;
        fields = Field.FindObjectsOfType<Field>();
        
        Vector3 startingPosition = startingField.transform.position;

        foreach (Field field in fields)
        {
            Vector3 diff = field.transform.position - startingPosition;
            
            if(Mathf.Abs(diff.x) <= moveDistance && Mathf.Abs(diff.z) <= moveDistance)
            {
                availableFields.Add(field);
            }
        }
        return availableFields;
    }
}
