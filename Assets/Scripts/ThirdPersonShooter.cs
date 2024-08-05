using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using System;
using UnityEngine.UI;

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
    [SerializeField] private Image chargedLoading;
    [SerializeField] private Image skillLoading;

    private bool _hasAnimator;
    private Animator _animator;
    private int _animIDSpeed;
    private int _animIDAiming;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;

    private bool canCharged = true;
    private bool canSkill = true;
    private float chargedCD = 2f;
    private float skillCD = 5f;

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDAiming = Animator.StringToHash("Aiming");
    }

    private void Start()
    {
        _hasAnimator = TryGetComponent(out _animator);
        AssignAnimationIDs();

        chargedLoading.fillAmount = 1;
        skillLoading.fillAmount = 1;

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
                if (canCharged)
                {
                    canCharged = false;
                    _animator.SetTrigger("ChargedShot");

                    bulletProjectile.damage = bulletProjectile.damage * 5;
                    StartCoroutine(ChargedCooldown(chargedLoading, chargedCD));
                    StartCoroutine(ShootArrow(0f, 1, mouseWorldPosition));
                }
            } else
            {
                _animator.SetTrigger("Shoot");
                StartCoroutine(ShootArrow(0.2f, 3, mouseWorldPosition));
            }
            starterAssetsInputs.shoot = false;
        }

        if (starterAssetsInputs.skill)
        {
            if (canSkill)
            {
                canSkill = false;
                _animator.SetTrigger("Skill");

                bulletProjectile.damage = bulletProjectile.damage * 10;
                StartCoroutine(SkillCooldown(skillLoading, skillCD));
                StartCoroutine(Skill());
            }
            starterAssetsInputs.skill = false;
        }
    }

    private IEnumerator ShootArrow(float timeBtwnShots, float numOfShots, Vector3 mouseWorldPosition)
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < numOfShots; i++)
        {
            yield return new WaitForSeconds(timeBtwnShots);
            Vector3 aimDirection = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(bullet, spawnBulletPosition.position, Quaternion.LookRotation(aimDirection, Vector3.up));
        }
        bulletProjectile.damage = bulletProjectile.baseDamage;
    }

    private IEnumerator Skill()
    {
        yield return new WaitForSeconds(1f);
        Vector3 aimDirection = (Vector3.down * Time.deltaTime).normalized;
        Instantiate(bullet, spawnBulletPosition.position, Quaternion.LookRotation(aimDirection, Vector3.up));
        bulletProjectile.damage = bulletProjectile.baseDamage;
    }

    private IEnumerator SkillCooldown(Image loadingBar, float cooldown)
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / cooldown;
            loadingBar.fillAmount = Mathf.Lerp(0, 1, t);

            yield return null;
        }
        canSkill = true;
    }

    private IEnumerator ChargedCooldown(Image loadingBar, float cooldown)
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / cooldown;
            loadingBar.fillAmount = Mathf.Lerp(0, 1, t);

            yield return null;
        }
        canCharged = true;
    }
}

