using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RotatePlatform : LeverBehavior
{
    public GameObject platform;
    public GameObject[] toThrowOff;

    public GameObject enemy_ragdoll;

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void On_Behavior()
    {
        base.On_Behavior();

        StartCoroutine(Rotate());

        ThrowOffChildren();
    }

    IEnumerator Rotate()
    {
        float duration = 10;
        float current_secs = 0;

        while(platform.transform.eulerAngles.x < 90)
        {
            float newX = Mathf.Lerp(platform.transform.eulerAngles.x, 90, current_secs / duration);
            platform.transform.eulerAngles = new Vector3(newX, platform.transform.eulerAngles.y, platform.transform.eulerAngles.z);
            current_secs += Time.smoothDeltaTime;
            yield return new WaitForEndOfFrame();

        }
    }

    public void ThrowOffChildren()
    {
        foreach(GameObject obj in toThrowOff)
        {
            if(obj.GetComponent<Collider>() != null)
            {
                obj.GetComponent<Collider>().enabled = false;
                Debug.Log("Disabled collider on " + obj);
            }

            if(obj.GetComponentInChildren<DetectionCollider>() != null)
            {
                obj.GetComponentInChildren<DetectionCollider>().enabled = false;
                GameObject enemy = Instantiate<GameObject>(enemy_ragdoll, obj.transform.position, obj.transform.rotation);
                enemy.GetComponentInChildren<Rigidbody>().AddForce(enemy.transform.forward*100, ForceMode.Impulse);
                StartCoroutine(DestroyAfterSecs(5, enemy));
                obj.SetActive(false);
                AudioManager.Instance.PlaySFX(AudioManager.SFX.death_enemy);
                Debug.Log("Disabled enemy " + obj);
            }

            StartCoroutine(DestroyAfterSecs(3, obj));
        }
    }

    IEnumerator DestroyAfterSecs(float secs, GameObject obj)
    {
        yield return new WaitForSeconds(secs);
        GameObject.Destroy(obj);
    }

}
