using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private int xpAmount = 1;
    [SerializeField] private float magnetRange = 3f;
    [SerializeField] private float magnetSpeed = 8f;

    private Transform player;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Gem: Player 태그를 가진 오브젝트를 찾지 못했습니다.");
        }
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 diff = player.position - transform.position;

        if (diff.sqrMagnitude <= magnetRange * magnetRange)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                magnetSpeed * Time.deltaTime
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