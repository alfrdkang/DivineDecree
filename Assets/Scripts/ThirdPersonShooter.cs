using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using System;
using UnityEngine.UIElements;

public class ThirdPersonShooter : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugRayTransform;
    [SerializeField] private Transform bullet;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private BulletProjectile bulletProjectile;


    private bool _hasAnimator;
    private Animator _animator;
    private float _animationBlend;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDAiming;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDAiming = Animator.StringToHash("Aiming");
    }

    private void Start()
    {
        _hasAnimator = TryGetComponent(out _animator);
        AssignAnimationIDs();

        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        _hasAnimator = TryGetComponent(out _animator);

        // Creating a Raycast where cursor is pointing
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugRayTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        if (starterAssetsInputs.aim)
        {
            _animator.SetBool(_animIDAiming, true);

            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);


            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        } else
        {
            _animator.SetBool(_animIDAiming, false);

            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
        }

        if (starterAssetsInputs.shoot)
        {
            if (starterAssetsInputs.aim)
            {
                _animator.SetTrigger("ChargedShot");

                starterAssetsInputs.shoot = false;
                bulletProjectile.damage = bulletProjectile.damage * 5;
                StartCoroutine(shootArrow(0f, 1, mouseWorldPosition));
            }
            else
            {
                _animator.SetTrigger("Shoot");
                starterAssetsInputs.shoot = false;
                StartCoroutine(shootArrow(0.2f, 3, mouseWorldPosition));
            }
        }
    }

    private IEnumerator shootArrow(float timeBtwnShots, float numOfShots, Vector3 mouseWorldPosition)
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < numOfShots; i++)
        {
            yield return new WaitForSeconds(timeBtwnShots);
            Vector3 aimDirection = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(bullet, spawnBulletPosition.position, Quaternion.LookRotation(aimDirection, Vector3.up));
        }
    }
}

