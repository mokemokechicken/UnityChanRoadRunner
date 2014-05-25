// Require a character controller to be attached to the same game object
@script RequireComponent(CharacterController)



public var speed : float = 2; 
public var crawlSpeed : float = 5.0;
public var jumpSpeed : float = 8.0;
public var gravity : float = 20.0;

private var controller : CharacterController;


//Move Params
private var f_verticalSpeed : float = 0.0;
private var f_moveSpeed : float = 0.0;
private var v3_moveDirection : Vector3 = Vector3.zero;

//Boolean
private var crawl : boolean;
private var b_isBackward : boolean;
private var b_isJumping : boolean;
private var grounded : boolean = false;
private var jumping : boolean = false;
private var canjump : boolean;

//Rotate Params
private var q_currentRotation : Quaternion; //current rotation of the character
private var q_rot : Quaternion; //Rotate to left or right direction
private var f_rotateSpeed : float = 1.0; //Smooth speed of rotation

//Direction Params
private var v3_forward : Vector3; //Forward Direction of the character
private var v3_right : Vector3; //Right Direction of the character

private var c_collisionFlags : CollisionFlags; //Collision Flag return from Moving the character

//Create in air time
private var f_inAirTime : float = 0.0;
private var f_inAirStartTime : float = 0.0;
private var f_minAirTime : float = 0.15; // 0.15 sec.



//Using Awake to set up parameters before Initialize
public function Awake() : void {
	controller = GetComponent(CharacterController);
	crawl = false;
	b_isBackward = false;
	b_isJumping = false;
	f_moveSpeed = speed;
	c_collisionFlags = CollisionFlags.CollidedBelow;
	canjump = true;
	
}

public function Start() : void {
	f_inAirStartTime = Time.time;
	
	
}

public function Update() : void {
	//Get Main Camera Transfrom
	var cameraTransform = Camera.main.transform;
	animator = GetComponent(Animator);
	//Get forward direction of the character
	v3_forward = cameraTransform.TransformDirection(Vector3.forward);
	v3_forward.y = 0; //Make sure that vertical direction equal zero
	// Right vector relative to the character
	// Always orthogonal to the forward direction vector
	v3_right = new Vector3(v3_forward.z, 0, -v3_forward.x); // -90 degree to the left from the forward direction
	//Get Horizontal move - rotation
	var f_hor : float = Input.GetAxis("Horizontal");
	//Get Vertical move - move forward or backward
	var f_ver : float = Input.GetAxis("Vertical");
	//If we are moving backward
	if (f_ver < 0) {
		b_isBackward = true;
	} else { 
		b_isBackward = false; 
	}
	//Get target direction
	var v3_targetDirection : Vector3 = (f_hor * v3_right) + (f_ver * v3_forward);
	//If the target direction is not zero - mean there is no button pressing
	if (v3_targetDirection != Vector3.zero) {
		//Rotate toward the target direction
		v3_moveDirection = Vector3.Slerp(v3_moveDirection, v3_targetDirection, f_rotateSpeed * Time.deltaTime);
		v3_moveDirection = v3_moveDirection.normalized; //Get only direction by normalize our target vector
	} else {
		v3_moveDirection = Vector3.zero;
	}
	
	//Checking if character is on the ground	
	if (!b_isJumping) {
		//crawl
		if (Input.GetButton("Fire2")) {
			crawl = true;
			f_moveSpeed = crawlSpeed;
			animator.SetBool("gocrouch", true);
			canjump = false;
		} else {
			crawl = false;
			f_moveSpeed = speed;
			animator.SetBool("gocrouch", false);
			canjump = true;
		}  
        //Press Space to Jump
        if (canjump && Input.GetButton ("Jump")) 
        {
            f_verticalSpeed = jumpSpeed;
            b_isJumping = true;
            animator.SetBool("dojump", true); 
        }
        else
        {
        	animator.SetBool("dojump", false);
        }
	}
	// Apply gravity
	if (IsGrounded()) {
		f_verticalSpeed = 0.0; //if our character are grounded
		b_isJumping = false; //Checking if our character is in the air or not
		f_inAirTime = 0.0;
		f_inAirStartTime = Time.time;
		animator.SetBool("isgrounded", true);
	} else {
		f_verticalSpeed -= gravity * Time.deltaTime; //if our character in the air
		//Count Time
		f_inAirTime = Time.time - f_inAirStartTime;
		animator.SetBool("isgrounded", false);
	}

	// Calculate actual motion
	var v3_movement : Vector3 = (v3_moveDirection * f_moveSpeed) + Vector3 (0, f_verticalSpeed, 0); // Apply the vertical speed if character fall down
	v3_movement *= Time.deltaTime;
    
    // Move the controller
    c_collisionFlags = controller.Move(v3_movement);
    
   	
	//Update rotation of the character
    if (v3_moveDirection != Vector3.zero) 
    {
    	transform.rotation = Quaternion.LookRotation(v3_moveDirection);
    }
    animator.SetFloat("speed", f_hor*f_hor+f_ver*f_ver);
}
public function IsGrounded () : boolean {
	return (c_collisionFlags & CollisionFlags.CollidedBelow);
}
//Geting if the character is jumping or not
public function IsJumping() : boolean {
	return b_isJumping;
}
//Checking if the character is in the air more than the minimun time 
//This function is to make sure that we are falling not walking down slope
public function IsAir() : boolean {
	return (f_inAirTime > f_minAirTime);
}
//Geting if the character is moving backward
public function IsMoveBackward() : boolean {
	return b_isBackward;
}
