using UnityEngine;
using TMPro;
using System.Security.Cryptography;

public class SubmarineManager : MonoBehaviour
{
    [SerializeField] float fuel = 100f;
    [SerializeField] float maxFuel = 100f;
    [SerializeField] float fuelUsageSpeed = 1f;
    [SerializeField] float mineFuelReduction = 5f;
    [SerializeField] float boxFuelGain = 10f;

    [SerializeField] Vector3 impulseForce = Vector3.up * 10;
    [SerializeField] Vector3 constantForce = Vector3.up * 20;
    [SerializeField] Vector3 forwardForce = Vector3.right * 20;

    [SerializeField] ForceMode forceMode = ForceMode.Force;

    [SerializeField] TextMeshProUGUI fuelText;
    [SerializeField] GameObject gameOverPanel;

    [SerializeField] private Color fullFuelColor = Color.green;
    [SerializeField] private Color lowFuelColor = Color.red;

    bool _thrust;
    Rigidbody rb;

    [SerializeField]
    float minRotation = 35;

    [SerializeField]
    float maxRotaion = -35;

    [SerializeField]
    float pitchSpeed = 1;

    [SerializeField]
    float speed = 1;

    [SerializeField]
    Transform ship;

    private bool resetted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        fuel -= Time.deltaTime * fuelUsageSpeed;

        UpdateFuelUI();

        if (fuel <= 0)
        {
            enabled = false;
        }
        if (Input.GetButtonDown("Jump"))
        {
            _thrust = true;
            resetted = false;
        }
        else if (Input.GetButton("Jump"))
        {
            Vector3 dest = new Vector3(maxRotaion, ship.transform.localRotation.eulerAngles.y,
            ship.transform.localRotation.eulerAngles.z);
            ship.transform.localRotation = Quaternion.Lerp(ship.transform.localRotation,
            Quaternion.Euler(dest), Time.deltaTime * pitchSpeed);
        }
        else if (Input.GetButtonUp("Jump"))
        {
            _thrust = false;
        }
        else
        {
            Vector3 dest = new Vector3(minRotation, ship.transform.localRotation.eulerAngles.y,ship.transform.localRotation.eulerAngles.z); 

            ship.transform.localRotation = Quaternion.Lerp(ship.transform.localRotation, Quaternion.Euler(dest), Time.deltaTime * pitchSpeed);
        }
    }
    private void FixedUpdate()
    {
        if (_thrust)
        {
            if (forceMode == ForceMode.Impulse)
            {
                _thrust = false;
                resetted = true;
                rb.AddForce(impulseForce, forceMode);
                ship.transform.localEulerAngles = new Vector3(maxRotaion,
                ship.transform.localRotation.eulerAngles.y, ship.transform.localRotation.eulerAngles.z);
            }
            else
            {
                rb.AddForce(constantForce, forceMode);
            }
        }
        rb.AddForce(forwardForce, forceMode);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger by:{other.gameObject}", other.gameObject);
        if (other.gameObject.CompareTag("Box"))
        {
            Destroy(other.gameObject);
            fuel = Mathf.Clamp(fuel + boxFuelGain, 0, maxFuel);
            Debug.Log($"Fuel gained: {fuel}");
        }
        else if (other.gameObject.CompareTag("Mine"))
        {
            Destroy(other.gameObject);
            fuel = Mathf.Clamp(fuel - mineFuelReduction, 0, maxFuel);
            Debug.Log($"Fuel lost: {fuel}");

            UpdateFuelUI();

            if (fuel <= 0)
            {
                GameOver();
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        rb.isKinematic = true;
        enabled = false;
        GameOver();
    }

    void UpdateFuelUI()
    {
        if (fuelText != null)
        {
            fuelText.text = "Fuel: " + fuel.ToString("F0");

            float fuelPercentage = fuel / maxFuel;

            fuelText.color = Color.Lerp(lowFuelColor, fullFuelColor, fuelPercentage);

        }
    }

    void GameOver()
    {
        fuelText.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
        enabled = false;
    }
}