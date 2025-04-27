using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class SubmarineController : MonoBehaviour
{
    public InputSystem_Actions actions;
    public bool IsInvincible = false;

    [Header("Speed Values")]
    [SerializeField] private float ascendSpeed = 5f;
    [SerializeField] private float descendSpeed = 10f;
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float backwardSpeed = 3f;

    [Header("Speed Limits")]
    [SerializeField] private float maxX = 2f;
    [SerializeField] private float maxNegativeX = 2f;
    [SerializeField] private float maxY = 2f;
    [SerializeField] private float maxNegativeY = 2f;

    [Header("Survival")] 
    [SerializeField] public float energy = 100;
    [SerializeField] private int hullIntegrity = 100;
    [SerializeField] private float energyPerSecBase = 0.05f; // need to be adjusted for balancing
    [SerializeField] private float engineConsumptionPerSec = 0.01f;
    public bool EngineIsOn { get; private set; } = true;
    [Header("Diesel Stats")]
    public bool DieselEngineOn { get; private set; }

    [SerializeField] private float energyPerSecGeneration = 0.5f;
    [SerializeField] private float fuelConsumption = 10f;
    [SerializeField] private float currentFuel = 300;

    private InputAction move;
    private InputAction rotate;
    private Rigidbody2D rb;


    // Didnt touched the stuff above.
    [Header("Input blocked related")]
    [SerializeField] float collisionFeedbackDuration = 0.3f;
    [SerializeField] float knockbackForce = 0.5f;
    [SerializeField] GameObject collisionModel;
    [SerializeField] Material collisionMaterial;
    [SerializeField] float materialUpdateInterval = 0.1f;
    [SerializeField] float hitStopDuration = 0.2f;
    WaitForSeconds matUpdateDuration;
    bool isBusyColliding;

    private void OnEnable()
    {
        move = actions.Player.Move;
        move.Enable();

        rotate = actions.Player.Rotate;
        rotate.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        rotate.Disable();
    }

    private void Awake()
    {
        matUpdateDuration = new WaitForSeconds(materialUpdateInterval);
        actions = new InputSystem_Actions();

        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SubmarineSoundsManager.Instance.StartEngine(DieselEngineOn);
    }

    private void FixedUpdate()
    {
        if (isBusyColliding) return;
        if (EngineIsOn)
            Move();
    }

    void Move()
    {
        Vector2 inputDirection = move.ReadValue<Vector2>();
        Vector2 moveDirection = new Vector2(0, 0);

        if (inputDirection.x > 0)
        {
            moveDirection.x = 1 * forwardSpeed;
        }
        else if (inputDirection.x < 0)
        {
            moveDirection.x = -1 * backwardSpeed;
        }

        if (inputDirection.y > 0)
        {
            moveDirection.y = 1 * ascendSpeed;
        }
        else if (inputDirection.y < 0)
        {
            moveDirection.y = -1 * descendSpeed;
        }

        rb.AddForce(transform.right * moveDirection.x, ForceMode2D.Force);
        rb.AddForce(Vector2.up * moveDirection.y, ForceMode2D.Force);

        rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, maxNegativeX, maxX);

        rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, maxNegativeY, maxY);
    }
    private void Update()
    {
        EngineHandler();
    }
    private void EngineHandler()
    {
        if (!DieselEngineOn)
        {
            energy -= (energyPerSecBase + engineConsumptionPerSec) * Time.deltaTime;
        }
        else
        {
            energy += (energyPerSecGeneration - energyPerSecBase) * Time.deltaTime;
            currentFuel -= fuelConsumption * Time.deltaTime;
        }
    }

    public void ToggleEngine()
    {
        EngineIsOn = !EngineIsOn;

        if (EngineIsOn)
        {
            SubmarineSoundsManager.Instance.StartEngine(DieselEngineOn);
        }
        else
        {
            SubmarineSoundsManager.Instance.StopEngines(DieselEngineOn);
        }
    }

    public void ToggleDiesel()
    {
        DieselEngineOn = !DieselEngineOn;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Finish")
        {
            float triggerSize = other.transform.localScale.x;
            PingDisplayHandler.Instance.CrazynessFactor =
                0.15f + (1.35f * (1f - (Vector2.Distance(transform.position, other.transform.position) / triggerSize))); // inperformant af...
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Finish")
        {
            PingDisplayHandler.Instance.CrazynessFactor = 0.15f;
        }
       
    }

    private void OnCollisionEnter2D(Collision2D other) // TODO: Tweak numbers so collision would create decent noise if close to the monster
    {
        if(!other.gameObject.CompareTag("Monster"))
        {
            SubmarineSoundsManager.Instance.EmitCollision(rb.linearVelocity.magnitude / 2f * 120f);

            hullIntegrity -= Random.Range(5, 10);
        }
        else
        {
            hullIntegrity = 0;
        }

        if (hullIntegrity <= 0)
        {
            // Handle game over here
            GameOverEffect.Instance.TriggerDeathEffect();
        }

        rb.AddForce(((Vector2)transform.position - other.contacts[0].point).normalized * knockbackForce, ForceMode2D.Impulse);
        CollisionFeedback();
    }

    #region CollisionFeedback

    private void CollisionFeedback()
    {
        if (isBusyColliding) return;

        StartCoroutine(HandleCollision());
    }
    private IEnumerator HandleCollision()
    {
        isBusyColliding = true;
        float timer = collisionFeedbackDuration;
        collisionModel.SetActive(true);

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(hitStopDuration);
        Time.timeScale = 1f;

        while (timer > 0)
        {
            collisionMaterial.SetInt("_PixelateSize", Random.Range(4, 50));
            timer -= materialUpdateInterval;
            yield return matUpdateDuration;
        }

        collisionModel.SetActive(false);
        isBusyColliding = false;
    }
    #endregion
}
