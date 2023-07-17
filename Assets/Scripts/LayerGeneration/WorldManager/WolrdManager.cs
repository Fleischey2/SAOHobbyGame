using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace terrain {
    public class WolrdManager : MonoBehaviour
    {
        public Material mat;
        private Container container;

        //public GameObject town;

        public int worldWidth;
        public int worldHeight;
        public int chunkWidth = 16;
        public int chunkHeight = 16;
        public int chunkSize = 16;

        int width; // Breite des 2D-Arrays
        int height; // Höhe des 2D-Arrays
        float scale = 80f; // Skalierungsfaktor für die Feinheit der Höhenwerte

        float[,] heights;   

        // Start is called before the first frame update
        void Start()
        {
            width = worldWidth * chunkWidth;
            height = worldHeight * chunkHeight;

            heights = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float xCoord = (float)x /10;
                    float yCoord = (float)y /10;
                    
                    float heightValue;

                    if((y <= height - chunkHeight&&y >= height - 4 * chunkHeight) && (x <= width - chunkWidth&&x >= width - 4 * chunkWidth)) {

                        heightValue = 0;

                    } else if((y >= chunkHeight && y <= 13 * chunkHeight) && (x >= chunkWidth && x <= 13* chunkWidth)) {
                        
                        heightValue = 0;
                        
                    } else {

                        heightValue =  (Mathf.Pow ((Mathf.PerlinNoise(x/scale,y/scale)*6),(2) ));

                        UnityEngine.Debug.Log("height: " + heightValue);

                    }

                    heights[x, y] = heightValue;

                    
                }
            }

            int startX = 0;
            int startY = 0;

            for(int x1 = 0; x1 < worldWidth; x1++) {
                
                startX = x1 * 16;

                for(int z1 = 0; z1 < worldHeight; z1++) {
                    
                    GameObject cont = new GameObject("Chunk");
                    cont.layer = LayerMask.NameToLayer("whatIsGround");
                    cont.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    cont.transform.parent = transform;
                    container = cont.AddComponent<Container>();
                    container.Initialize(mat, new Vector3(x1 * 16,0,z1 * 16));
                    float[,] chunk = new float[chunkSize, chunkSize];

                    startY = z1 * 16;

                    for (int x = 0; x < chunkSize; x++)
                    {
                        for (int y = 0; y < chunkSize; y++)
                        {
                            //UnityEngine.Debug.Log("xWidth: " + width + "yheight: " + height);
                            //UnityEngine.Debug.Log("x: " + (startX + x) + "y: " + (startY + y));
                            chunk[x, y] = heights[startX + x, startY + y];
                        }
                    }

                    for (int x = 0; x < 16; x++)
                    {
                        for (int z = 0; z < 16; z++)
                        {
                            
                            for(int y = (int) chunk[x,z] ;y >= 0; y--) {
                                container[(new Vector3(x, y, z) + container.getPosition())] = new Voxel() { ID = 1 };
                            }
                            
                        }
                    }

                    container.GenerateMesh();
                    container.UploadMesh();

                }
            }

            //Instantiate(town, new Vector3(16,0.65f, 16), Quaternion.identity);



        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}