using UnityEngine;

public class EnergyCap : Pickup
{
    ScoreManager scoreManager;
    [SerializeField] int scoreAmount = 100;
    [SerializeField] AudioClip pickupSound;

    public void Init(ScoreManager scoreManager)
    {
        this.scoreManager = scoreManager;
    }

    protected override void OnPickup()
    {
        scoreManager.IncreaseScore(scoreAmount);

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
    }
}
