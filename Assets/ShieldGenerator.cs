using UnityEngine;

public class ShieldGenerator : MonoBehaviour
{
    [SerializeField] Shield shield;
    [SerializeField] EnemyStats parentStats;

    private void Update()
    {
        if ((parentStats.GetCurrentHealth() / parentStats.GetMaxHealth()) < 0.5f)
        {
            shield.KillShield();
        }
    }
}
