using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Joystick joystick;
    [SerializeField]
    private float reachRadius;
    [SerializeField]
    private GameObject Scythe;

    private Animator animator;
    private Transform target;
    private CharacterController controller;
    private float playerSpeed = 2.0f;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {

        Vector3 move = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        controller.Move(move * Time.deltaTime * playerSpeed);

        animator.SetFloat("f_speed", controller.velocity.magnitude);

        if (move != Vector3.zero)
        {
            StopAllCoroutines();
            target = null;
            animator.SetFloat("f_speed", 1);
            gameObject.transform.forward = move;
        }
        else if (target != null) animator.SetFloat("f_speed", 1);
        else animator.SetFloat("f_speed", 0);

        if (Input.touchCount > 0)
        {
            target = ClickOnGrass();
            if (target != null)
            {
                StopAllCoroutines();
                StartCoroutine(MoveToTarget());
            }
        } 

    }

    private Transform ClickOnGrass()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.collider.gameObject.GetComponent<Grass>() != null)
        {
            return hit.collider.gameObject.transform;
        }
        else return null;
    }

    private IEnumerator MoveToTarget()
    { 
        bool reached = false;
        while (!reached)
        {     
            Vector3 groundedTarget = new Vector3(target.position.x, 0, target.position.z);
            Vector3 direction = groundedTarget - transform.position;
            gameObject.transform.forward = direction;
            controller.Move(direction.normalized * Time.deltaTime * playerSpeed);

            if (Vector3.Distance(transform.position, groundedTarget) < reachRadius)
            {
                reached = true;
            }
            yield return null;
        }
        Scythe.SetActive(true);
        animator.SetTrigger("t_cut");
    }

    public void Slice()
    {
        target.GetComponent<Grass>().Cut();
        target = null;
    }

    public void DisableScythe()
    {
        Scythe.SetActive(false);
    }
}

