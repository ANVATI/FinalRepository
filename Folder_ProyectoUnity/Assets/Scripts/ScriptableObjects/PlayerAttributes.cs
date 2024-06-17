using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttributes", menuName = "ScriptableObjects/PlayerAttributes")]
public class PlayerAttributes : ScriptableObject
{
    public float Life = 200f;
    public float walkSpeed = 5.0f;
    public float runSpeed = 9.8f;
    public float crouchSpeed = 3.0f;
    public float rollForce = 10f;
    public float Stamina = 100f;
    public float RageDuration = 10f;
    public float currentSpeed = 0f; 
    public float acceleration = 3.5f; 
    public float maxSpeed = 15f;
    public void ApplyRageAttributes()
    {
        walkSpeed *= 1.5f;
        runSpeed *= 1.2f;
        crouchSpeed *= 1.35f;
        rollForce *= 1.35f;
    }

    public void ResetAttributes()
    {
        walkSpeed /= 1.5f;
        crouchSpeed /= 1.35f;
        rollForce /= 1.35f;
    }

}
