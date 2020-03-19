using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioClip[] footsteps_normal;
    public AudioClip[] footsteps_sneaking;

    public AudioSource source_footsteps;
    public AudioSource source_jumping;
    public AudioSource source_music;
    public AudioSource source_SFX;

    public AudioClip music_ingame;
    public AudioClip music_success;
    public AudioClip music_failure;

    public AudioClip death_player;
    public AudioClip death_enemy;

    public AudioClip player_jump;
    public AudioClip player_lever;

    public AudioClip enemy_spotted;

    public enum FootstepType { Normal, Sneaking };

    public enum Music { ingame, success, failure, none };

    public enum SFX { death_player, death_enemy, player_jump, player_lever, enemy_spotted };

    private static AudioManager _instance;

    public static AudioManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {
    }

    public void PlaySFX(SFX sfx)
    {
        if(sfx == SFX.death_enemy)
        {
            source_SFX.PlayOneShot(death_enemy);
        }
        else if(sfx == SFX.death_player)
        {
            source_SFX.PlayOneShot(death_player);
        }
        else if (sfx == SFX.enemy_spotted)
        {
            source_SFX.PlayOneShot(enemy_spotted);
        }
        else if (sfx == SFX.player_jump)
        {
            if(source_jumping.isPlaying == false)
            {
                source_jumping.PlayOneShot(player_jump);
            }
            
        }
        else if (sfx == SFX.player_lever)
        {
            source_SFX.PlayOneShot(player_lever);
        }

    }

    public void SetMusic(Music music)
    {
        if(music == Music.success)
        {
            source_music.Stop();
            source_music.clip = music_success;
            source_music.Play();
        }
        else if(music == Music.failure)
        {
            source_music.Stop();
            source_music.clip = music_failure;
            source_music.Play();
        }
        else if(music == Music.ingame)
        {
            source_music.Stop();
            source_music.clip = music_ingame;
            source_music.Play();
        }
        else if(music == Music.none)
        {
            source_music.Stop();
        }
    }

    public void PlayFootstep(FootstepType type)
    {
        if (PlayerController.Instance.Jumping()) { return; }

        if (source_footsteps.isPlaying == false && source_jumping.isPlaying == false)
        {
            source_footsteps.PlayOneShot(RandomFootstep(type));
        }
    }

    public AudioClip RandomFootstep(FootstepType type)
    {
        if(type == FootstepType.Normal)
        {
            return footsteps_normal[Random.Range(0,footsteps_normal.Length)];
        }
        else if(type == FootstepType.Sneaking)
        {
            return footsteps_sneaking[Random.Range(0, footsteps_sneaking.Length)];
        }
        else
        {
            return null;
        }
    }
}
