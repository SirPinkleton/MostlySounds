using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class StopAllSoundsButton : TextureButton
{
	[Signal]
	public delegate void TriggerPaintingOffEventHandler();
	[Signal]
	public delegate void TriggerBurningOffEventHandler();
	[Signal]
	public delegate void TriggerChaosOffEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SilenceAllButtonNoises()
	{
		var playingSpace = GetNode("../../PlayingSpace");
		GD.Print("looking through play space for all the sound effects");
		SearchForSound(playingSpace);
		SearchForThing(playingSpace, "Nepeta");
		SearchForThing(playingSpace, "Stroke");
		SearchForThing(playingSpace, "Burn");
		SearchForThing(playingSpace, "BrokenButton");
		// Send a signal so that brush strokes start happening
		EmitSignal("TriggerPaintingOff");
		EmitSignal("TriggerBurningOff");
		EmitSignal("TriggerChaosOff");
		
		//fix cursor
		var arrow = ResourceLoader.Load("Images/arrow.png");
		Input.SetCustomMouseCursor(arrow);

		//fix button
		var buttonToFix = playingSpace.GetNode<TextureButton>("Button3Pipe");
		buttonToFix.Show();
	}

	private void SearchForSound(Node theNode)
	{
		if (theNode == null)
		{
			//GD.Print("node is null, returning");
			return;
		}
		else
		{
			//GD.Print($"Looking at {theNode.Name}");
			try
			{
				AudioStreamPlayer2D nodeSound = (AudioStreamPlayer2D) theNode;
				//GD.Print("found a music player, stopping it");
				nodeSound.Stop();
			}
			catch
			{
				//GD.Print("No sound found, moving on.");
			}
			var children = theNode.GetChildren();
			foreach (var child in children)
			{
				SearchForSound(child);
			}
		}
	}
	
	private void SearchForThing(Node theNode, string thing)
	{
		if (theNode == null)
		{
			//GD.Print("node is null, returning");
			return;
		}
		else
		{
			GD.Print($"Looking at {theNode.Name}");
			var children = theNode.GetChildren();
			if (theNode.Name.ToString().StartsWith(thing))
			{
				GD.Print($"found a {thing}, removing");
				theNode.QueueFree();
			}
			else
			{
				GD.Print($"No {thing} found, moving on.");
			}
			foreach (var child in children)
			{
				SearchForThing(child, thing);
			}
		}
	}

}
