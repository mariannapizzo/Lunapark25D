using UnityEngine;

public class Conveyors : MonoBehaviour
{
    private Rigidbody rBody;
    public float speed;

    void Start()
    {
        // Ottieni il rigidbody dell'oggetto attuale
        rBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
     /* Vector3 pos = rBody.position;
        rBody.position += Vector3.back * speed *Time. fixedDeltaTime;
        rBody.MovePosition(pos);*/ 
        
        Vector3 pos = rBody.position;
        rBody.position -= transform.right * speed *Time. fixedDeltaTime;
        rBody.MovePosition(pos);
    }
}
