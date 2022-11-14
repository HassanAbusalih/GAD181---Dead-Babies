using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvoloutionUI : MonoBehaviour
{
    public GameObject evolutionUI;
    public Image pokemonImage;
    public BattleDialogue dialogue;
    public AudioSource evoMusic;
    public AudioSource youEvolvedSound;
    public AudioSource glitterSfx;
    public ParticleSystem particle;

    public IEnumerator Evolve(Pokemon pokemon)
    {
        evolutionUI.SetActive(true);
        pokemonImage.sprite = pokemon.pokemonBase.pokeSprite;
        yield return dialogue.SetDialogue($"{pokemon.pokemonBase.name} is evolving.");
        evoMusic.Play();
        pokemonImage.color = Color.black;
        yield return Particle();
        var oldPokemon = pokemon.pokemonBase.name;
        pokemon.Evolve();
        evoMusic.Stop();
        pokemonImage.sprite = pokemon.pokemonBase.pokeSprite;
        particle.Stop();
        pokemonImage.color = Color.white;
        youEvolvedSound.Play();
        yield return dialogue.SetDialogue($"{oldPokemon} evolved into {pokemon.pokemonBase.name}.");
        yield return new WaitForSeconds(2f);
        evolutionUI.SetActive(false);
    }
    public IEnumerator Particle()
    {
        particle.Play();
        glitterSfx.Play();
        var em = particle.emission;
        for (int i = 10; i < 100; i++)
        {
            em.rateOverTime = i;
            yield return new WaitForSeconds(.1f);
        }
        particle.Stop();
        glitterSfx.Stop();
    }
}
