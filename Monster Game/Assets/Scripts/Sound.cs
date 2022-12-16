using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {
	/*
	Esta clase es una clase unica para Audio manager aunque cualquier codigo puede acceder a esto
	Igual y podemos utilizar algo similar para otras cosas dentro de nuestro código, pero es algo que apenas conozco
	simplemente son varios valores de esta clase donde estos pueden ser modificados dentro de otros codigos y en el inspector
	*/
	public string name;
	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume;
	[Range(.1f, 3f)]
	public float pitch;
	public bool loop;

	[HideInInspector]
	public AudioSource source;
}
