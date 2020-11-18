﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHit : MonoBehaviour
{
    [SerializeField] private PlayerContllole PC;
    [SerializeField] private GaugeProcessing GP;
    [SerializeField] private Level LE;
    //敵のMAX体力と現在(仮)
    [SerializeField]
    public int MaxHp = 3;
    int currentHp;
    //噛まれたときのノックバック
    public int back;
    [SerializeField]
    private GameObject EnemyUI;
    private Slider Hp;
    public float CountNumber = 0.0f;
    private float CountDown;
    Vector3 EP;

    void Start()
    {
        Hp = EnemyUI.transform.Find("EnemyHP").GetComponent<Slider>();
        Hp.value = 1f;
        currentHp = MaxHp;
        CountDown = CountNumber;
        Debug.Log("Start currentHp : " + currentHp);
    }

    void Update()
    {
        CountDown -= Time.deltaTime;
        //Debug.Log(GP.currentFull);
    }

    void OnCollisionEnter(Collision other)
    {

        //レイヤー0を取得している
        AnimatorClipInfo[] clip = PC.anim.GetCurrentAnimatorClipInfo(0);

        AnimatorStateInfo stat = PC.anim.GetCurrentAnimatorStateInfo(0);
        float len = stat.length;

        //クリップ(アニメーション)名を取得
        string clipName =clip[0].clip.name;
        Debug.Log(clipName);

        //Debug.Log("length" +stat.length);
        if (other.gameObject.tag== "Player" && clipName=="アーマチュア|カム")
        {
            if (CountDown < 0)
            {
                currentHp = currentHp - 1;
                //最大満腹における現在の満腹をSliderに反映。
                Hp.value = (float)currentHp / (float)MaxHp; ;
                Debug.Log("Start currentHp : " + currentHp);
                //エネミー(敵)のHPがゼロの場合の処理
                if (currentHp == 0)
                {
                    GP.currentFull += MaxHp;
                    LE.Geken += MaxHp;
                    if (GP.currentFull > GP.maxFull) {
                        GP.currentFull = GP.maxFull;
                    }
                    GP.Full.value = (float)GP.currentFull / (float)GP.maxFull;
                    Destroy(gameObject, 0.2f);
                    //Debug.Log(GP.currentFull);
                }
                //オブジェクト(エネミー)の位置を取る
                EP = gameObject.GetComponent<Transform>().position;
                //現在地からノックバック分を足す
                gameObject.GetComponent<Transform>().position = new Vector3(EP.x + back, EP.y, EP.z);
                CountDown = CountNumber;
            }
        }
    }
}
