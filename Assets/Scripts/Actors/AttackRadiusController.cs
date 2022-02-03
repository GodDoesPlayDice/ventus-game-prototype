using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors
{
    public class AttackRadiusController : MonoBehaviour
    {
        public float attackRadius;
        public float lineThickness;
        public float growSmooth = 0.01f;
        public float shrinkSmooth = 0.02f;
        public bool isEnabled = false;
        
        public Material material;
        private static readonly int LineThickness = Shader.PropertyToID("_Line_Thickness");
        private static readonly int Radius = Shader.PropertyToID("_Radius");

        private void Start()
        {
            if (material != null)
            {
                material.SetFloat(LineThickness, lineThickness);
            }
            else
            {
                Debug.LogWarning("No material on attack radius object!", this);
            }
        }

        private void FixedUpdate()
        {
            if (material == null) return;
            float currentRadius = material.GetFloat(Radius);
            if (isEnabled)
            {
                if (Mathf.Abs(attackRadius - currentRadius) > 0.01f)
                {
                    material.SetFloat(Radius, Mathf.Lerp(currentRadius, attackRadius, growSmooth));
                }
            }
            else
            {
                if (currentRadius >= 0.01f)
                {
                    material.SetFloat(Radius, Mathf.Lerp(currentRadius, 0, shrinkSmooth));
                }
            }
        }

        public void ShowAttackRadius()
        {
            
        }
        
    }
}