using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using System;
using UnityEngine.UI;

public class ThirdPersonShooter : MonoBehaviour
{
    public static ThirdPersonShooter instance = null;

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
    [SerializeField] private GameObject energyExplosion;
    [SerializeField] private GameObject glowPurple;
    [SerializeField] private GameObject skillCircle;
    [SerializeField] private GameObject meteor;
    [SerializeField] private GameObject bow;

    private bool _hasAnimator;
    private Animator _animator;
    private int _animIDSpeed;
    private int _animIDAiming;

    private ThirdPersonController thirdPersonController;

    private bool canCharged = true;
    private bool canShoot = true;
    private bool canSkill = true;
    private bool playingChargeVFX = false;
    private bool playingSkillVFX = false;

    public float chargedCD = 2f;
    public float skillCD = 5f;
    public float shootCD = 1f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip arrowStormClip;
    [SerializeField] private AudioClip arrowCharging;
    [SerializeField] private AudioClip arrowShot;

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDAiming = Animator.StringToHash("Aiming");
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        _hasAnimator = TryGetComponent(out _animator);
        AssignAnimationIDs();

        chargedLoading.fillAmount = 1;
        skillLoading.fillAmount = 1;

        thirdPersonController = GetComponent<ThirdPersonController>();
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

        if (StarterAssetsInputs.instance.aim)
        {
            if (!canCharged)
            {
                StarterAssetsInputs.instance.aim = false;
                return;
            }

            StarterAssetsInputs.instance.canMove = false;

            if (!playingChargeVFX)
            {
                audioSource.PlayOneShot(arrowCharging, 0.8f);
                StartCoroutine(ChargingVFX(bow.transform, transform));
            }
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
            StarterAssetsInputs.instance.canMove = true;
            _animator.SetBool(_animIDAiming, false);

            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
        }

        if (StarterAssetsInputs.instance.shoot)
        {
            if (StarterAssetsInputs.instance.aim)
            {
                if (canCharged)
                {
                    StarterAssetsInputs.instance.aim = false;
                    canCharged = false;
                    _animator.SetTrigger("ChargedShot");

                    StartCoroutine(ChargedCooldown(chargedLoading, chargedCD));
                    StartCoroutine(ShootArrow(0f, 1, mouseWorldPosition, true));
                }
            } else
            {
                if (canShoot)
                {
                    canShoot = false;
                    _animator.SetTrigger("Shoot");
                    StartCoroutine(ShootCooldown(shootCD));
                    StartCoroutine(ShootArrow(0.2f, 3, mouseWorldPosition, false));
                }
            }
            StarterAssetsInputs.instance.shoot = false;
        }

        if (StarterAssetsInputs.instance.skill)
        {
            if (!canSkill)
            {
                StarterAssetsInputs.instance.skill = false;
                return;
            }

            StarterAssetsInputs.instance.canMove = false;

            if (canSkill)
            {
                if (!playingSkillVFX)
                {
                    StartCoroutine(SkillVFX(bow.transform, transform));
                }
                canSkill = false;
                _animator.SetTrigger("Skill");

                StartCoroutine(SkillCooldown(skillLoading, skillCD));
                //StartCoroutine(Skill());
            }
            StarterAssetsInputs.instance.skill = false;
        }
    }

    private IEnumerator ChargingVFX(Transform arrowLocation, Transform player)
    {
        playingChargeVFX = true;
        yield return new WaitForSeconds(0.5f);
        //GameObject energyVFXObj = GameObject.Instantiate(energyExplosion, player.position, player.rotation, player) as GameObject;
        GameObject purpleVFXObj = GameObject.Instantiate(glowPurple, arrowLocation.position, arrowLocation.rotation, arrowLocation) as GameObject;
        while (StarterAssetsInputs.instance.aim)
        {
            yield return null;
        }
        //Destroy(energyVFXObj);
        Destroy(purpleVFXObj);
        playingChargeVFX = false;
    }

    private IEnumerator SkillVFX(Transform arrowLocation, Transform player)
    {
        playingSkillVFX = true;
        audioSource.clip = arrowStormClip;
        audioSource.Play();
        yield return new WaitForSeconds(0.8f);
        GameObject skillCircleVFXObj = GameObject.Instantiate(skillCircle, player.position, player.rotation) as GameObject;
        GameObject meteorVFXObj = GameObject.Instantiate(meteor, player.position, player.rotation) as GameObject;
        yield return new WaitForSeconds(3f);
        Destroy(skillCircleVFXObj);
        Destroy(meteorVFXObj);
        audioSource.Stop();
        playingSkillVFX = false;
        StarterAssetsInputs.instance.canMove = true;
    }

    private IEnumerator ShootArrow(float timeBtwnShots, float numOfShots, Vector3 mouseWorldPosition, bool isCharged)
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < numOfShots; i++)
        {
            audioSource.PlayOneShot(arrowShot, 0.8f);
            bulletProjectile.damage = GameManager.instance.playerBaseDamage;

            yield return new WaitForSeconds(timeBtwnShots);
            Vector3 aimDirection = (mouseWorldPosition - spawnBulletPosition.position).normalized;

            Transform obj = Instantiate(bullet, spawnBulletPosition.position, Quaternion.LookRotation(aimDirection, Vector3.up));

            if (isCharged)
            {
                Instantiate(energyExplosion, obj.position, obj.rotation, obj);
                obj.GetComponent<BulletProjectile>().damage = GameManager.instance.playerBaseDamage * 5;
            }
        }
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

    private IEnumerator ShootCooldown(float cooldown)
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / cooldown;

            yield return null;
        }
        canShoot = true;
    }
}

