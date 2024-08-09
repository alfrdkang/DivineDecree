using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SageWisdom : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private void Start()
    {
        cam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        cam.m_Lens.FieldOfView *= 1.1f;
    }
}
