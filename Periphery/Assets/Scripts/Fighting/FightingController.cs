using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightingController : MonoBehaviour
{
    /*
     * Hyper-Parameters
     *  1. DIM = scalar that multiplies the joystick position to get world position
     *  2. DIS = distance between fore anchor and back anchor
    */
    public float dim;
    public float dis;

    [SerializeField]
    private Joystick joystick;

    public int movementSpeed;
    public int rotationSpeed;

    private Rigidbody2D rb;

    public GameObject foreAnchor;
    public GameObject backAnchor;

    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {

    }

    private void Transform()
    {
        Vector3 forePos = foreAnchor.transform.position;
        Vector3 backPos = backAnchor.transform.position;

        float inputX = dim * joystick.Horizontal;
        float inputY = dim * joystick.Vertical;
        forePos = Vector3.MoveTowards(forePos, new Vector3(inputX, inputY), movementSpeed * Time.deltaTime);

        
    }

    
}
