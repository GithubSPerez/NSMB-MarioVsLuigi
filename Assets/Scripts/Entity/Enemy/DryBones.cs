using UnityEngine;
using Photon.Pun;
using NSMB.Utils;

public class DryBones : KillableEntity {
    [SerializeField] float speed, deathTimer = -1, terminalVelocity = -8;
    float stateTimer = 0;

    int sleepState = 0;
    bool playedWakeSound = false;

    new void Start() {
        base.Start();
        body.velocity = new Vector2(speed * (left ? -1 : 1), body.velocity.y);
        animator.SetBool("dead", false);
    }

    new void FixedUpdate() {
        if (GameManager.Instance && GameManager.Instance.gameover) {
            body.velocity = Vector2.zero;
            body.angularVelocity = 0;
            animator.enabled = false;
            body.isKinematic = true;
            return;
        }

        base.FixedUpdate();

        switch (sleepState)
        {
            case 0:
                stateTimer = 0;
                break;
            case 1:
                stateTimer += Time.fixedDeltaTime;
                if (stateTimer >= 4)
                {
                    stateTimer = 0;
                    sleepState = 2;
                    animator.SetBool("sleep", false);
                    animator.SetBool("wake", true);
                    PlayClip("enemy/drybones_shake");
                }
                break;
            case 2:
                stateTimer += Time.fixedDeltaTime;
                if (stateTimer >= 1 && !playedWakeSound)
                {
                    playedWakeSound = true;
                    PlayClip("enemy/drybones_wake");
                }
                if (stateTimer >= 2.35)
                {
                    body.velocity = new Vector2(speed * (left ? -1 : 1), body.velocity.y);
                    body.isKinematic = false;
                    //speed = 0;
                    dead = false;
                    sleepState = 0;
                    hitbox.enabled = true;
                    animator.SetBool("wake", false);
                    playedWakeSound = false;
                }
                break;
        }

        if (dead) {
            if (deathTimer >= 0 && (photonView?.IsMine ?? true)) {
                Utils.TickTimer(ref deathTimer, 0, Time.fixedDeltaTime);
                if (deathTimer == 0)
                    PhotonNetwork.Destroy(gameObject);
            }
            return;
        }

        physics.UpdateCollisions();
        if (physics.hitLeft || physics.hitRight) {
            left = physics.hitRight;
        }
        body.velocity = new Vector2(speed * (left ? -1 : 1), Mathf.Max(terminalVelocity, body.velocity.y));
        sRenderer.flipX = !left;
    }

    [PunRPC]
    public override void Kill() {
        body.velocity = Vector2.zero;
        body.isKinematic = true;
        PlayClip("enemy/drybones_sleep");
        //speed = 0;
        dead = true;
        sleepState = 1;
        hitbox.enabled = false;
        animator.SetBool("sleep", true);
    }
}