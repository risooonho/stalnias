﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileLayer : MonoBehaviour {
    Texture2D _mapTileset;

    TileMap tilemap {
        get {
            return transform.parent.GetComponent<TileMap>();
        }
    }
    
    //public Texture2D[] tileTextures;
    public int textureResolution = 32;

    public MapLayer layerdata;

    // Use this for initialization
    void Start () {
        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        mesh_renderer.sharedMaterials = new Material[] { tilemap.material };
        _mapTileset = tilemap.mapTileset;
        mesh_renderer.sharedMaterials[0].mainTexture = _mapTileset;

        BuildMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BuildMap() {
        BuildTileMap();
    }

    Vector2[] getUVForTileType(int id) {
        int tilesetwidth = _mapTileset.width / textureResolution;

        int x = id % tilesetwidth;
        int y = id / tilesetwidth;

        Vector2[] uv = new Vector2[4];
        uv[0]=new Vector2((float)x/tilesetwidth, (float)y/ tilesetwidth);
        uv[1] = new Vector2((float)(x + 1) / tilesetwidth, (float)y / tilesetwidth);
        uv[2] = new Vector2((float)x / tilesetwidth, (float)(y+1) / tilesetwidth);
        uv[3] = new Vector2((float)(x + 1) / tilesetwidth, (float)(y+1) / tilesetwidth);

        return uv;
    }

    public void BuildTileMap() {
        int size_x = layerdata.width;
        int size_y = layerdata.height;
        
        int numTiles = size_x * size_y;
        int numVertices = numTiles*4;

        Vector3[] vertices = new Vector3[numVertices];
        Vector3[] normals = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];
        int[] triangleVertices = new int[numTiles * 2 * 3]; // numTiles * numTrianglesPerTile * numVerticesPerTriangle
        
        for(int y=0; y < size_y; y++) {
            for(int x=0; x < size_x; x++) {
                if (layerdata.tileTypeIdAt(x, y) == 0) continue;
                int verticeTileIndex = (y * size_x + x) * 4;
                Vector2[] tileuv = getUVForTileType(layerdata.tileTypeIdAt(x,y));
                for (int v = 0; v < 4; v++) {
                    int vx = v%2;
                    int vy = v/2;
                    
                    vertices[verticeTileIndex + v] = new Vector3((x+vx) * tilemap.tileSize, (y+vy) * tilemap.tileSize);
                    normals[verticeTileIndex + v] = Vector3.forward;
                    uv[verticeTileIndex + v] = tileuv[v];
                }

                int firstTriangleEdgeOfTile = (y * size_x + x) * 2 * 3;     // (tileRow * tileRowSize + tileInRow) * numTrianglesPerTile * numVerticesPerTriangle
                
                triangleVertices[firstTriangleEdgeOfTile + 0] = verticeTileIndex;
                triangleVertices[firstTriangleEdgeOfTile + 1] = verticeTileIndex + 2;
                triangleVertices[firstTriangleEdgeOfTile + 2] = verticeTileIndex + 3;

                triangleVertices[firstTriangleEdgeOfTile + 3] = verticeTileIndex;
                triangleVertices[firstTriangleEdgeOfTile + 4] = verticeTileIndex + 3;
                triangleVertices[firstTriangleEdgeOfTile + 5] = verticeTileIndex + 1;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangleVertices;
        mesh.normals = normals;
        mesh.uv = uv;

        MeshFilter mesh_filter = GetComponent<MeshFilter>();
        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        MeshCollider mesh_collider = GetComponent<MeshCollider>();

        mesh_filter.mesh = mesh;
    }
}
