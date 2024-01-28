using Godot;
using System;

public partial class Options : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void ShowOptions()
	{
		Show();
		var openSettingsButton = GetNode<Node2D>("../PlayingSpace");
		openSettingsButton.Hide();
	}
	private void HideOptions()
	{
		Hide();
		//need to re-show the button to re-open settings
		var openSettingsButton = GetNode<Node2D>("../PlayingSpace");
		openSettingsButton.Show();
	}
}
