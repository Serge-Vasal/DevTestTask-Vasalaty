using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace TPS
{
    public class TPS_CharacterController : MonoBehaviour
    {      
        private Animator anim;
        private Vector2 previousInput;
        
        public Transform cam;
        public Transform groundCheck;        
        public MouseLook mouseLook = new MouseLook();
        public bool swordActiveState=false;
        
        
        private float m_YRotation;
        private Vector3 m_GroundContactNormal;
        private bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded;

        public bool Grounded
        {
            get { return m_IsGrounded; }
        }

        public bool Jumping
        {
            get { return m_Jumping; }
        }

        void Start()
        {           
            mouseLook.Init(transform, cam.transform);
            anim = GetComponent<Animator>();
        }


        void Update()
        {
            RotateView();

            if (Input.GetMouseButtonDown(0))
            {
                int attackID = UnityEngine.Random.Range(0, 2);                
                anim.SetInteger("AttackID", attackID);
                anim.SetTrigger("Attack");
            }

            if (Input.GetMouseButtonDown(1))
            {
                if ((anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_L")) ||
                   (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_R")))
                {  
                    anim.SetTrigger("AttackStrong");                    
                }
            } 

            if (Input.GetButtonDown("Jump") && !m_Jump)
            {
                m_Jump = true;
            }
        }

        private void FixedUpdate()
        {            
            Vector2 input = GetInput();

            if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) )
            {
                anim.SetBool("isMoving", true);
                anim.SetFloat("xInput", input.x);
                anim.SetFloat("yInput", input.y);
                if ((input.x > 0.9f || input.x < -0.9f)||(input.y > 0.9f || input.y < -0.9f))
                {
                    anim.SetBool("isRunning", true);
                }
                else
                {
                    anim.SetBool("isRunning", false);
                }            
                previousInput = input; 
            }
            else
            {
                anim.SetBool("isMoving", false);
                anim.SetFloat("xInput", input.x);
                anim.SetFloat("yInput", input.y);
            } 
        }

        private Vector2 GetInput()
        {
            Vector2 input = new Vector2
            {
                x = Input.GetAxis("Horizontal"),
                y = Input.GetAxis("Vertical")
            }; 
            return input;
            }

        private void RotateView()
        {
            if (Mathf.Abs(Time.timeScale) < float.Epsilon)
                return;

            float oldYRotation = transform.eulerAngles.y;

            mouseLook.LookRotation(transform,cam);            
        }
        
        public void SwordOn()
        {
            GameManager.Instance.swordActive = true;            
        }

        public void SwordOff()
        {
            GameManager.Instance.swordActive = false;
        }


    }
}