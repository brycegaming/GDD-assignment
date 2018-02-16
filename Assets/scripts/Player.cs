using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class Player : MonoBehaviour {
    private const float MAX_MOVEMENT_SPEED = 7.0f;
    private const float JUMP_VELOCITY = 9.0f;
    private const float ACCELERATION = 100;
    private Rigidbody2D body;
	AudioSource audio;

   private  bool isJumping = false;

   private  GameObject[] lamps;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        lamps = GameObject.FindGameObjectsWithTag("Lamp");
	    
	    //reset the level
	    for (int i = 0; i < lamps.Length; i++)
	    {
	        lamps[i].GetComponent<Lamp>().reset();
	    }
	    
	    GetComponent<LampMechanics>().reset();
	    
	    GameObject startPlatform = GameObject.FindGameObjectWithTag("StartPlatform");
	    transform.position = startPlatform.transform.position + new Vector3(0,startPlatform.transform.localScale.y/2+.1f,0);
		audio = GetComponent<AudioSource> ();
	}

    public void reset()
    {
        body.velocity = new Vector3(0,0,0);
        
        //reset the level
        for (int i = 0; i < lamps.Length; i++)
        {
            lamps[i].GetComponent<Lamp>().reset();
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyAI>().reset();
        }

        GetComponent<LampMechanics>().reset();

        GameObject startPlatform = GameObject.FindGameObjectWithTag("StartPlatform");
        transform.position = startPlatform.transform.position + new Vector3(0,startPlatform.transform.localScale.y/2+.1f,0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground"){
            reset();
        }

        if(collision.gameObject.tag == "Platform" || collision.gameObject.tag == "StartPlatform" || collision.gameObject.tag == "EndPlatform" )
            isJumping = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "StartPlatform" || collision.gameObject.tag == "EndPlatform") 
            isJumping = true;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.A))
        {
            if(!isJumping)
                body.velocity = new Vector3(-MAX_MOVEMENT_SPEED, body.velocity.y, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if(!isJumping)
                body.velocity = new Vector3(MAX_MOVEMENT_SPEED, body.velocity.y, 0);
        }
        else
        {
            if (!isJumping)
            {
                if (inRange(body.velocity.x, 1.0f, 1000000.0f))
                {
                    body.velocity = new Vector3((body.velocity.x) - Time.deltaTime * ACCELERATION, body.velocity.y, 0);
                }
                else if (inRange(body.velocity.x, -100000.0f, -1.0f))
                {
                    body.velocity = new Vector3((body.velocity.x) + Time.deltaTime * ACCELERATION, body.velocity.y, 0);
                }
                else
                {
                    body.velocity = new Vector3(0, body.velocity.y, 0);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!isJumping)
                body.velocity = new Vector3(body.velocity.x, JUMP_VELOCITY, 0);
			audio.Play ();
            isJumping = true;
        }
	}

    private bool inRange(float val, float low, float high)
    {
        return (val >= low) && (val <= high);
    }
    
    public void setJumping(bool jump)
    {
        this.isJumping = jump;
    }
}
