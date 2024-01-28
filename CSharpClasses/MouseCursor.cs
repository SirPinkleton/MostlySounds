using Godot;
using System;

public partial class MouseCursor : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Setting cursor image");
		// Load the custom images for the mouse cursor.
		var arrow = ResourceLoader.Load("Images/arrow.png");

		// Changes only the arrow shape of the cursor.
		// This is similar to changing it in the project settings.
		Input.SetCustomMouseCursor(arrow);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
