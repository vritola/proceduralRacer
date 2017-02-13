using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Drivetain {RWD, FWD, AWD} //allows variables for different drivetain types, rear-wheel-drive, front-wheel-drive, all-wheel-drive

public class CarControl : MonoBehaviour
{
	//car part references
	public WheelJoint2D frontwheel;
	public WheelJoint2D backwheel;
	private JointMotor2D motorFront;//these give a "never assigned to" -warning, but are still needed
	private JointMotor2D motorBack; //because wheeljoint2D requires the jointmotor2D to be reassigned after value changes
	private Rigidbody2D body;

	//speed options
	public float maxForwardSpeed; //positive number
	public float maxBackwardSpeed; //negative number
	public float forwardTorque;
	public float backwardTorque;

	//drivetain options
	public Drivetain drivetainOption;
	private bool tractionFront = true;
	private bool tractionBack = true;

	//car rotation options
	public bool canRotate = false;
	public float carRotationSpeed;


	void Start ()
	{
		//enable traction based on drivetain option
		if (drivetainOption == Drivetain.FWD)
		{
			tractionFront = true;
			tractionBack = false;
		}
		if (drivetainOption == Drivetain.RWD)
		{
			tractionFront = false;
			tractionBack = true;
		}
		else //drivetain == AWD
		{
			tractionFront = true;
			tractionBack = true;
		}

		body = GetComponent<Rigidbody2D> ();
	}
		
	void Update ()
	{
		if (Input.GetAxisRaw ("Vertical") > 0) //if W pressed
		{
			if (tractionFront)
			{
				motorFront.motorSpeed = maxForwardSpeed * -1;
				motorFront.maxMotorTorque = forwardTorque;
				frontwheel.motor = motorFront; //reassign motor to the wheel
			}

			if (tractionBack)
			{
				motorBack.motorSpeed = maxForwardSpeed * -1;
				motorBack.maxMotorTorque = forwardTorque;
				backwheel.motor = motorBack; //reassign motor to the wheel

			}

		}
		else if (Input.GetAxisRaw ("Vertical") < 0) //if S pressed
		{
			if (tractionFront)
			{
				motorFront.motorSpeed = maxBackwardSpeed * -1;
				motorFront.maxMotorTorque = backwardTorque;
				frontwheel.motor = motorFront; //reassign motor to the wheel
			}

			if (tractionBack)
			{
				motorBack.motorSpeed = maxBackwardSpeed * -1;
				motorBack.maxMotorTorque = backwardTorque;
				backwheel.motor = motorBack; //reassign motor to the wheel
			}
		} 
		else //no input
		{
			backwheel.useMotor = false;
			frontwheel.useMotor = false;
		}
			
		if (canRotate == true)
		{
			if (Input.GetAxisRaw ("Horizontal") != 0) //if A or D pressed
			{
				body.AddTorque (carRotationSpeed * Input.GetAxisRaw ("Horizontal") * -1); //add torque directly to the rigidbody
			}
		}
	}
}