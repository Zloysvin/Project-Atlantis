using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class SubmarineController : MonoBehaviour
{
    public InputSystem_Actions actions;

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
    [SerializeField] private float energy = 100;
    [SerializeField] private int hullIntegrity = 100;
    [SerializeField] private float energyPerSecBase = 0.05f; // need to be adjusted for balancing
    [SerializeField] private float engineConsumptionPerSec = 0.01f;
    public bool EngineOn { get; private set; }
    [Header("Diesel Stats")]
    public bool DieselEngineOn { get; private set; }

    [SerializeField] private float energyPerSecGeneration = 0.5f;
    [SerializeField] private float fuelConsumption = 10f;
    [SerializeField] private float currentFuel = 300;

    private InputAction move;
    private InputAction rotate;
    private Rigidbody2D rb;

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
        actions = new InputSystem_Actions();

        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
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

        if (rb.linearVelocityX > maxX)
        {
            rb.linearVelocityX = maxX;
        }
        else if(rb.linearVelocityX < maxNegativeX)
        {
            rb.linearVelocityX = maxNegativeX;
        }


        if (rb.linearVelocityY > maxY)
        {
            rb.linearVelocityY = maxY;
        }
        else if (rb.linearVelocityY < maxNegativeY)
        {
            rb.linearVelocityY = maxNegativeY;
        }
    }

    private void EngineHandler()
    {
        if (!DieselEngineOn)
        {
            SubmarineSoundsManager.Instance.EmitElectricEngine();
            energy -= (energyPerSecBase + engineConsumptionPerSec) * Time.deltaTime;
        }
        else
        {
            SubmarineSoundsManager.Instance.EmitDieselEngine();
            energy += energyPerSecGeneration * Time.deltaTime;
            currentFuel -= fuelConsumption * Time.deltaTime;
        }
    }

    public void ToggleEngine()
    {
        EngineOn = !EngineOn;
    }

    public void ToggleDiesel()
    {
        DieselEngineOn = !DieselEngineOn;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Finish")
        {
            Debug.Log(
                $"Distance{0.15f + (1.35f * (65f - Vector2.Distance(transform.position, other.transform.position)) / 17.5f)}");
            PingDisplayHandler.Instance.CrazynessFactor =
                0.15f + (1.35f * (65f - Vector2.Distance(transform.position, other.transform.position)) / 65f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PingDisplayHandler.Instance.CrazynessFactor = 0.15f;
    }

    private void OnCollisionEnter2D(Collision2D other) // TODO: Tweak numbers so collision would create decent noise if close to the monster
    {
        if(other.gameObject.tag != "Monster")
        {
            SubmarineSoundsManager.Instance.EmitCollision(rb.linearVelocity.magnitude / 2f * 120f);
            Debug.Log(rb.linearVelocity.magnitude / 2f * 120f);

            hullIntegrity -= Random.Range(5, 10);
        }
        else
        {
            hullIntegrity = 0;
        }

        if (hullIntegrity <= 0)
        {
            // Handle game over here
            Debug.Log("You are dead");
        }
    }
}
