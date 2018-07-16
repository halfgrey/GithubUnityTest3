using UnityEngine;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;
using System;

public class PerlinScroller : MonoBehaviour {

    int cubeCount;
    public int width = 500;
    public int height = 500;
    public int layers = 3;

    GameObject[] cubes;


    Transform[] cubeTransforms;
    TransformAccessArray cubeTransformsAccessArray;
    PositionUpdateJob cubeJob;
    JobHandle cubePositionJobHandle;
    public bool useJobSystem;
    public Material cubeMaterial;

    void Awake()
    {
        cubeCount = (int)(width * height * layers);
        cubes = new GameObject[cubeCount];
        cubeTransforms = new Transform[cubeCount];
    }

    // Use this for initialization
    void Start () {
        cubes = CreateCubes(cubeCount);
        for(int i = 0; i < cubeCount; i++)
        {
            GameObject obj = cubes[i];
            cubeTransforms[i] = obj.transform;
        }
        cubeTransformsAccessArray = new TransformAccessArray(cubeTransforms);
	}

    public GameObject[] CreateCubes(int count)
    {
        GameObject[] cubes = new GameObject[count];
        GameObject cubeToCopy = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Renderer renderer = cubeToCopy.GetComponent<MeshRenderer>();

        renderer.material = cubeMaterial;

        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;

        Collider collider = cubeToCopy.GetComponent<Collider>();
        collider.enabled = false;
        for(int i= 0; i < count; i++)
        {
            GameObject cube = GameObject.Instantiate(cubeToCopy);
            int x = i / (width * layers);
            cube.transform.position = new Vector3(x, 0, (i - x * height * layers) / layers);
            cubes[i] = cube;
        }
        GameObject.Destroy(cubeToCopy);
        return cubes;
    }
    int xoffset = 0;

    struct PositionUpdateJob : IJobParallelForTransform
    {
        public int height;
        public int width;
        public int layers;
        public int xoffset;
        public int zoffset;
        public void Execute(int i, TransformAccess transform)
        {
            int x = i / (width * layers);
            int z = (i - x * height * layers) / layers;
            int yoffset = i - x * width * layers - z * layers;
            transform.position = new Vector3(x, GeneratePerlinHeight(x + xoffset, z + zoffset) + yoffset, z + zoffset);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (useJobSystem)
        {
            //   int xoffset = (int)(this.transform.position.x - width / 2.0f);
            cubeJob = new PositionUpdateJob()
            {
                xoffset = xoffset++,
                zoffset = (int)(this.transform.position.z - height / 2.0f),
                height = height,
                width = width,
                layers = layers
            };
            cubePositionJobHandle = cubeJob.Schedule(cubeTransformsAccessArray);

        }
        else { 
           // OLD CODE WITHOUT JOB SYSTEM
            xoffset++;
            int zoffset = (int)(this.transform.position.z - height / 2.0f);
            for (int i = 0; i < cubeCount; i++)
            {
                int x = i / (width * layers);
                int z = (i - x * height * layers) / layers;
                int yoffset = i - x * width * layers - z * layers;
                cubes[i].transform.position = new Vector3(x, GeneratePerlinHeight(x + xoffset, z + zoffset) + yoffset, z + zoffset);
            }
        }
    }

    public void LateUpdate()
    {
        cubePositionJobHandle.Complete();
    }

    private void OnDestroy()
    {
        cubeTransformsAccessArray.Dispose();
    }



    static float GeneratePerlinHeight(float posx, float posz)
    {
        float smooth = 0.03f;
        float heightMult = 5;
        float height = (Mathf.PerlinNoise(posx * smooth,
            posz * smooth * 2) * heightMult + 
            Mathf.PerlinNoise(posx * smooth, 
            posz * smooth * 2) * heightMult) / 2.0f;
        return (height * 10);
    }
}
