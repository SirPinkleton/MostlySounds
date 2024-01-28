using Godot;
using System;
using System.Threading.Tasks;

public partial class Button16TimeToFunk : TextureButton
{
	[Signal]
	public delegate void ProgressSpellEventHandler(int spellStep);

	[Export]
	AudioStream soundToPlay;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private async void PlaySound()
	{
        AudioStreamPlayer2D newSound = new AudioStreamPlayer2D
        {
            Stream = soundToPlay
        };
		AddChild(newSound);
        newSound.Play();
		//Start The Spell
		EmitSignal("ProgressSpell", 0);
		while (newSound.Playing)
		{
			await Task.Delay(500);
		}
		newSound.QueueFree();
		//a failure state without sound
		EmitSignal("ProgressSpell", 69);

	}
}
