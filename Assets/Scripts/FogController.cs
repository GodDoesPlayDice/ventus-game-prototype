using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class FogController : MonoBehaviour
{
    public Material mat;
    public float playerEnterShrinkSmooth = 0.01f;
    public float defaultDistFromCenter = 30f;
    private Transform player;

    private bool playerEntered = false;
    private static readonly int DistFromCenter = Shader.PropertyToID("_Dist_From_Center");
    private static readonly int PlayerPosition = Shader.PropertyToID("_Player_Position");

    private void Awake()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    private void Start()
    {
        if (mat == null) return;
        mat.SetFloat(DistFromCenter, defaultDistFromCenter);
    }

    private void Update()
    {
        if (player == null || mat == null) return;
        mat.SetVector(PlayerPosition, player.position);
        if (playerEntered)
        {
            Shrink();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerEntered = true;
        }
    }

    private void Shrink()
    {
        if (mat == null) return;
        var currentSize = mat.GetFloat(DistFromCenter);
        if (Mathf.Abs(0 - currentSize) > 0.1f)
        {
            mat.SetFloat(DistFromCenter, Mathf.Lerp(currentSize, 0f, playerEnterShrinkSmooth));
        }
        else
        {
            Destroy(gameObject);
        }
    }
}