using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public string color; // black or white
    public float moveTime;
    public Field occupiedField;
    public Field goToField = null;
    public float moveDistance = 2.1f;
    public bool waitMode = false;
    public bool attackAllMode = false;
    public bool twoFieldsMode = false;

    private float timeCounter;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool moving = false;
    private bool isDead = false;

    private new Rigidbody rigidbody;
    private static int forceDirection = 1;

    private bool selected = false;
    public bool Selected { get => selected; set => selected = value; }
    public bool IsDead { get => isDead; set => isDead = value; }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        occupiedField.Occupied = true;
        startPosition = targetPosition = transform.position;

        if (!color.Equals("white") && !color.Equals("black"))
        {
            throw new System.ArgumentException("piece color must be black or white", "color");
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(moving)
        {
            timeCounter += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, timeCounter);

            if (transform.position.Equals(targetPosition)) moving = false;
        }
        if(occupiedField == null || occupiedField.Destroyed == true)
        {
            HandleOccupiedFieldDestruction();
        }
        
    }

    private void HandleOccupiedFieldDestruction()
    {
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        IsDead = true;
        Destroy(this.gameObject, 5f);
    }
    
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ComputerControllable")
        {
            StartCoroutine(Die(collision.transform.position, 2, 0));
        }
    }

    public virtual List<Field> GetAvailableFields()
    {
        List<Field> availableFields = new List<Field>();
        Field[] fields = Field.FindObjectsOfType<Field>();

        Vector3 startingPosition = occupiedField.transform.position;
        foreach (Field field in fields)
        {
            if (IsFieldAvailable(field, startingPosition) && !field.Occupied && !field.Destroyed && !field.Equals(occupiedField))
            {
                availableFields.Add(field);
            }
        }

        return availableFields;
    }

    public virtual Field GetComputerFieldToMove()
    {
        if(goToField != null)
        {
            if(IsFieldAvailable(goToField, occupiedField.transform.position))
            {
                return goToField;
            }
            return null;
        }

        if(goToField == null && twoFieldsMode)
        {
            return null;
        }

        List<Field> fields = GetAvailableFields();

        int index = Random.Range(0, fields.Count);

        if (fields.Count == 0) return null;
        return fields[index];
    }

    public virtual void MoveTo(Field field)
    {
        if(field != null && !field.Occupied && !field.Destroyed && !IsDead)
        {
            occupiedField.Occupied = false;
            field.Occupied = true;

            if (goToField != null)
            {
                goToField = occupiedField;
            }

            occupiedField = field;

            targetPosition = new Vector3(field.transform.position.x, transform.position.y, field.transform.position.z);
            timeCounter = 0f;
            startPosition = transform.position;
            moving = true;
        }
    }

    public virtual void Attack(Piece enemy, float forceMultiplier)
    {
        if(enemy != null && !IsDead)
        {
            StartCoroutine(enemy.Die(transform.position, forceMultiplier, moveTime-0.2f));
            MoveTo(enemy.occupiedField);
        }
    }

    public virtual IEnumerator Die(Vector3 attackerPosition, float forceMultiplier, float delay)
    {
        occupiedField.Occupied = false;
        yield return new WaitForSeconds(delay);

        rigidbody.AddTorque(new Vector3(0, 0, forceDirection*forceMultiplier), ForceMode.Force);
        Vector3 victimPosition = transform.position;
        Vector3 forceVector = new Vector3((victimPosition.x-attackerPosition.x), 1, (victimPosition.z - attackerPosition.z));
        rigidbody.AddForce(forceVector*forceMultiplier, ForceMode.Impulse);
        forceDirection *= -1;
        IsDead = true;
        Destroy(this.gameObject, 10f);
    }

    public abstract bool IsFieldAvailable(Field field, Vector3 startingPosition);

    public virtual Piece GetComputerPieceToAttack()
    {
        Piece[] pieces = Piece.FindObjectsOfType<Piece>();

        Vector3 startingPosition = occupiedField.transform.position;
        foreach (Piece piece in pieces)
        {
            if (!piece.Equals(this) && IsFieldAvailable(piece.occupiedField, startingPosition) && !piece.isDead)
            {
                if (attackAllMode && piece.tag == "ComputerControllable")
                {
                    return piece;
                }

                if (piece.tag == "PlayerControllable")
                {
                    return piece;
                }
            }
        }
        return null;
    }
}
