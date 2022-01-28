using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _speedMultiplier = 2.5f;
    [SerializeField] private float _camPlayerSlerpFactor = 3f;

    [Header("Character Controller Vertical")]
    [SerializeField] private float _jumpSpeed = 10f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _minFall = -5f;
    [SerializeField] private float _terminalVelocity = -10f;
    [SerializeField] private float _checkOffset = 0.15f;
    private float _vertSpeed;

    [Header("text")]


    [SerializeField] private TMPro.TextMeshProUGUI _interactingText;
    [SerializeField] private TMPro.TextMeshProUGUI _notification;

    private PowerUpContainer nearDNA = null;

    //private Rigidbody _rigidbody;

    private CharacterController _characterController;
    private CapsuleCollider _capsuleCollider;


    private List<Animator> _animators = new List<Animator>();
    private bool _lastMoving = false;

    private Vector3 targetRotation;
    private Vector3 startRotation;
    private float _startTime;
    private bool _speedUp = false;

    public delegate void PowerUpEvent(PowerUp powerUp);
    public static PowerUpEvent acquiredPowerup;

    private bool _canMove = false;

    public int maxHealth = 100;
    public int currentHealth;

    // For tutorial purposes
    public bool isNearDNA = false;
    //////////////////////////////////

    [SerializeField] private HealthBar healthBar;

    [SerializeField] private GameOver gameOver;

    // Start is called before the first frame update
    void Start()
    {
        _vertSpeed = _minFall;
        _interactingText.enabled = false;

        _characterController = GetComponent<CharacterController>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        foreach (Animator a in GetComponentsInChildren<Animator>())
        {
            _animators.Add(a);
        }

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void enableMovement()
    {
        _canMove = true;
    }

    private void OnEnable()
    {
        TerrainGenerator.playerReady += enableMovement;
        TutorialEnvironment.tutorialReady += enableMovement;
    }
    private void OnDisable()
    {
        TerrainGenerator.playerReady -= enableMovement;
        TutorialEnvironment.tutorialReady -= enableMovement;
    }


    private bool checkJump()
    {

        bool _hitGround = false;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float check = (_capsuleCollider.height / 2) + _checkOffset;
            _hitGround = hit.distance <= check;
        }
        return _hitGround;
    }

    private void move(float horInput, float verInput)
    {
        Vector3 movement = new Vector3();
        float moveSpeed = _speedUp ? _speed * _speedMultiplier : _speed;
        if (horInput != 0 || verInput != 0)
        {
            movement.x = horInput * moveSpeed;
            movement.z = verInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);
        }

        if (checkJump())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _vertSpeed = _jumpSpeed;
            }
            else
            {
                _vertSpeed = _minFall;
            }
        }
        else
        {
            _vertSpeed += _gravity * 5 * Time.deltaTime;
            if (_vertSpeed < _terminalVelocity)
            {
                _vertSpeed = _terminalVelocity;
            }
        }
        movement = Quaternion.Euler(0, CameraManager.currentCamera.gameObject.transform.rotation.eulerAngles.y, 0) * movement;
        movement.y = _vertSpeed;
        movement *= Time.deltaTime;

        if (_canMove)
        {
            _characterController.Move(movement);
            //_characterController.Move(Quaternion.Euler(_rotation * _speedRotate * Time.deltaTime).eulerAngles);
        }


    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        bool moving = moveX != 0 || moveZ != 0;

        if (_lastMoving != moving)
        {
            _lastMoving = moving;

            foreach (Animator a in _animators)
            {
                a.SetBool("moving", moving);
            }
        }

        float camYRotation = CameraManager.currentCamera.gameObject.transform.rotation.eulerAngles.y;
        float playerYRotation = transform.rotation.eulerAngles.y;
        if (Mathf.Abs(camYRotation - playerYRotation) >= 5 && (targetRotation == null || Mathf.Abs(targetRotation.y - camYRotation) >= 5))
        {
            targetRotation = new Vector3(0, camYRotation, 0);
            startRotation = new Vector3(0, playerYRotation, 0);
            _startTime = Time.time;
        }

        float progress = (Time.time - _startTime) * _camPlayerSlerpFactor;
        if (progress <= 1)
        {
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRotation), Quaternion.Euler(targetRotation), progress);
        }

        _speedUp = Input.GetKey(KeyCode.LeftShift);

        if (nearDNA != null)
        {
            Vector3 vec1 = new Vector3(nearDNA.transform.position.x - transform.position.x, 0, nearDNA.transform.position.z - transform.position.z);
            Vector3 vec2 = new Vector3(transform.forward.x, 0, transform.forward.z);

            if (Mathf.Abs(Vector3.Angle(vec1, vec2)) < 50 || vec1.magnitude <= 1.5)
            {
                _interactingText.text = string.Format("Press E to Buy - {0}$", PowerUp.POWERUP_PRICE);
                _interactingText.enabled = true;
                if (Input.GetKey(KeyCode.E))
                {
                    if (ScoreManager.CanBuy(PowerUp.POWERUP_PRICE))
                    {
                        try
                        {
                            ScoreManager.removeTokens(PowerUp.POWERUP_PRICE);
                            nearDNA.OpenContainer();
                            nearDNA = null;
                            _interactingText.enabled = false;
                        }
                        catch (ScoreManager.ScoreException e)
                        {
                            Debug.LogWarning(e.Message);
                        }

                    }
                    else
                    {
                        StartCoroutine(flashInteractingTextRed());
                    }

                }
            }
            else
            {
                _interactingText.enabled = false;
            }
        }

        move(moveX, moveZ);
    }

    private void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;

            healthBar.SetHealth(currentHealth);
        }
        else
        {
            gameOver.EndGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PowerUpContainer>() != null)
        {
            nearDNA = other.gameObject.GetComponent<PowerUpContainer>();
            isNearDNA = true;
        }
        else if (other.gameObject.GetComponent<PowerUp>() != null)
        {
            PowerUp powerUp = other.gameObject.GetComponent<PowerUp>();
            acquiredPowerup(powerUp);
            StartCoroutine(powerUpAcquiredNotification(powerUp.PowerupType, powerUp.gameObject.GetComponentInChildren<Outline>().OutlineColor));
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Enemy")
        {
            TakeDamage(10);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PowerUpContainer>() != null)
        {
            nearDNA = null;
            _interactingText.enabled = false;
        }
    }


    private IEnumerator flashInteractingTextRed()
    {
        _interactingText.color = Color.red;
        yield return new WaitForSeconds(.1f);
        _interactingText.color = Color.white;
        yield return new WaitForSeconds(.1f);

        _interactingText.color = Color.red;
        yield return new WaitForSeconds(.1f);
        _interactingText.color = Color.white;

        yield return null;
    }

    private IEnumerator powerUpAcquiredNotification(PowerupTypes type, Color color)
    {
        float startingTime = Time.time;
        float endTime = startingTime + 0.6f;
        _notification.text = string.Format("Acquired <color=#{0}>{1}</color> !", ColorUtility.ToHtmlStringRGB(color), Enum.GetName(typeof(PowerupTypes), type).ToLower());


        Vector2 startingPos = new Vector2(0, 0);
        Vector2 endPos = new Vector2(0, 55);
        while (Time.time < endTime)
        {
            Vector2 pos = Vector2.Lerp(startingPos, endPos, (Time.time - startingTime) / (endTime - startingTime));
            _notification.rectTransform.anchoredPosition = pos;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(3f);
        startingTime = Time.time;
        endTime = startingTime + 0.6f;

        while (Time.time < endTime)
        {
            Vector2 pos = Vector2.Lerp(endPos, startingPos, (Time.time - startingTime) / (endTime - startingTime));
            _notification.rectTransform.anchoredPosition = pos;
            yield return new WaitForEndOfFrame();
        }


        yield return null;
    }
}





