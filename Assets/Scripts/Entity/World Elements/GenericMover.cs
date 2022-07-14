using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Photon.Pun;

public class GenericMover : MonoBehaviour {

    public AnimationCurve x;
    public AnimationCurve y;

    public float animationTimeSeconds = 1;

    public AudioSource sfx;

    public List<AudioClip> sound_list;
    public List<float> soundTime_list;
    //private List<bool> sound_executed = new List<bool> {false, false, false, false, false};

    private Vector3? origin = null;
    private double timestamp = 0;
    private double lastTime = 0;



    public void Awake() {
        if (origin == null)
            origin = transform.position;
    }

    public void Update() {
        int start = GameManager.Instance.startServerTime;

        if (PhotonNetwork.Time <= timestamp) {
            timestamp += Time.deltaTime;
        } else {
            timestamp = (float) PhotonNetwork.Time;
        }

        double time = timestamp - (start / (double) 1000);
        time /= animationTimeSeconds;
        time %= 1.0;
        print(time);

        for (int i = 0; i < sound_list.Count; i ++)
        {
            float st = soundTime_list[i];
            if (time >= st && lastTime < st)
            {
                //photonView.RPC("PlaySound", RpcTarget.All, sound_list[i]);
                sfx.PlayOneShot(sound_list[i], 2);
            }
        }

        lastTime = time;

        transform.position = (origin ?? default) + new Vector3(x.Evaluate((float) time), y.Evaluate((float) time), 0);
    }
}