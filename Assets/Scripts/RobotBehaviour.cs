using UnityEngine;
using System.Collections;

public class RobotBehaviour : Bolt.EntityBehaviour<IRoboState> {

    public Transform gunBarrel;
    public float bulletSpeed;
    public override void Attached()
    {
        state.Transform.SetTransforms(transform);
        state.SetAnimator(GetComponent<Animator>());

        state.Animator.applyRootMotion = entity.isOwner;

        if (entity.isOwner)
        {
            Camera.main.GetComponent<Follow_Robot_Test>().target = transform;
            //gunBarrel = transform.Find();
        }
    }

    public override void SimulateOwner()
    {
        //Get variables for simplicity use
        var speed = state.Speed;
        var angularSpeed = state.AngularSpeed;

        var aiming = state.Aiming;

        //Okay this is for aiming
        if(!aiming && Input.GetKeyDown(KeyCode.Mouse1)){
            state.Aiming = true;
        }
        else if(aiming && Input.GetKeyUp(KeyCode.Mouse1))
        {
            state.Aiming = false;
        }

        //This is for the "firing" animation
        if (aiming && Input.GetKeyDown(KeyCode.Mouse0))
        {
            state.Fire = true;

            // instantiate blast
            GameObject bolt = BoltNetwork.Instantiate(BoltPrefabs.Shot2, gunBarrel.transform.position, Quaternion.identity);
            bolt.GetComponent<Rigidbody>().AddForce(gunBarrel.forward * bulletSpeed, ForceMode.VelocityChange);
            state.Fire = false;
        }

        //Movement controlled by mecanim 
        if (Input.GetKey(KeyCode.W))
        {
            speed += 0.025f;
        }
        else
        {
            speed -= 0.025f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            angularSpeed -= 0.025f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            angularSpeed += 0.025f;
        }
        else
        {
            if (angularSpeed < 0)
            {
                angularSpeed += 0.025f;
                angularSpeed = Mathf.Clamp(angularSpeed, -1f, 0);
            }
            else if (angularSpeed > 0)
            {
                angularSpeed -= 0.025f;
                angularSpeed = Mathf.Clamp(angularSpeed, 0, +1f);
            }
        }


        //Clamp the states 
        state.Speed = Mathf.Clamp(speed, 0f, 1.5f);
        state.AngularSpeed = Mathf.Clamp(angularSpeed, -1f, +1f);
    }
}
