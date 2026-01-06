using UnityEngine;

/* Example script to apply trauma to the camera or any game object */
public class TraumaInducer : MonoBehaviour 
{
    [Tooltip("Maximum stress the effect can inflict upon objects Range([0,1])")]
    public float MaximumStress = 0.6f;
    [Tooltip("Maximum distance in which objects are affected by this TraumaInducer")]
    public float Range = 45;

    [SerializeField] private GameObject camara;

    private StressReceiver stressReceiverScript;

    void Start()
    {
        stressReceiverScript = camara.GetComponent<StressReceiver>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, camara.transform.position);
        float distance01 = Mathf.Clamp01(distance / Range);
        float stress = (1 - Mathf.Pow(distance01, 2)) * MaximumStress;
        stressReceiverScript.InduceStress(stress);
    }
}