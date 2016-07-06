using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public enum ControlTypes{ Gamepad,MouseKeyboard };

[RequireComponent (typeof(Rigidbody2D))]
public class FrePlayerMovement : MonoBehaviour {


	public float acceleration;
	public float friction;
	public float rotateSpeed;

	public ControlTypes controller;
	public float deadZone = .25f;
	Rigidbody2D rigbod;

	bool playerIndexSet = false;
	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;

    public Vector3 lookDirection;
    public float lookDistance = 0;
    public float maxLookDistance = 10;

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
		case ControlTypes.Gamepad:
			if (!playerIndexSet || !prevState.IsConnected)
			{
				for (int i = 0; i < 4; ++i)
				{
					PlayerIndex testPlayerIndex = (PlayerIndex)i;
					GamePadState testState = GamePad.GetState(testPlayerIndex);
					if (testState.IsConnected)
					{
						Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
						playerIndex = testPlayerIndex;
						playerIndexSet = true;
					}
				}
			} 

			prevState = state;
			state = GamePad.GetState(playerIndex);

			if(deadZone < GetAnalogueMagnitude(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y))
			{
				moveVec.x = state.ThumbSticks.Left.X;
				moveVec.y = state.ThumbSticks.Left.Y;
			}

			if(deadZone < GetAnalogueMagnitude(state.ThumbSticks.Right.X,state.ThumbSticks.Right.Y))
			{
				
				aimVec.x = state.ThumbSticks.Right.X;
				aimVec.y = state.ThumbSticks.Right.Y;
			}
            lookDistance = GetAnalogueMagnitude(aimVec.x, aimVec.y)*maxLookDistance;
            

			break;

		case ControlTypes.MouseKeyboard:
			moveVec.x = Input.GetAxisRaw("Horizontal");
			moveVec.y = Input.GetAxisRaw("Vertical");

			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			aimVec.x = mousePos.x-transform.position.x ;
			aimVec.y = mousePos.y-transform.position.y ;
            lookDistance = Mathf.Clamp(Vector3.Distance(transform.position, aimVec), 0, maxLookDistance);
            lookDistance = ((lookDistance / maxLookDistance) * (lookDistance / maxLookDistance)) * maxLookDistance;
			break;
		}

		moveVec.Normalize();
		aimVec.Normalize();

		print(aimVec);

		rigbod.velocity -= friction * rigbod.velocity *Time.deltaTime;
		if(moveVec != Vector2.zero)
			rigbod.velocity += moveVec * acceleration *Time.deltaTime;

        if (aimVec != Vector2.zero)
        {
			transform.rotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0, 0, Mathf.Atan2(aimVec.y, aimVec.x) * Mathf.Rad2Deg),
				Time.deltaTime * rotateSpeed * Quaternion.Angle(transform.localRotation, Quaternion.Euler(0, 0, Mathf.Atan2(aimVec.y, aimVec.x) * Mathf.Rad2Deg)));

            lookDirection = aimVec;
            
        }
        else
            lookDistance = 0;
	}

    /// <summary>
    /// Gets the magnitude of an analogue stick.
    /// </summary>
    /// <returns>
    /// The analogue magnitude. (0-1) But depending on the controller might be more.
    /// </returns>
    /// <param name='xAxis'>
    /// Horizontal (X axis).
    /// </param>
    /// <param name='yAxis'>
    /// Vertical (Y axis).
    /// </param>
    public static float GetAnalogueMagnitude(float xAxis, float yAxis)
    {
        return Mathf.Sqrt(xAxis * xAxis + yAxis * yAxis);
    }

    /// <summary>
    /// Gets the magnitude of an analogue stick.
    /// </summary>
    /// <returns>
    /// The analogue magnitude. (0-1) But depending on the controller might be more.
    /// </returns>
    /// <param name='xAxis'>
    /// Horizontal (X axis).
    /// </param>
    /// <param name='yAxis'>
    /// Vertical (Y axis).
    /// </param>
    public static float GetAnalogueMagnitude(string xAxis, string yAxis)
    {
        float horizontal = UnityEngine.Input.GetAxis(xAxis);
        float vertical = UnityEngine.Input.GetAxis(yAxis);
        return GetAnalogueMagnitude(horizontal, vertical);
    }
}
