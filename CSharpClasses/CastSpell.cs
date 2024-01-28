using Godot;
using System;

public partial class CastSpell : Node2D
{
	[Export]
	AudioStream soundToPlay;

	public int currentSpellStep;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentSpellStep = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void HandleSpellSignals(int spellStepFired)
	{
		GD.Print($"Spell currently at: {currentSpellStep}, given spell step: {spellStepFired}");
		if (spellStepFired == currentSpellStep)
		{
			GD.Print($"spell component successful, incrementing to new step.");
			currentSpellStep++;
		}
		else if (spellStepFired == 69)
		{
			GD.Print($"lolnice");
			currentSpellStep = 0;
		}
		else
		{
			GD.Print($"spell component incorrect, resetting progress.");
			if (currentSpellStep > 0)
			{
				AudioStreamPlayer2D newSound = new AudioStreamPlayer2D
				{
					Stream = soundToPlay
				};
				AddChild(newSound);
				newSound.Play();
			}
			currentSpellStep = 0;
		}
	}
}
