using UnityEngine;

public class HeadbobSystem : MonoBehaviour
{
    public static HeadbobSystem Instance { get; private set; }

    [SerializeField] private AudioClip[] audioFootsteps;
    [SerializeField] private AudioClip[] audioFootstepsTierra;
    [SerializeField] private AudioClip[] audioFootstepsMadera;

    private Transform player;
    private bool stepped = false;

    [SerializeField, Range(0f, 1f)] private static float Amount = 0.004f;

    [SerializeField, Range(0f, 40f)] private static float Frequency = 10.0f;

    private float Smooth = 80.0f;
    
    private Vector3 StartPos;
    private float sumY = 0f;

    private LayerMask mask;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        mask = LayerMask.GetMask("Tierra") | LayerMask.GetMask("Madera") | LayerMask.GetMask("Default");
    }

    void Start()
    {
        StartPos = transform.localPosition;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        CheckForHeadbobTrigger();
        CheckSound();
        StopHeadbob();
    }

    private void CheckSound()
    {
        if (!stepped && sumY < -0.005f)
        {
            Ray ray = new(player.position, -player.up);
            if (Physics.Raycast(ray, out RaycastHit hit, 2f, mask))
            {
                AudioClip audioToPlay;
                switch(hit.transform.gameObject.layer)
                {
                    // Para tierra
                    case 9:
                        audioToPlay = audioFootstepsTierra[Random.Range(0, audioFootstepsTierra.Length)];
                        break;
                    // Para madera
                    case 10:
                        audioToPlay = audioFootstepsMadera[Random.Range(0, audioFootstepsMadera.Length)];
                        break;
                    // Sonido pisar por defecto
                    default:
                        audioToPlay = audioFootsteps[Random.Range(0, audioFootsteps.Length)];
                        break;
                }
                AudioSource.PlayClipAtPoint(audioToPlay, player.position, 0.2f);
            }            
            stepped = true;
        }
        else if(stepped && sumY >= 0f)
        {
            stepped = false;
        }
    }

    private void CheckForHeadbobTrigger()
    {
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        if (inputMagnitude > 0)
        {
            StartHeadBob();
        }
    }

    private Vector3 StartHeadBob()
    {
        Vector3 pos = Vector3.zero;

        pos.y += Mathf.Lerp(pos.y, sumY = Mathf.Sin(Time.time * Frequency) * Amount * 1.4f, Smooth * Time.deltaTime);

        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * Frequency / 2f) * Amount * 1.6f, Smooth * Time.deltaTime);

        transform.localPosition += pos;

        return pos;
    }

    private void StopHeadbob()
    {
        if (transform.localPosition == StartPos) return;

        transform.localPosition = Vector3.Lerp(transform.localPosition, StartPos, 1 * Time.deltaTime);
    }

    public static void ChangeData(float amount, float frequency)
    {
        Amount = amount;
        Frequency = frequency;
    }
}
