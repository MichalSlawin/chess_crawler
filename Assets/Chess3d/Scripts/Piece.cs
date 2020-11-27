using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public string color; // black or white
    public float moveTime;
    public Field occupiedField;
    public float moveDistance = 2.1f;

    private float timeCounter;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool moving = false;
    private bool isDead = false;

    private static int forceDirection = 1;

    private bool selected = false;
    public bool Selected { get => selected; set => selected = value; }
    public bool IsDead { get => isDead; set => isDead = value; }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        occupiedField.Occupied = true;
        startPosition = targetPosition = transform.position;

        if (!color.Equals("white") && !color.Equals("black"))
        {
            throw new System.ArgumentException("color must be black or white", "color");
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
    }

    public abstract List<Field> GetAvailableFields();

    public abstract Field GetComputerFieldToMove();

    public virtual void MoveTo(Field field)
    {
        if(field != null && !field.Occupied && !IsDead)
        {
            occupiedField.Occupied = false;
            field.Occupied = true;
            occupiedField = field;

            targetPosition = new Vector3(field.transform.position.x, transform.position.y, field.transform.position.z);
            timeCounter = 0f;
            startPosition = transform.position;
            moving = true;
        }
    }

    public virtual void Attack(Piece enemy)
    {
        if(enemy != null && !IsDead)
        {
            enemy.Die();
            MoveTo(enemy.occupiedField);
        }
    }

    public virtual void Die()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        rigidbody.AddTorque(new Vector3(0, 0, forceDirection*20), ForceMode.Force);
        rigidbody.AddForce(new Vector3(0,10, forceDirection*20), ForceMode.Impulse);
        Destroy(this.gameObject, 5f);
        forceDirection *= -1;
        IsDead = true;
        occupiedField.Occupied = false;
    }

    public abstract Piece GetComputerPieceToAttack();
}
