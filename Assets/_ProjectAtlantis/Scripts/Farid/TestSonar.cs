using System.Collections;
using System.Linq;
using UnityEngine;

public class TestSonar : MonoBehaviour
{

    public float rotationSpeed = 360.0f;
    public float autoDotDestroyTime = 1.2f;
    public GameObject DotPrefab;
    public Vector2 updateIntervalRange = new Vector2(0.1f,0.2f);
    public int updateSpacing = 1;
    public float detectionRange = 25f;
    float currentUpdateTimer;
    bool updateDots = true;
    float currentRotation;
    public Vector2 deepUpdateIntervalRange = new Vector2(0.1f,0.2f);
    public Vector2 deepUpdateDelayRange = new Vector2(0.1f,0.2f);
    public int deepUpdateSpacing = 3;
    float currentDeepTimer;
    bool updateDeep = true;

    public float moveSpeed = 5f;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!updateDots)
        {
            currentUpdateTimer -= Time.deltaTime;
            if(currentUpdateTimer <= 0 ) { updateDots = true; }
        }
        if (!updateDeep)
        {
            currentDeepTimer -= Time.deltaTime;
            if (currentDeepTimer <= 0) { updateDeep = true; }
        }

        currentRotation += Time.deltaTime * rotationSpeed;
        if (currentRotation >= 360f) currentRotation -= 360f;
        transform.rotation = Quaternion.AngleAxis(currentRotation, Vector3.forward);

        var hit = Physics2D.RaycastAll(transform.position, transform.up, detectionRange);
        if (hit != null && hit.Length >= 1 && updateDots)
        {
            int spacer = 0;
            for (int i = 0; i < hit.Length; i++)
            {
                if(spacer % updateSpacing != 0) { spacer++;continue; }
                spacer++;
                var dot = Instantiate(DotPrefab, hit[i].point, Quaternion.identity);
                //dot.transform.up = -hit.normal;
                dot.transform.up = (hit[i].point - (Vector2)transform.position).normalized;
                Destroy(dot, autoDotDestroyTime);
                updateDots = false;
                currentUpdateTimer = Random.Range(updateIntervalRange.x, updateIntervalRange.y);
            }
        }

        // Tmp input.
        transform.parent.position += new Vector3(Input.GetAxis("Horizontal") , Input.GetAxis("Vertical"),0f) *(moveSpeed * Time.deltaTime);
    }


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision != null)
    //    {
    //        foreach (var col in collision.contacts)
    //        {
    //            Destroy(Instantiate(DotPrefab, col.point, Quaternion.identity),autoDotDestroyTime);
    //        }
    //    }
    //}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (updateDeep)
        {
            if (collision != null)
            {
                StartCoroutine(DeepUpdate(collision.contacts));
                //int spacer = 0;
                //foreach (var col in collision.contacts)
                //{
                //    spacer++;
                //    if (spacer % deepUpdateSpacing != 0) { continue; }
                //    var dot = Instantiate(DotPrefab, col.point, Quaternion.identity);
                //    dot.transform.up = (col.point - (Vector2)transform.position).normalized;
                //    Destroy(dot, autoDotDestroyTime);
                //}
            }
            updateDeep = false;
            currentDeepTimer = Random.Range(deepUpdateIntervalRange.x, deepUpdateIntervalRange.y); ;
        }
    }

    IEnumerator DeepUpdate(ContactPoint2D[] contacts)
    {
        int spacer = 0;
        foreach (var col in contacts)
        {
            spacer++;
            if (spacer % deepUpdateSpacing != 0) { continue; }
            var dot = Instantiate(DotPrefab, col.point, Quaternion.identity);
            dot.transform.up = (col.point - (Vector2)transform.position).normalized;
            Destroy(dot, autoDotDestroyTime);
            yield return new WaitForSeconds(Random.Range(deepUpdateDelayRange.x, deepUpdateDelayRange.y));
        }
    }
}
