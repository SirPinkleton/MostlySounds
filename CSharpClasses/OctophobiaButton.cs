using Godot;
using System;

public partial class OctophobiaButton : TextureButton
{
	Node playingSpace;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playingSpace = GetNode("../../PlayingSpace");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void ToggleShowingEights(bool hideEights)
	{
		SearchForEights(playingSpace, hideEights);
	}

	private void SearchForEights(Node theNode, bool hideEight)
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
			if (theNode.Name.ToString().ToLower().StartsWith("8label"))
			{
				//richtextlabel . Text
				GD.Print("Found an 8, adjusting");
				RichTextLabel asLabel = (RichTextLabel)theNode;
				if (hideEight)
				{
					asLabel.Text = asLabel.Text.Replace("8","7+1");
					asLabel.PushFontSize(30);
					asLabel.Position += new Vector2(-20,0);
				}
				else
				{
					asLabel.Text = asLabel.Text.Replace("7+1","8");
					asLabel.PushFontSize(40);
					asLabel.Position += new Vector2(20,0);

				}
			}
			else
			{
				//GD.Print("No 8 found, moving on.");
			}
			foreach (var child in children)
			{
				SearchForEights(child, hideEight);
			}
		}
	}
}
