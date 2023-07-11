using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gun : MonoBehaviour
{
    RaycastHit hit;
    public GameObject muzzle;
    LineRenderer beam;

    public float fireDelay;

    bool isFireReady;

    public ParticleSystem muzzleFlash;

    AudioSource audio;
    public AudioSource audio2;


    public GameObject bulletHole;

    public int ammo = 30;
    public int maxAmmo = 30;

    public TextMeshProUGUI text;

    public Transform originalPos;
    public Transform zoomPos;

    public GameObject Sight;
    public GameObject Scope;

    public float reboundValue;

    float reboundX;
    float reboundZ;

    public float maxRebound;
    // Start is called before the first frame update
    void Start()
    {
        beam = muzzle.GetComponentInChildren<LineRenderer>();
        isFireReady = true;
        audio = muzzle.GetComponent<AudioSource>();
    }
    

    // Update is called once per frame
    void Update()
    {
        if(transform.parent.name == "Main Camera")
        {
            if (Input.GetMouseButton(0) && isFireReady && ammo > 0)
                Shoot();

            if (Input.GetKeyDown(KeyCode.R) || ammo <= 0)
            {
                Reload();
            }
            Zoom();
        }
       
    }
    public void PlayerDetected(RaycastHit playerHit)
    {
        if (isFireReady && ammo > 0)
            EnemyShoot(playerHit);

        if (ammo <= 0)
        {
            Reload();
        }
    }

    void Zoom()
    {
        if(Input.GetMouseButton(1))
        {
            transform.position = zoomPos.position;
            Scope.SetActive(true);
            Sight.SetActive(false);
            transform.GetChild(5).gameObject.SetActive(false);

            //while(this.transform.localPosition != zoomPos.position)
            //{
            //    this.transform.localPosition = Vector3.Lerp(transform.position, zoomPos.position, 0.2f);
            //}
        }
       else
        {
            transform.position = originalPos.position;
            Scope.SetActive(false);
            Sight.SetActive(true);
            transform.GetChild(5).gameObject.SetActive(true);


            //while (this.transform.localPosition != originalPos.position)
            //{
            //    this.transform.localPosition = Vector3.Lerp(transform.position, originalPos.position, 0.2f);
            //}
        }
            
    }

    void Reload()
    {
        ammo = maxAmmo;
        audio2.Play();
        isFireReady = false;
        StopAllCoroutines();
        StartCoroutine(ReloadDelayOn());
    }

    void Shoot()
    {
        ammo -= 1;
        text.text = ammo.ToString();
        muzzleFlash.Play();
        audio.Play();
        beam.enabled = true;
        beam.SetPosition(0,muzzle.transform.position);

       
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100))
        {
            //Debug.DrawRay(muzzle.transform.position, muzzle.transform.forward * hit.distance, Color.red);
            Vector3 hitVecctor = new Vector3(hit.point.x + reboundX, hit.point.y , hit.point.z + reboundZ);

           beam.SetPosition(1, hitVecctor);
    
                Instantiate(bulletHole, hitVecctor, Quaternion.FromToRotation(Vector3.back,hit.normal));
           
        }
        else
        {
            beam.SetPosition(1, muzzle.transform.position + muzzle.transform.forward * 100);
        }

        //StopCoroutine("Rebound");
       // StartCoroutine("Rebound");
        isFireReady = false;
        StartCoroutine("FireDelayON");
    }

    void EnemyShoot(RaycastHit playerHit)
    {
        ammo -= 1;
        muzzleFlash.Play();
        audio.Play();
        beam.enabled = true;
        beam.SetPosition(0, muzzle.transform.position);

        Vector3 hitVecctor = new Vector3(playerHit.point.x + reboundX, playerHit.point.y, playerHit.point.z + reboundZ);

        beam.SetPosition(1, hitVecctor);

        playerHit.collider.GetComponent<Player>().GetHit(10);

        //StopCoroutine("Rebound");
        // StartCoroutine("Rebound");
        isFireReady = false;
        StartCoroutine("FireDelayON");
    }
    IEnumerator FireDelayON()
    {
        yield return new WaitForSeconds(fireDelay);
        isFireReady = true;
        beam.enabled = false;
    }

    IEnumerator ReloadDelayOn()
    {
        yield return new WaitForSeconds(1.5f);
        if(transform.parent.name == "Main Camera")
            text.text = maxAmmo.ToString();
        text.text = maxAmmo.ToString();
        isFireReady = true;

    }

    //IEnumerator Rebound()
    //{
    //    reboundX += Random.Range(-0.3f, 0.3f);
    //    while (Mathf.Abs(reboundX) > maxRebound)
    //    {
    //        reboundX += Random.Range(-0.3f, 0.3f);
    //    }

    //    reboundZ += Random.Range(-0.3f, 0.3f);
    //    while (Mathf.Abs(reboundZ) > maxRebound)
    //    {
    //        reboundZ += Random.Range(-0.3f, 0.3f);
    //    }

    //    yield return null;
    //}
}
