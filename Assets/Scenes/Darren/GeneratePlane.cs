using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GeneratePlane : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] tris;
    Vector2[] uvs;


    [SerializeField] private int xSize, zSize;
    int currentVertex;


    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        //verts
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];


        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {

                vertices[currentVertex] = new Vector3(x, 0, z);

                currentVertex++;
            }
        }

        //tris
        tris = new int[xSize * zSize * 6];

        int tri = 0;
        int vert = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                tris[tri] = vert; // same as vertice
                tris[tri + 1] = vert + xSize + 1; // y + 1
                tris[tri + 2] = vert + 1; // x + 1
                tris[tri + 3] = vert + xSize + 1; // y + 1
                tris[tri + 4] = vert + xSize + 2; // x + 1 && y + 1
                tris[tri + 5] = vert + 1; // x + 1

                vert++;
                tri += 6;
            }
            vert++;
        }

        //uvs
        uvs = new Vector2[(xSize + 1) * (zSize + 1)];
        float xDivider = 1f / (xSize);
        float zDivider = 1f / (zSize);
        //Debug.Log(xDivider);
        //Debug.Log(yDivider);

        for (int i = 0, z = 0; z < zSize + 1; z++)
        {
            for (int x = 0; x < xSize + 1; x++)
            {
                //Debug.Log("x = " + x + " | y = " + y + " | xPos = " + (x * xDivider) + " | yPos = " + (y * yDivider));
                uvs[i] = new Vector2(x * xDivider, z * zDivider);
                i++;
            }
        }




        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();
    }
}