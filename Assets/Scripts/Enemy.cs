using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    RaycastHit hit;
    RaycastHit playerHit;

    Vector3 moveVec;

    GameManager manager;
    int interval;

    public float speed;

    Vector3 seeDistance;
    Vector3 cubeVec;
    Vector3 cubeSize;

    bool isPlayerDetected = false;
    // Start is called before the first frame update
    void Start()
    {
        
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        interval = manager.interval;

        RightHand();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        switch ((int)transform.rotation.eulerAngles.y)
        {
            case 0:
                cubeVec = new Vector3(transform.position.x, transform.position.y, transform.position.z + Mathf.Abs(seeDistance.z / 2));
                cubeSize = new Vector3(interval, interval, Mathf.Abs(seeDistance.z));
                break;
            case 90:
                cubeVec = new Vector3(transform.position.x + Mathf.Abs(seeDistance.x / 2), transform.position.y, transform.position.z);
                cubeSize = new Vector3(Mathf.Abs(seeDistance.x), interval, interval);
                break;
            case 180:
                cubeVec = new Vector3(transform.position.x, transform.position.y, transform.position.z - Mathf.Abs(seeDistance.z / 2));
                cubeSize = new Vector3(interval, interval, Mathf.Abs(seeDistance.z));
                break;
            case 270:
                cubeVec = new Vector3(transform.position.x - Mathf.Abs(seeDistance.x / 2), transform.position.y, transform.position.z);
                cubeSize = new Vector3(Mathf.Abs(seeDistance.x),interval, interval);
                break;
        }
        Gizmos.DrawWireCube(cubeVec, cubeSize);

        
    }
    // Update is called once per frame
    void Update()
    {
       
        if(transform.position == moveVec)
        RightHand();

        if(!isPlayerDetected)
        transform.position = Vector3.MoveTowards(transform.position, moveVec, speed * Time.deltaTime);

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000, LayerMask.GetMask("Wall")))
        {
            seeDistance = hit.point - transform.position;
        }
        if (Physics.BoxCast(transform.position, new Vector3(interval, interval, interval) / 2,
            transform.forward,
            out playerHit,
            Quaternion.identity,
            Vector3.Distance(hit.point, transform.position),
            LayerMask.GetMask("Player")))
        {
            Debug.Log("플레이어 감지");

            GetComponentInChildren<Gun>().PlayerDetected(playerHit);

            isPlayerDetected = true;

        }
        else
        {
            isPlayerDetected = false;
        }
    }

    void RightHand()
    {
        if(!Physics.Raycast(transform.position, transform.right, out hit, interval, LayerMask.GetMask("Wall")))
        {
            transform.Rotate(new Vector3(0, 90, 0));
            
        }
        else if(!Physics.Raycast(transform.position, transform.forward, out hit, interval, LayerMask.GetMask("Wall")))
        {
            
        }
        else if (!Physics.Raycast(transform.position, -transform.right, out hit, interval, LayerMask.GetMask("Wall")))
        {
            transform.Rotate(new Vector3(0, -90, 0));
        }
        else
        {
            transform.Rotate(new Vector3(0, 180, 0));
        }
        moveVec = transform.position + transform.forward * interval;
    }
}
