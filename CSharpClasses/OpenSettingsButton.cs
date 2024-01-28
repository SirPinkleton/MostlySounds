using Godot;
using System;

public partial class OpenSettingsButton : TextureButton
{
	Node2D playingSpace;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playingSpace = GetNode<Node2D>("../../PlayingSpace");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void ShowingOptions()
	{
		playingSpace.Hide();
	}
}
