using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerController : MonoBehaviour {

    [Header("Movement")]

    public float horizontalSpeed;

    public int maxJumpCount;
    public float jumpInitialSpeed;
    public float jumpDeceleration;
    public float terminalVelocity;

    int currentJumpCount;
    float verticalSpeed;
    public bool leftGround;

    [Header("Speech")]

    public GameObject speechBubblePrefab;
    public float speechBubbleDuration;

    public float animalTriggerDistance;

    bool hasSpeechBubble;
    float speechBubbleProgress;
    SpeechBubble speechBubble;

    new Transform transform;
    new Rigidbody2D rigidbody;

    Transform spriteTransform;
    SpriteRenderer spriteRenderer;

    Transform heartTransform;
    SpriteRenderer heartRenderer;
    Material heartMaterial;

    [Header("Animation")]

    public Sprite[] walkCycle;
    public Vector3[] walkCycleHeartPosition;

    public Sprite[] jumpCycle;
    public Vector3[] jumpCycleHeartPosition;

    public float durationPerFrame;

    float frameProgress;
    int currentCycle;

	public AudioClip sfxClip;
	public AudioSource source;

    // Use this for initialization
    void Start () {
		
        if (transform == null) transform = GetComponent<Transform>();
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody2D>();

        if (spriteTransform == null) spriteTransform = transform.GetChild(0);
        if (spriteRenderer == null) spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();

        if (heartTransform == null) heartTransform = spriteTransform.GetChild(0);
        if (heartRenderer == null) heartRenderer = heartTransform.GetComponent<SpriteRenderer>();
        if (heartMaterial == null) heartMaterial = heartRenderer.material;

        // ---

        if (LevelTeleporter.hasTeleported) {
            transform.position = LevelTeleporter.nextDestination;
        }

        currentJumpCount = 1;
        hasSpeechBubble = false;
        currentCycle = 0;

        UpdateSpriteAnimationFrame(walkCycle,walkCycleHeartPosition);

        // ---

        float[] layerHeight = { 1f };
        heartMaterial.SetFloatArray("_LayerHeight",layerHeight);

        Vector4[] colors = { new Vector4(0,0,0,0) };
        heartMaterial.SetVectorArray("_Colors",colors);

		source = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {

        float dt = Time.deltaTime;

        Vector2 bottomLeft = transform.position + new Vector3(-transform.localScale.x/2,-transform.localScale.y / 2);
        Vector2 bottomRight = transform.position + new Vector3(transform.localScale.x / 2,-transform.localScale.y / 2);

        RaycastHit2D touchGroundLinecast = Physics2D.Linecast(bottomLeft,bottomRight);

        if (touchGroundLinecast) {

            if (leftGround) {

                currentJumpCount = 0;
                verticalSpeed = 0;

                UpdateSpriteAnimationFrame(walkCycle,walkCycleHeartPosition);

                currentCycle = 0;
                frameProgress = 0;
            }

            leftGround = false;

        } else {

            leftGround = true;
            if (currentJumpCount <= 0) {
                currentJumpCount = 1;
                currentCycle = 0;
            }

        }

        if (currentJumpCount < maxJumpCount && Input.GetKeyDown(KeyCode.Space)) {

            currentJumpCount++;
            verticalSpeed = jumpInitialSpeed;
            currentCycle = 0;
			SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_Jumping);
        }

        if (currentJumpCount > 0 && verticalSpeed > -terminalVelocity) {

            verticalSpeed -= jumpDeceleration * dt;
            if (verticalSpeed < -terminalVelocity) verticalSpeed = -terminalVelocity;

        }

        float hsp = Input.GetAxisRaw("Horizontal") * horizontalSpeed;

        if (currentJumpCount <= 0) 
		{

            if (Mathf.Abs(hsp) > 0) 
			{

                if (spriteRenderer.flipX != hsp < 0) 
				{
                    spriteRenderer.flipX = hsp < 0;
                    UpdateSpriteAnimationFrame(walkCycle,walkCycleHeartPosition);

                }

                frameProgress += dt;

                if (frameProgress >= durationPerFrame) 
				{
                    currentCycle = (currentCycle + 1) % walkCycle.Length; 
                    UpdateSpriteAnimationFrame(walkCycle,walkCycleHeartPosition);
                    frameProgress = 0;
                }



            } else {

                UpdateSpriteAnimationFrame(walkCycle,walkCycleHeartPosition);
                currentCycle = 0;
                frameProgress = 0;

            }

        } else {

            if (Mathf.Abs(hsp) > 0 && spriteRenderer.flipX != hsp < 0) {

                spriteRenderer.flipX = hsp < 0;
                UpdateSpriteAnimationFrame(jumpCycle,jumpCycleHeartPosition);

            }

            if (currentCycle < jumpCycle.Length - 1) {

                frameProgress += dt;

                if (frameProgress >= durationPerFrame) {
                    currentCycle++;
                    UpdateSpriteAnimationFrame(jumpCycle,jumpCycleHeartPosition);
                    frameProgress = 0;
                }

            }

        }

        rigidbody.velocity = new Vector2(hsp,verticalSpeed);

        if (hasSpeechBubble) {

            speechBubbleProgress += dt;

            if (speechBubbleProgress >= speechBubbleDuration || Input.GetKeyDown(KeyCode.E)) {

                hasSpeechBubble = false;
                speechBubble.StartPoolingAnimation();
                speechBubble = null;

            }

        } else if (Input.GetKeyDown(KeyCode.E)) {

            bool triggered = AnimalSpeech.TriggerAnimalSpeech(transform.position);

            if (!triggered) {

                speechBubble = SpeechBubble.GetFromPool(speechBubblePrefab);
                speechBubble.Initialize(transform,new Vector3(-0.1f,1.1f,0),new Vector2(1.3f,1f),transform.position.x < 0);
                speechBubble.SetText("<sprite=0>?");

                hasSpeechBubble = true;
                speechBubbleProgress = 0;

            }

        }

		PlayWalkingSound();

    }

    void UpdateSpriteAnimationFrame (Sprite[] _frames, Vector3[] _heartPosition) {

        spriteRenderer.sprite = _frames[currentCycle];
        heartTransform.localPosition = new Vector3(_heartPosition[currentCycle].x * (spriteRenderer.flipX ? -1 : 1),_heartPosition[currentCycle].y);

    }

	void PlayWalkingSound()
	{
		if(Input.GetKeyDown(KeyCode.D) && !leftGround)
		{
			SoundManagerScript.Instance.PlayLoopingSFX(AudioClipID.SFX_Walking);
		}
		else if(Input.GetKeyUp(KeyCode.D))
		{
			SoundManagerScript.Instance.StopLoopingSFX(AudioClipID.SFX_Walking);
		}

		if(Input.GetKeyDown(KeyCode.A) && !Input.GetKey(KeyCode.Space))
		{
			SoundManagerScript.Instance.PlayLoopingSFX(AudioClipID.SFX_Walking);
		}
		else if(Input.GetKeyUp(KeyCode.A))
		{
			SoundManagerScript.Instance.StopLoopingSFX(AudioClipID.SFX_Walking);
		}
		else if(leftGround)
		{
			SoundManagerScript.Instance.StopLoopingSFX(AudioClipID.SFX_Walking);
		}

	}

}