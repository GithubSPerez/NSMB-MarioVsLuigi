using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrappingHitbox : MonoBehaviour {
    
    private Rigidbody2D body;
    private BoxCollider2D[] ourColliders, childColliders, vertColliders, cornerColliders;
    private float levelMiddle, levelWidth;
    private bool levelVertLoop = false;
    private float levelYMiddle, levelHeight;
    private Vector2 xOffset, yOffset;
    private Vector2 fullOffset = Vector2.zero;

    void Awake() {
        body = GetComponent<Rigidbody2D>();
        if (!body)
            body = GetComponentInParent<Rigidbody2D>();
        ourColliders = GetComponents<BoxCollider2D>();
        LateUpdate();
    }
    public void LateUpdate() {
        if (!GameManager.Instance) 
            return;
        if (!GameManager.Instance.loopingLevel) {
            enabled = false;
            return;
        }


        if (fullOffset == Vector2.zero) {
            levelVertLoop = GameManager.Instance.levelVerticalLoop;

            childColliders = new BoxCollider2D[ourColliders.Length];
            vertColliders = new BoxCollider2D[ourColliders.Length];
            cornerColliders = new BoxCollider2D[ourColliders.Length];

            for (int i = 0; i < ourColliders.Length; i++)
            {
                childColliders[i] = gameObject.AddComponent<BoxCollider2D>();
                if (levelVertLoop)
                {
                    vertColliders[i] = gameObject.AddComponent<BoxCollider2D>();
                    cornerColliders[i] = gameObject.AddComponent<BoxCollider2D>();
                }
            }

            levelWidth = GameManager.Instance.levelWidthTile/2f;
            levelMiddle = GameManager.Instance.GetLevelMinX() + levelWidth/2f;

            levelHeight = GameManager.Instance.levelHeightTile / 2f;
            levelYMiddle = GameManager.Instance.GetLevelMinY() + levelHeight / 2f;

            xOffset = new Vector2(levelWidth, 0);
            yOffset = new Vector2(0, levelHeight);
            fullOffset = xOffset + yOffset;
        }

        for (int i = 0; i < ourColliders.Length; i++)
        {
            UpdateChildColliders(i);
        }
    }
    
    void UpdateChildColliders(int index) {
        BoxCollider2D ourCollider = ourColliders[index];
        BoxCollider2D[] colliders = new BoxCollider2D[3];

        colliders[0] = childColliders[index];
        if (levelVertLoop)
        {
            colliders[1] = vertColliders[index];
            colliders[2] = cornerColliders[index];
        }

        for (int j = 0; j < (levelVertLoop ? 3 : 1); j ++)
        {
            BoxCollider2D coll = colliders[j];
            coll.autoTiling = ourCollider.autoTiling;
            coll.edgeRadius = ourCollider.edgeRadius;
            coll.enabled = ourCollider.enabled;
            coll.isTrigger = ourCollider.isTrigger;
            coll.sharedMaterial = ourCollider.sharedMaterial;
            coll.size = ourCollider.size;
            coll.usedByComposite = ourCollider.usedByComposite;
            coll.usedByEffector = ourCollider.usedByComposite;

            coll.offset = ourCollider.offset;
            if (j == 0 || j == 2) coll.offset += (((body.position.x < levelMiddle) ? xOffset : -xOffset) / body.transform.lossyScale);
            if (j == 1 || j == 2) coll.offset += (((body.position.y < levelYMiddle) ? yOffset : -yOffset) / body.transform.lossyScale);
        }
    }
}
