using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(TimeBody), typeof(PlayerPlatformerController2D))]
public class RecordedAnimationState : MonoBehaviour
{
    List<int> hashNames  = new List<int>();
    List<float> normalizedTimes  = new List<float>();
    List<bool> flipStatuses  = new List<bool>();
    List<bool> groundedStatuses = new List<bool>();

    SpriteRenderer spriteRenderer;
    Animator animator;
    TimeBody timeBody;
    PlayerPlatformerController2D playerPlatformerController2D;
    
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        timeBody = GetComponent<TimeBody>();
        playerPlatformerController2D = GetComponent<PlayerPlatformerController2D>();
    }

    private void Update() {
        playerPlatformerController2D.CanMove = !timeBody.IsRewinding;
        playerPlatformerController2D.ComputingPhysics = !timeBody.IsRewinding;
    }

    private void FixedUpdate() {
       if (timeBody.IsRewinding) Rewind();
       else if (timeBody.IsRewinding == false) Record();
    }

    void Record()
    {
        // Records the hash name (an integer value) of the animation playing at the current frame
        if (hashNames.Count > Mathf.Round((1f / Time.fixedDeltaTime) * timeBody.MaxRecordTime))
        {
            hashNames.RemoveAt(hashNames.Count - 1); 
        }

        hashNames.Insert(0, animator.GetCurrentAnimatorStateInfo(0).fullPathHash);

        // Records the exact time frame (a noramlized value) of the animation playing at the current frame
        if (normalizedTimes.Count > Mathf.Round((1f / Time.fixedDeltaTime) * timeBody.MaxRecordTime))
        {
            normalizedTimes.RemoveAt(normalizedTimes.Count - 1); 
        }

        normalizedTimes.Insert(0, animator.GetCurrentAnimatorStateInfo(0).normalizedTime); 

        // Records the value of SpriteRendere.FlipX at the current frame
        if (flipStatuses.Count > Mathf.Round((1f / Time.fixedDeltaTime) * timeBody.MaxRecordTime))
        {
            flipStatuses.RemoveAt(flipStatuses.Count - 1); 
        }

        flipStatuses.Insert(0, spriteRenderer.flipX); 

        // Records the value of PhysicsObject2D.grounded at the current frame
        if (groundedStatuses.Count > Mathf.Round((1f / Time.fixedDeltaTime) * timeBody.MaxRecordTime))
        {
            groundedStatuses.RemoveAt(groundedStatuses.Count - 1); 
        }

        groundedStatuses.Insert(0, playerPlatformerController2D.Grounded);
    }

    public void Rewind()
    {
        if (hashNames.Count > 0 || normalizedTimes.Count > 0 || flipStatuses.Count > 0 || groundedStatuses.Count > 0)
        {
            animator.Play(hashNames[0], 0, normalizedTimes[0]);
            hashNames.RemoveAt(0);
            normalizedTimes.RemoveAt(0);

            spriteRenderer.flipX = flipStatuses[0];
            flipStatuses.RemoveAt(0);

            playerPlatformerController2D.Grounded = groundedStatuses[0];
            groundedStatuses.RemoveAt(0);
        }
    }

}
