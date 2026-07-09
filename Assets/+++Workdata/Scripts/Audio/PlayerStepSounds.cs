using System.Collections;
using UnityEngine;


public class PlayerSounds : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource; //Audio source to play the sounds
    
    // Arrays for different types of Player sounds
    [Header("Walk Sounds")] 
    [SerializeField] private AudioClip[] grassWalkStepSounds;
    [SerializeField] private AudioClip[] itemSound;
    [SerializeField] private AudioClip[] attackSound;
    [SerializeField] private AudioClip[] hitSound;

    public void PlayWalkStepSound() // Plays a random walk step sound based on the ground type
    {
        PlayRandomSound(grassWalkStepSounds); // Grass walk sound
    }
    public void ItemSound() // Plays a random walk step sound based on the ground type
    {
        PlayRandomSound(itemSound); // Grass walk sound
    }
    public void attackStepSound() // Plays a random walk step sound based on the ground type
    {
        PlayRandomSound(attackSound); // Grass walk sound
    }
   
    public void hitSounds() // Plays a random walk step sound based on the ground type
    {
        PlayRandomSound(hitSound); // Grass walk sound
    }

    void PlayRandomSound(AudioClip[] audioClips) //Picks a random clip from the given array and play it
    {
        int index = Random.Range(0, audioClips.Length); // Get a random index
        audioSource.PlayOneShot(audioClips[index]); // Play the selected sound
    }
}

