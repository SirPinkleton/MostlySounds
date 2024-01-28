using Godot;
using System;
using System.Threading.Tasks;

public partial class SoundButton : TextureButton
{
	[Export]
	AudioStream soundToPlay;
	[Signal]
	public delegate void ProgressSpellEventHandler(int spellStep);

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
		EmitSignal("ProgressSpell", -1);
		while (newSound.Playing)
		{
			await Task.Delay(500);
		}
		newSound.QueueFree();
	}
}
