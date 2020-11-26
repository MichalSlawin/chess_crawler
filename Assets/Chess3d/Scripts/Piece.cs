using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public string color; // black or white
    public float moveTime;

    private float timeCounter;
    private Vector3 startPosition;
    private Vector3 targetPosition;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        startPosition = targetPosition = transform.position;

        if (!color.Equals("white") && !color.Equals("black"))
        {
            throw new System.ArgumentException("color must be black or white", "color");
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        timeCounter += Time.deltaTime / moveTime;
        transform.position = Vector3.Lerp(startPosition, targetPosition, timeCounter);
    }

    public abstract List<Field> GetAvailableFields(Field startingField);

    public virtual void MoveTo(Field field)
    {
        targetPosition = new Vector3(field.transform.position.x, transform.position.y, field.transform.position.z);
        timeCounter = 0f;
        startPosition = transform.position;
    }
}
