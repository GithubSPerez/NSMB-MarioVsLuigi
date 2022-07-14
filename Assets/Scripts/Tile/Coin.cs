using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;
/*
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;

[CreateAssetMenu]
public class PrefabTile : UnityEngine.Tilemaps.TileBase
{
    public Sprite Sprite; //The sprite of tile in a palette and in a scene
    public GameObject Prefab; //The gameobject to spawn

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        if (Sprite) tileData.sprite = Sprite; // Asigning sprite
        tileData.gameObject = Prefab; // Assigning prefab
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        // Streangly the position of gameobject starts at Left Bottom point of cell and not at it center
        go.transform.position += Vector3.up * 0.5f + Vector3.right * 0.5f;

        return base.StartUp(position, tilemap, go);
    }
}
*/

[CreateAssetMenu(fileName = "Coin-onTile", menuName = "ScriptableObjects/Tiles/Coin", order = 1)]
public class Coin : UnityEngine.Tilemaps.TileBase
{
    public string CoinPrefab;
    GameObject gObj;
   
    // Start is called before the first frame update
    void Start()
    {
        //Vector3Int tileLocation = Utils.WorldToTilemapPosition(worldLocation);

        //PhotonNetwork.InstantiateRoomObject(CoinPrefab, new Vector2(3, 3), Quaternion.identity);
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        // Streangly the position of gameobject starts at Left Bottom point of cell and not at it center
        //go.transform.position += Vector3.up * 0.5f + Vector3.right * 0.5f;
        gObj = go;

        PhotonNetwork.InstantiateRoomObject(CoinPrefab, go.transform.position, go.transform.rotation);

        return base.StartUp(position, tilemap, go);
    }

    // Update is called once per frame
    void Update()
    {
        //Instantiate(CoinPrefab, new Vector3(4, 4, 0), Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject(CoinPrefab, gObj.transform.position, gObj.transform.rotation);
    }
}
