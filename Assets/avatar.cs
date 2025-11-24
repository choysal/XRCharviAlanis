using UnityEngine;
using Meta.XR.MRUtilityKit;
using UnityEngine.AI;
using System.Collections;

public class AvatarFollowPlayer : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed = 1;

    void Start()
    {
        agent.speed = speed;
    }


    void Update()
    {
        Vector3 targetPosition = Camera.main.transform.position;

        agent.SetDestination(targetPosition);
        
        
        Animator animator = GetComponentInChildren<Animator>();

        float currSpeed = Vector3.Magnitude(agent.velocity);
        if(animator != null)
        {
            if(currSpeed < .1f)
            {
                animator.SetFloat("Speed", 0.0f);
            } else
            {
                animator.SetFloat("Speed", currSpeed);  
            }
            
        }
    }
}
