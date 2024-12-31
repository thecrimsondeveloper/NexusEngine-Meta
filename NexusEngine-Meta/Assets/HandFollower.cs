using Oculus.Interaction.Input;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class PalmFollower : MonoBehaviour
{
    // Reference to the hand
    [ShowInInspector,ShowIf("hand",true)]
    public Hand hand;

    [ShowInInspector,HideIf("hand",true)]
    Transform trackingTransformer = null;

    // Speed at which the object follows the palm
    public float followSpeed = 10f;
    public float distanceTolerance = 0.1f;
    public float angleTolerance = 0.1f;

    bool reachedDestination = false;

    public enum FollowMode 
    {
        Timed,
        Infinite
    }

    public FollowMode followMode = FollowMode.Infinite;

    [ShowIf("followMode", FollowMode.Timed)]
    public float followDuration = 1f;
    [ShowIf("followMode", FollowMode.Timed)]

    public UnityEvent OnFollowEnd = new UnityEvent();
    public UnityEvent OnReachHand = new UnityEvent();

    private void Update()
    {
        if(followMode == FollowMode.Timed && followDuration <= 0)
        {
            return;
        }

        if(hand == null)
        {
            return;
        }

        trackingTransformer = hand.TrackingToWorldTransformer.Transform;

        Vector3 targetPos = trackingTransformer.position;
        Quaternion targetRot = trackingTransformer.rotation;
      
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, followSpeed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, targetPos);
        float angle = Quaternion.Angle(transform.rotation, targetRot);

    
        bool atDestination = distance < distanceTolerance && angle < angleTolerance;
        if(atDestination&& !reachedDestination)
        {
            OnReachHand.Invoke();
            reachedDestination = true;
        }

        if (followDuration > 0 && followMode == FollowMode.Timed)
        {
            followDuration -= Time.deltaTime;
        }
        else if(followMode == FollowMode.Timed && followDuration <= 0)
        {
            OnFollowEnd.Invoke();
        }
    }


    public void SetHand(IHand ihand)
    {
        if(ihand is Hand handRef)
        {
            hand = handRef;
        }
        
    }
    public void SetFollowSpeed(float followSpeed)
    {
        this.followSpeed = followSpeed;
    }

    public void SetFollowDuration(float followDuration)
    {
        this.followDuration = followDuration;
    }
}