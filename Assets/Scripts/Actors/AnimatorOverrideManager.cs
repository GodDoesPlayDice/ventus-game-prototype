using System;
using System.Collections.Generic;
using Enums;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Actors
{
    public class AnimatorOverrideManager : MonoBehaviour
    {
        public List<AnimatorOverrideController> characters;
        public Characters selectedCharacter;
        public Animator animator;
        

        private void Start()
        {
            animator.runtimeAnimatorController = characters[0];
        }

        private void Update()
        {
            if (animator.runtimeAnimatorController != characters[(int) selectedCharacter])
            {
                animator.runtimeAnimatorController = characters[(int) selectedCharacter];
            }
        }
    }
}