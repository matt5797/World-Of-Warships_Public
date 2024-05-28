using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WOW.Projectile;
using WOW.Data;
using WOW;

public class BallisticDemoFire : MonoBehaviour
{
    public GameObject bulletFactory;
    public List<ProjectileBase> projectileList;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i<20; i++)
        {
            CreateBullet(GetBullet(Random.Range(1, 600)));
        }

        /*foreach (Shell projectile in projectileList)
        {
            projectile.OnShoot(0);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateBullet(ShellData shellData)
    {
        GameObject bullet = Instantiate(bulletFactory);
        bullet.GetComponent<Shell>().shellData = shellData;
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        //projectileList.Add(bullet.GetComponent<Shell>());

        bullet.transform.Translate(Vector3.right * Random.Range(-10f, 10f));
    }

    ShellData GetBullet(int ShellID)
    {
        return BallisiticManager.Instance.GetShellData(ShellID);
    }
}
