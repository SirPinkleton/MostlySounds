using Godot;
using System;

public partial class spell_instructions : Node2D
{
	[Export]
	AudioStream soundToPlay;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void ToggleShowingSpellInstructions(bool showInstructions)
	{
		if (showInstructions)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}
}
