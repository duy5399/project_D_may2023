using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraController : BossController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        summonPoint = GameObject.Find("MobSpawnPoint").transform;
        PandoraAttack.instance.LoadBulletToAttack(prefabBullet);
        PandoraSummon.instance.LoadMobToSummon(prefabMob, numberOfMob);
    }

    protected override void FixedUpdate()
    {
        base .FixedUpdate();
    }

    protected override IEnumerator BossMechanics(float _interval)
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(_interval);
            StartCoroutine(SummonMobs());
            yield return new WaitForSeconds(_interval);
            StartCoroutine(Attack02(5, attackPoint, target));
            yield return new WaitForSeconds(_interval);
            StartCoroutine(Attack01());
            yield return new WaitForSeconds(_interval);
            StartCoroutine(Attack00(5, attackPoint, target));
            yield return new WaitForSeconds(_interval);
            StartCoroutine(Attack02(5, attackPoint, target));
            yield return new WaitForSeconds(_interval);
            turnAction++;
            if (turnAction < 5)
            {
                if (turnAction == 2)
                {
                    StartCoroutine(Movement());
                }
                //StartCoroutine(PandoraMechanics(intervalNextAction));
            }
            Debug.Log("turn: " + turnAction);
        }
        yield return null;
    }

    protected override IEnumerator SummonMobs()
    {
        UIController.instance.OnAlertWarning("Pandora chuẩn bị triệu hồi các chiến binh kiến, hãy tiêu diệt chúng!!!");
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("beatA");
        StartCoroutine(PandoraSummon.instance.SummonMobs(numberOfMob, 0.75f, summonPoint));
    }

    public IEnumerator Attack00(float speed, Transform atkPoint, Transform tg)
    {
        anim.SetTrigger("beatB");
        yield return new WaitForSeconds(0.5f);
        PandoraAttack.instance.BeatB_FireArrow(speed, atkPoint, tg.position, attackPoint);
    }

    public IEnumerator Attack01()
    {
        anim.SetTrigger("beatC");
        yield return new WaitForSeconds(0.5f);
        PandoraAttack.instance.BeatC_LightningArrow();
    }

    public IEnumerator Attack02(float _speed, Transform _atkPoint, Transform _tg)
    {
        anim.SetTrigger("beatD");
        yield return new WaitForSeconds(0.5f);
        PandoraAttack.instance.BeatD_IceArrow(_speed, _atkPoint, _tg.position, attackPoint);
    }

    protected override void BuffATK()
    {
        anim.SetTrigger("beatE");
        attackDamage = attackDamage + (attackDamage / 10);
    }

    protected override void BuffDEF()
    {
        anim.SetTrigger("beatE");
        armor = armor + (armor / 10);
    }

    private IEnumerator Movement()
    {
        UIController.instance.OnAlertWarning("Pandora chuẩn bị ra khỏi tổ, hãy sẵn sàng tấn công!!!");
        GameObject teleport = transform.GetChild(2).gameObject;
        teleport.SetActive(true);
        yield return new WaitForSeconds(2f);
        Vector2 newPosition = new Vector2(6f, -1.8f);
        this.transform.position = newPosition;
        teleport.SetActive(false);
        UIController.instance.OffAlertWarning();
        gameObject.layer = LayerMask.NameToLayer("Boss");
    }
}