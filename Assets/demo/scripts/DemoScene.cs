using UnityEngine;
using System.Collections;
using Prime31;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DemoScene : MonoBehaviour
{
    [Tooltip("Curr scene index")]
    public int sceneIndex = 1;
    public GameObject _camera;
	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;

    public const float srcHp = 200f;
    public float currHp = 200f;
    public GameObject uiHP;
    //private RectTransform rectTransform;

    private static DemoScene instance;

    public Joystick joyStick;
    private float primaryWidth = 0f;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;

	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;

    public Button replayBtn;
    public Button backBtn;
    public GameObject losePanel;

    public GameObject winPanel;
    public Button nextLevel;
    public Button backMainBtn;

    private float primaryScaleX;

    public GameObject quitPanel;
    public Button quitCancel;
    public Button sureQuit;


    public static DemoScene Instance;
    public void Decrease(int count)
    {
        uiHP.transform.localScale = new Vector3(uiHP.transform.localScale.x - primaryScaleX *  count / srcHp, uiHP.transform.localScale.y);
       // uiHP.transform.position = new Vector3(4.3f * (1 - count / srcHp), uiHP.transform.position.y);
        //Debug.Log("curr" + srcHp);
        //rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x - count * 1.0f/ srcHp * primaryWidth, rectTransform.sizeDelta.y);
        //rectTransform.position = new Vector2(rectTransform.position.x - count * 1.0f / (srcHp * 2 )* primaryWidth, rectTransform.position.y);
        currHp -= count;
    }

    public void Increase(int count)
    {
        if (!uiHP.gameObject.activeSelf)
            uiHP.gameObject.SetActive(true);
        count = (int)Mathf.Min(count, srcHp - currHp);

        uiHP.transform.localScale = new Vector3(uiHP.transform.localScale.x + primaryScaleX * count / srcHp, uiHP.transform.localScale.y);

        //uiHP.gameObject.re
        //rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x + count * 1.0f / srcHp * primaryWidth, rectTransform.sizeDelta.y);
        //rectTransform.position = new Vector2(rectTransform.position.x + count * 1.0f / (srcHp * 2) * primaryWidth, rectTransform.position.y);
        currHp += count;
    }

	void Awake()
	{
        primaryScaleX = uiHP.transform.localScale.x;
           Instance = this;
        //rectTransform = uiHP.GetComponent<RectTransform>();
        //primaryWidth = rectTransform.rect.xMax - rectTransform.rect.xMin;
       // Debug.Log("Primary Width" + primaryWidth);
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();

		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
        _controller.onTriggerEnterEvent += _controller_onTriggerEnterEvent;

        replayBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(sceneIndex);
        });

        backBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });

        nextLevel.onClick.AddListener(() =>
        {
            SceneManager.LoadScene((sceneIndex + 1)%4);
        });

        backMainBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });

        quitCancel.onClick.AddListener(() =>
        {
            quitPanel.SetActive(false);
        });

        sureQuit.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
	}

    private void _controller_onTriggerEnterEvent(Collider2D obj)
    {
        
    }


    #region Event Listeners

    void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void onTriggerEnterEvent( Collider2D col )
	{
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
        if (col.gameObject.name.EndsWith("Star"))
        {
            Destroy(col.gameObject);
            Increase(20);
        }else if (col.gameObject.name.EndsWith("Damage"))
        {
            losePanel.SetActive(true);
        }else if (col.gameObject.name.EndsWith("Dis"))
        {
            winPanel.SetActive(true);
        }
	}


	void onTriggerExitEvent( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion


	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
        if (Input.GetButtonDown("Cancel"))
        {
            quitPanel.SetActive(true);
        }
        if (currHp <= 0)
        {
            currHp = 0;
            uiHP.gameObject.SetActive(false);
        }
        if (gameObject.transform.position.x - _camera.transform.position.x <= -8)
            gameObject.transform.position = new Vector3(_camera.transform.position.x - 8, transform.position.y, transform.position.z);
		if( _controller.isGrounded )
			_velocity.y = 0;

		if( joyStick.Horizontal > 0.3f )
		{
			normalizedHorizontalSpeed = 1;
			if( transform.localScale.x < 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Run" ) );
		}
		else if( joyStick.Horizontal <= -0.3f )
		{
			normalizedHorizontalSpeed = -1;
			if( transform.localScale.x > 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Run" ) );
		}
		else
		{
			normalizedHorizontalSpeed = 0;

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Idle" ) );
		}


		// we can only jump whilst grounded
		if( _controller.isGrounded && joyStick.Vertical > 0.3f )
		{
			_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
			_animator.Play( Animator.StringToHash( "Jump" ) );
		}


		// apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );

		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;

		// if holding down bump up our movement amount and turn off one way platform detection for a frame.
		// this lets us jump down through one way platforms
		if( _controller.isGrounded && Input.GetKey( KeyCode.DownArrow ) )
		{
			_velocity.y *= 3f;
			_controller.ignoreOneWayPlatformsThisFrame = true;
		}

		_controller.move( _velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}

}
