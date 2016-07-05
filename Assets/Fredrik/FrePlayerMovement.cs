using UnityEngine;
using System.Collections;
using XInputDotNetPure;

 
public enum ControllerType{ Gamepad,MouseKeyboard};

[RequireComponent (typeof (Rigidbody2D))]
public class FrePlayerMovement : MonoBehaviour {

	// Top speed == acceleration / friction
	public float acceleration = 100;
	public float friction = 10;
	public float rotateSpeed = 20;
	public ControllerType controller = ControllerType.Gamepad;
	public float gamepadDeadzone = 0.25f;
	Rigidbody2D rigbod;
	PlayerIndex gamepadIndex;
	GamePadState gamepadState;
	// Use this for initialization
	void Start () {
		rigbod = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Vector2 moveVec = Vector2.zero;
		Vector2 aimVec = Vector2.zero;
		switch(controller)
		{
		case ControllerType.Gamepad:
	
			gamepadIndex = GetGamepad();

			gamepadState = GamePad.GetState(gamepadIndex);
				
			moveVec.x = Input.GetAxisRaw("Horizontal");
			moveVec.y = Input.GetAxisRaw("Vertical");

			aimVec.x = gamepadState.ThumbSticks.Right.X;
			aimVec.y = gamepadState.ThumbSticks.Right.Y;
			break;
		}

		moveVec.Normalize();
		aimVec.Normalize();



		rigbod.velocity -= friction * rigbod.velocity *Time.deltaTime;
			
		if(moveVec.sqrMagnitude > gamepadDeadzone*gamepadDeadzone)
			rigbod.velocity += moveVec * acceleration *Time.deltaTime;

		if(aimVec.sqrMagnitude > gamepadDeadzone*gamepadDeadzone)
			transform.localRotation = Quaternion.RotateTowards(transform.localRotation,Quaternion.Euler(0,0,Mathf.Atan2(aimVec.y,aimVec.x)*Mathf.Rad2Deg ),
				Time.deltaTime *rotateSpeed*Quaternion.Angle(transform.localRotation,Quaternion.Euler(0,0,Mathf.Atan2(aimVec.y,aimVec.x)*Mathf.Rad2Deg )));

//		GamePad
	}

	PlayerIndex GetGamepad()
	{
		PlayerIndex testPlayerIndex = (PlayerIndex)1;
		for (int i = 0; i < 4; ++i)
		{
			testPlayerIndex = (PlayerIndex)i;
			GamePadState testState = GamePad.GetState(testPlayerIndex);
			if (testState.IsConnected)
			{
				return testPlayerIndex;

			}

		}
		return testPlayerIndex;
	}
}
