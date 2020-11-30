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
    
    public abstract List<Field> GetAvailableFields();

    public abstract Field GetComputerFieldToMove();

    public virtual void MoveTo(Field field)
    {
        if(field != null && !field.Occupied && !field.Destroyed && !IsDead)
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
        Destroy(this.gameObject, 10f);
        forceDirection *= -1;
        IsDead = true;
    }

    public abstract Piece GetComputerPieceToAttack();
}
