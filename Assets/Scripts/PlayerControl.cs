using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //Player Movement
    [FMODUnity.EventRef]
    public string walkSound;
    public float playerSpeed = 5f;
    public bool playerisMoving;

    //Location
    Rigidbody2D myBody;
    Vector3 direction;
    Vector3 prevLoc;


    // Start is called before the first frame update
    void Start()
    {
        playerisMoving = false;
        myBody = GetComponent<Rigidbody2D>();
        InvokeRepeating("PlayerSteps", 0, .2f);
    }

    // Update is called once per frame
    void Update()
    {
        //To adjust current location
        float locationX = Input.GetAxisRaw("Horizontal");
        float locationY = Input.GetAxisRaw("Vertical");


        //Arrow or WASD Input movement to transform the player
        if (locationX > 0.1f || locationX < -0.1f)
        {
            playerisMoving = true;
        }
        if (locationY > 0.1f || locationY < -0.1f)
        {
            playerisMoving = true;
        }
        //If the player isn't moving or running into an unmovable object, they aren't moving
        if(prevLoc == transform.position)
        {
            playerisMoving = false;
        }

        //Find direction to head in
        direction = new Vector3(locationX, locationY).normalized;
        
        //Move in that Direction and track previous location
        myBody.MovePosition(transform.position + direction * playerSpeed * Time.deltaTime);
        prevLoc = transform.position;
    }

    public void OnDisable()
    {
        playerSpeed = 0;
        playerisMoving = false;
    }

    public void OnEnable()
    {
        playerSpeed = 5f;
    }

    public void PlayerSteps()
    {
        if (playerisMoving == true)
        {
            FMODUnity.RuntimeManager.PlayOneShot(walkSound);
        }
    }
}
