using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAttack : MonoBehaviour
{
    [SerializeField] Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator.Play("MeleeAttack");
    }

    private void Awake() {
        StartCoroutine("DestroyByTime");
    }

    IEnumerator DestroyByTime() {
        yield return new WaitForSeconds(2f);
        this.GetComponent<PhotonView>().RPC("destroyAnimation", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void destroyAnimation() {
        Destroy(this.gameObject);
    }
}
