using UnityEngine;

public class PlayerBombAttack : MonoBehaviour
{
    //プレイヤーの爆弾攻撃を管理するスクリプト
    //爆弾が爆発する部分はメソッドで作成し、後の組み合わせで再利用できるようにする
    //また、bombを投げる動作は別のメソッドから引っ張ってきて再利用する
    public GameObject bombPrefab;        //投げる爆弾のPrefab
}
