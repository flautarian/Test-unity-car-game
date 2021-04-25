using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip turnUpCar;
    public AudioClip crashCar;
    public float life;
    public float maxVelocity;
    public float statusCarVelocity;
    public float acceleration;
    public float velocity;
    public float grip;
    public float round;
    
    private bool startPowerUpActivated = false;
    private float minX = -8f;
    private float maxX = 8f;

    private Vector2 target;
    private bool endGameFlag = false;
    private bool startGameFlag = false;

    private Vector3 rigidBodyPosition, rigidBodyVelocity, rigidBodyAngularVelocity;

    GameObject motor;

    void Start()
    {
        motor = GameObject.FindGameObjectWithTag("motorCarreteras");
    }

    internal void executePowerUp(string powerUpButtonName)
    {
        switch (powerUpButtonName)
        {
            case "ThunderButtton":
                motor.GetComponent<MotorCarreteras>().detonateAllObstacles();
                break;
            case "StarButtton":
                StartPowerUpEffects();
                break;
            case "BulletButtton":
                break;
            default:
                break;
        }
    }

    private void StartPowerUpEffects()
    {
        StartCoroutine(applyStarPowerUpEffects());
    }

    internal void ContactedWithGrass()
    {
        this.velocity = this.statusCarVelocity / 2;
    }

    private IEnumerator applyStarPowerUpEffects()
    {
        // TODO: Activar sonido efecto power up
        // TODO: Activar efecto en coche para parecer super y tal 
        startPowerUpActivated = true;
        statusCarVelocity = maxVelocity + 10f;
        yield return new WaitForSeconds(5f);
        startPowerUpActivated = false;
        statusCarVelocity -= maxVelocity;
        // TODO: Desactivar sonido efecto power up
        // TODO: Desactivar efecto en coche para parecer super y tal 
    }


    // Update is called once per frame
    void Update()
    {
        controlVelocity();
        controlPlayer();
        moveWheels(velocity);
    }


    private void controlVelocity()
    {
        if (!endGameFlag){
            if (statusCarVelocity > velocity) velocity += acceleration;
        }
        else reduceVelocityToZero();
    }

    private void moveWheels(float velocity)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).Rotate(Vector3.right * Time.deltaTime * velocity * 50, Space.Self);
        }
    }

    public void endGame()
    {
        this.endGameFlag = true;
        motor.GetComponent<MotorCarreteras>().endGame();
    }

    public void startGame()
    {
        this.startGameFlag = true;
    }

    private void controlPlayer()
    {
        //acceleration
        if (Input.GetKey("up"))
        {
            if (!endGameFlag)
            {
                if (statusCarVelocity > velocity) velocity += acceleration;
            }
        }
        else reduceVelocityToZero();

        //grip
        if (Input.GetKey("left"))
        {
            GetComponent<Transform>().Rotate(Vector3.down * round * Time.deltaTime);
        }
        else if (Input.GetKey("right"))
        {
            GetComponent<Transform>().Rotate(Vector3.up * round * Time.deltaTime);
        }
        else
            GetComponent<Transform>().rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), 3.0f * Time.deltaTime);
       
        target = Input.GetAxis("Horizontal") * Vector3.forward * velocity * Time.deltaTime;

        //acceleration
        if (startGameFlag)
        {
            if (transform.position.x + target.x > minX && transform.position.x + target.x < maxX) GetComponent<Transform>().Translate(target);
            else
            {
                Vector3 eulerRotation = transform.rotation.eulerAngles;
                GetComponent<Transform>().rotation = Quaternion.Euler(-eulerRotation.x, eulerRotation.y, eulerRotation.z);
            }
           GetComponent<Transform>().Translate(Vector3.forward * velocity * Time.deltaTime);
        }      
    }

    private void reduceVelocityToZero()
    {
        if (velocity > 1) velocity -= Time.deltaTime * velocity;
        else velocity = 0;
    }

    private void enablePowerUpButton(string v)
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Buttons");
        foreach(GameObject button in buttons)
        {
            if (v.Equals(button.name)){
                button.GetComponent<PowerUpButton>().enable();
            }
        }
    }

    public void EndingStreet(GameObject street)
    {
        //Debug.Log("abandonando una calle!");
        motor.GetComponent<MotorCarreteras>().ciclarCalle(street);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(System.Object.Equals(collision.gameObject.tag, "PlayerInteractable"))
        {
            collision.gameObject.GetComponent<Coin>().takeCoin();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (System.Object.Equals(collision.gameObject.tag, "PlayerInteractable"))
        {
            if (collision.gameObject.GetComponent<Coin>() != null) collision.gameObject.GetComponent<Coin>().takeCoin();
            else life -= 1;
        }
    }
}
