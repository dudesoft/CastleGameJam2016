using UnityEngine;
using System.Collections;

public class Gates : MonoBehaviour {
    public Transform Left;
    public Transform Right;

    public Vector3 ClosedTarget;

    private Vector3 openTargetLeft;
    private Vector3 openTargetRight;

    private BoxCollider2D leftCollider;
    private BoxCollider2D rightCollider;

    private Vector3 closedTargetLeft;
    private Vector3 closedTargetRight;

    private BoxCollider2D closeTrigger;

    private enum GateState {
        Open,
        Closed,
        Midway
    }
    private GateState state;

    public float Speed;

    // Debugging
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if(state == GateState.Open) {
                Close();
            } else if(state == GateState.Closed) {
                Open();
            }            
        }
    }

    void Start () {
        closeTrigger = GetComponent<BoxCollider2D>();
        leftCollider = Left.GetComponent<BoxCollider2D>();
        rightCollider = Right.GetComponent<BoxCollider2D>();
        // Save left and right side starting positions
        openTargetLeft = Left.position;
        openTargetRight = Right.position;
        // Calculate right and left close targets
        closedTargetLeft = transform.TransformPoint(ClosedTarget) - (leftCollider.bounds.size.x * Vector3.right / 2.0f);
        //closedTargetLeft += ClosedTarget;
        closedTargetRight = transform.TransformPoint(ClosedTarget) + (rightCollider.bounds.size.x * Vector3.right / 2.0f);
        //closedTargetRight += ClosedTarget;
        Debug.Log("Left " + closedTargetLeft + " Right " + closedTargetRight);
        state = GateState.Open;
       // Close();
    }    
	
	void OnTriggerEnter2D(Collider2D col) {
		if(col.gameObject.tag == "Player")
        	Close();
    }

    public void Close() {
        if(state == GateState.Open) {
            StartCoroutine(CloseAnim());
        }
    }
    private IEnumerator CloseAnim() {
        state = GateState.Midway;
        float leftDist = Vector3.Distance(Left.position, closedTargetLeft);
        float rightDist = Vector3.Distance(Right.position, closedTargetRight);
        bool leftDone = leftDist < Time.deltaTime * Speed;
        bool rightDone = rightDist < Time.deltaTime * Speed;

        Vector3 leftDir = closedTargetLeft - openTargetLeft;
        leftDir.Normalize();
        Vector3 rightDir = closedTargetRight - openTargetRight;
        rightDir.Normalize();
        while (!leftDone || !rightDone) {
            // Move towards targets
            if(!leftDone) {
                Left.position += (leftDir * Speed * Time.deltaTime);
            }
            if(!rightDone) {
                Right.position += (rightDir * Speed * Time.deltaTime);
            }           
            // Update remaining distances
            leftDist = Vector3.Distance(Left.position, closedTargetLeft);
            rightDist = Vector3.Distance(Right.position, closedTargetRight);
            leftDone = leftDist < Time.deltaTime * Speed;
            rightDone = rightDist < Time.deltaTime * Speed;
            yield return null;
        }
        // Ensure fully closed
        Left.position = closedTargetLeft;
        Right.position = closedTargetRight;
        state = GateState.Closed;
    }

    public void Open() {
        if(state == GateState.Closed) {
            StartCoroutine(OpenAnim());
        }
    }

    private IEnumerator OpenAnim() {
        state = GateState.Midway;
        float leftDist = Vector3.Distance(Left.position, openTargetLeft);
        float rightDist = Vector3.Distance(Right.position, openTargetRight);
        bool leftDone = leftDist < Time.deltaTime * Speed;
        bool rightDone = rightDist < Time.deltaTime * Speed;

        Vector3 leftDir = openTargetLeft - closedTargetLeft;
        leftDir.Normalize();
        Vector3 rightDir = openTargetRight - closedTargetRight;
        rightDir.Normalize();
        while (!leftDone || !rightDone) {
            // Move towards targets
            if (!leftDone) {
                Left.position += (leftDir * Speed * Time.deltaTime);
            }
            if (!rightDone) {
                Right.position += (rightDir * Speed * Time.deltaTime);
            }
            // Update remaining distances
            leftDist = Vector3.Distance(Left.position, openTargetLeft);
            rightDist = Vector3.Distance(Right.position, openTargetRight);
            leftDone = leftDist < Time.deltaTime * Speed;
            rightDone = rightDist < Time.deltaTime * Speed;
            yield return null;
        }
        // Ensure fully open
        Left.position = openTargetLeft;
        Right.position = openTargetRight;
        state = GateState.Open;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector3 pos = transform.TransformPoint(ClosedTarget);
        Gizmos.DrawWireSphere(pos, 0.2f);
    }
}
