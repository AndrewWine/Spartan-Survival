using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    Animator animator;
    public Collider2D swordCollider;
    public float damage = 5;
  
    Vector2 rightAttackOffset;
    public void IncreaseDamage(int amount)
    {
        damage += amount;
        Debug.Log("New damage: " + damage);
    }

    private void Start() {

        if (PlayerPrefs.HasKey("SwordAttack"))
        {
            damage = PlayerPrefs.GetFloat("SwordAttack", damage); // Nếu không có dữ liệu lưu, sử dụng giá trị tối đa làm mặc định
        }
        if(swordCollider == null) {
            Debug.LogWarning("No collider set for sword attack");
        }
        

        
            
    }
    void OnTriggerEnter2D(Collider2D col) {
        col.SendMessage("OnHit", damage, SendMessageOptions.DontRequireReceiver);
    }

    public void AttackRight() {
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft() {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    public void StopAttack() {
        swordCollider.enabled = false;
    }

    public void ResetAttack()
    {
        damage = 5;  
        PlayerPrefs.DeleteKey("SwordAttack"); 
    }

  
}
