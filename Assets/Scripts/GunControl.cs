using UnityEngine;
using System.Collections;

public class GunControl: MonoBehaviour
{
    [Header("Referances")]
    public PlayerMovement pc;
    public GameObject weaponHolder;
    public ParticleSystem muzzleFlash;
    public GameObject hitEffect;
    public GameObject scop;

    [Header("Tunables")]
    public float fireRate = 1f;
    public float range = 100f;
    public float damagePerHit = 5f;
    public float impactForce = 100f;
    public float relodeTime = 1f;
    public int maxAmo = 10;
    public float scopInWaitTime = 0.5f;
    public float scopStep = 5f;
    public float initialScop = 14f;
    public float maxScop = 5f;
    public float minScop = 15f;
    

    private Animator weaponAnimator;
    private float nextTimeToFire = 0f;
    private int currentAmo;
    private bool isReloding = false;
    private int perspectiveBeforeScoping;
    private float gunZoomIn;

    private void Start()
    {
        gunZoomIn = initialScop;
        weaponAnimator = weaponHolder.GetComponent<Animator>();
        currentAmo = maxAmo;
        scop.SetActive(false);
    }

    private void OnEnable()
    {
        isReloding = false;
    }

    void Update()
    {
        if (isReloding) { return; }
        if (currentAmo <= 0) { StartCoroutine(Relode()); }
        if (Input.GetButtonDown("Fire2")) { StartCoroutine(ScopIn()); }
        if (Input.GetButtonUp("Fire2")) { ScopOut(); }

        if (pc.GetPerspective() == pc.ScopedInPerspective)
        {
            gunZoomIn -= Input.GetAxis("Mouse ScrollWheel") * scopStep;
            gunZoomIn = Mathf.Clamp(gunZoomIn, maxScop, minScop);
            pc.scopedInCam.fieldOfView = gunZoomIn;
        }

        bool perspectiCheck = (pc.GetPerspective() == pc.FirstPersonPerspective) || (pc.GetPerspective() == pc.ScopedInPerspective);
        bool timeCheck = Time.time >= nextTimeToFire;
        bool inputCheck = Input.GetButton("Fire1");
        if (perspectiCheck && timeCheck && inputCheck)
        {
            Shoot();
            nextTimeToFire = Time.time + 1/fireRate;
        }
    }

    IEnumerator Relode()
    {
        Debug.Log("Reloding...");
        isReloding = true;
        weaponAnimator.Play("Relode");
        yield return new WaitForSeconds(relodeTime);
        isReloding = false;
        currentAmo = maxAmo;
        Debug.Log("Done Reloding!");
    }

    private void Shoot()
    {
        currentAmo--;
        muzzleFlash.Play();
        RaycastHit hit;
        weaponAnimator.Play("Fire Impact");
        if (Physics.Raycast(pc.firstPersonCam.transform.position, pc.firstPersonCam.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null) { target.TakeDamage(damagePerHit); }
            if (hit.rigidbody != null) { hit.rigidbody.AddForce(-hit.normal * impactForce); }
            Destroy(Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal)), 0.2f);
        }
    }

    IEnumerator ScopIn()
    {
        yield return new WaitForSeconds(scopInWaitTime);
        scop.SetActive(true);
        perspectiveBeforeScoping = pc.GetPerspective();
        pc.changePerspecive(pc.ScopedInPerspective);
    }

    void ScopOut()
    {
        scop.SetActive(false);
        pc.changePerspecive(perspectiveBeforeScoping);
        perspectiveBeforeScoping = -1;
    }
}
