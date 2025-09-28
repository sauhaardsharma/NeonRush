using UnityEngine;

public class SpeedBoost : Pickup
{
    [SerializeField] float speedBoostAmount = 3f;
    LevelGenerator levelGenerator;
    [SerializeField] AudioClip pickupSound;

    public void Init(LevelGenerator levelGenerator)
    {
        this.levelGenerator = levelGenerator;
    }
    protected override void OnPickup()
    {
        levelGenerator.ChangeChunkMoveSpeed(speedBoostAmount);

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
    }
}