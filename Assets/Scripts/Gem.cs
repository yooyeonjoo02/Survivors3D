using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private int xpAmount = 1;
    [SerializeField] private float magnetRange = 3f;
    [SerializeField] private float magnetSpeed = 8f;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        // float dist = Vector3.Distance(transform.position, player.position);
        Vector3 diff = player.position - transform.position;

        if (diff.sqrMagnitude <= magnetRange * magnetRange)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                magnetSpeed 
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        XPManager.Instance.AddXP(xpAmount);
        Destroy(gameObject);
    }
}