using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class Button20Chaos : TextureButton
{
	[Signal]
	public delegate void ProgressSpellEventHandler(int spellStep);
	[Signal]
	public delegate void TriggerBurningOnEventHandler();
	[Signal]
	public delegate void TriggerPaintingOnEventHandler();
	
	[Export]
	AudioStream soundToPlay;

	bool chaosMode = false;

	IEnumerable<string> mp3Files = null;

	int everyTick = 50;
	int currentTick = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//get sound files
		mp3Files = Directory.GetFiles("Sounds").ToList().Where(filename => filename.EndsWith(".mp3"));
		GD.Print($"found {mp3Files.Count()} sound files to play");
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (chaosMode == true && (currentTick++ % everyTick == 0))
		{
			PlayRandomSound();
		}
	}
	private void PlayRandomSound()
	{
		if (mp3Files == null)
		{
			GD.Print("no sounds to play");
			return;
		}
		else
		{
			var randomNumber = GD.Randi();
			//GD.Print($"random number: {randomNumber}");
			var moddedNumber = randomNumber % mp3Files.Count();
			//GD.Print($"modded number: {moddedNumber}");
			int soundIndexToChoose = (int)moddedNumber;
			//GD.Print($"as an int: {soundIndexToChoose}");

			var randomSound = mp3Files.ElementAt(soundIndexToChoose);
			try
			{
				GD.Print($"Playing {randomSound}");
				var soundToPlay = ResourceLoader.Load(randomSound) as AudioStream;

				AudioStreamPlayer2D newSound = new AudioStreamPlayer2D
				{
					Stream = soundToPlay
				};
				AddChild(newSound);
				newSound.Play();
			}
			catch (Exception e)
			{
				GD.Print($"Got exception while loading sound: {e}");
			}
		}
	}

	private void ChaosOff()
	{
		chaosMode = false;
	}

	private void PlaySound()
	{
		GD.Print("BeginChaos.exe");
		chaosMode = true;

		EmitSignal("TriggerBurningOn");
		EmitSignal("TriggerPaintingOn");

		var arrow = ResourceLoader.Load("Images/brushlighter.png");
		Input.SetCustomMouseCursor(arrow);
		
        AudioStreamPlayer2D newSound = new AudioStreamPlayer2D
        {
            Stream = soundToPlay
        };
		AddChild(newSound);
        newSound.Play();
		EmitSignal("ProgressSpell", -1);
	}
}
