using UnityEngine;

public class FootStepsSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] stoneClips;
   // [SerializeField] private AudioClip[] mudClips;
   // [SerializeField] private AudioClip[] grassClips;

    [SerializeField] AudioSource audioSource;
    //private TerrainDetector terrainDetector;
    /*
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        //terrainDetector = new TerrainDetector();
    }
    */

    private void step()
    {
        //AudioClip clip = GetRandomClip();
       // AudioClip clip = stoneClips[UnityEngine.Random.Range(0, stoneClips.Length)];
        AudioClip clip = stoneClips[0];
        audioSource.PlayOneShot(clip);
        Debug.Log("step");
    }

/*
    private AudioClip GetRandomClip()
    {
        //int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);

        switch(terrainTextureIndex)
        {
            case 0:
                return stoneClips[UnityEngine.Random.Range(0, stoneClips.Length)];
            case 1:
                return mudClips[UnityEngine.Random.Range(0, mudClips.Length)];
            case 2:
            default:
                return grassClips[UnityEngine.Random.Range(0, grassClips.Length)];
        }
        
    }
*/
}