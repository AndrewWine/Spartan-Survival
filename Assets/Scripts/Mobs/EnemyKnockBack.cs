using UnityEngine;

public class EnemyKnockBack : MonoBehaviour, IKnockBack
{
    public void KnockBack(Transform enemyTransform, Transform targetTransform, float offset)
    {
        Vector2 direction = targetTransform.position - enemyTransform.position;
        if (direction.x < 0)
        {
            enemyTransform.position = new Vector3(enemyTransform.position.x + offset, enemyTransform.position.y, enemyTransform.position.z);
            
        }
        else if (direction.x > 0)
        {
            enemyTransform.position = new Vector3(enemyTransform.position.x - offset, enemyTransform.position.y, enemyTransform.position.z);
        }
    }
}
