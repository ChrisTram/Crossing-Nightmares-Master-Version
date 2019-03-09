using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Responsible for adding and maintaining characters in the scene.
/// </summary>
public class CharacterManager : MonoBehaviour 
{
	public static CharacterManager instance;

	/// <summary>
	/// All characters must be attached to the character panel.
	/// </summary>
	public RectTransform characterPanel;

	/// <summary>
	/// A list of all characters currently in our scene.
	/// </summary>
	public List<Character> characters = new List<Character>();

	/// <summary>
	/// Easy lookup for our characters.
	/// </summary>
	public Dictionary<string, int> characterDictionary = new Dictionary<string, int>();

	void Awake()
	{
		instance = this;
	}

	/// <summary>
	/// Try to get a character by the name provided from the character list.
	/// </summary>
	/// <returns>The character.</returns>
	/// <param name="characterName">Character name.</param>
	/// <param name="createCharacterIfDoesNotExist">If set to <c>true</c> create character if does not exist.</param>
	/// <param name="enableCreatedCharacterOnStart">If set to <c>true</c> enable created character on start.</param>
	public Character GetCharacter(string characterName, bool createCharacterIfDoesNotExist = true, bool enableCreatedCharacterOnStart = true)
	{
		//search our dictionary to find the character quickly if it is already in our scene.
		int index = -1;
		if (characterDictionary.TryGetValue (characterName, out index)) 
		{
			return characters [index];
		} 
		else if (createCharacterIfDoesNotExist)
		{
			return CreateCharacter (characterName, enableCreatedCharacterOnStart);
		}

		return null;
	}

	/// <summary>
	/// Creates the character.
	/// </summary>
	/// <returns>The character.</returns>
	/// <param name="characterName">Character name.</param>
	public Character CreateCharacter(string characterName, bool enableOnStart = true)
	{
		Character newCharacter = new Character (characterName, enableOnStart);

		characterDictionary.Add (characterName, characters.Count);
		characters.Add (newCharacter);

		return newCharacter;
	}

	public class CHARACTERPOSITIONS
	{
		public Vector2 bottomLeft = new Vector2 (0, 0);
		public Vector2 topRight = new Vector2 (1f, 1f);
		public Vector2 center = new Vector2 (0.5f, 0.5f);
		public Vector2 bottomRight = new Vector2 (1f, 0);
		public Vector2 topLeft = new Vector2 (0, 1f);
	}
	public static CHARACTERPOSITIONS characterPositions = new CHARACTERPOSITIONS();
}
