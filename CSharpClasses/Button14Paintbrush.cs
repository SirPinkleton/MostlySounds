using Godot;
using System;

public partial class Button14Paintbrush : TextureButton
{
	[Signal]
	public delegate void ProgressSpellEventHandler(int spellStep);
	[Signal]
	public delegate void TriggerBurningOffEventHandler();
	bool canPaint = false;
	int strokeNumber = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (canPaint == true)
		{
			if (Input.IsActionPressed("left-click"))
			{
				var positionOfMouse = GetViewport().GetMousePosition();

				var spellParent = GetNode<CastSpell>("..");
				float xColor = 0;
				float yColor = 0;
				float zColor = 0;
				if (spellParent.currentSpellStep > 0 && strokeNumber < 150)
				{
					//pick blue
					xColor = 1;
					yColor = 0;
					zColor = 0;
				}
				else if (spellParent.currentSpellStep > 0 && strokeNumber >= 150)
				{
					//pick red
					xColor = 0;
					yColor = 0;
					zColor = 1;
				}
				else
				{
					//pick a random color
					xColor = GD.Randf();
					yColor = GD.Randf();
					zColor = GD.Randf();
				}

				Sprite2D strokeSprite = new Sprite2D
				{
					Name = "Stroke" + strokeNumber++,
					Texture = (Texture2D)ResourceLoader.Load("Images/brushstroke.png"),
					Position = positionOfMouse,
					Modulate = new Color(xColor,yColor,zColor)
				};
				var parent = GetNode("..");
				parent.AddChild(strokeSprite);
			}
		}
	}

	private void PaintingOn()
	{
		canPaint = true;
	}

	private async void PlaySound()
	{
		EmitSignal("TriggerBurningOff");
		
		GD.Print("Starting Painting.");
		PaintingOn();

		var arrow = ResourceLoader.Load("Images/brush.png");
		Input.SetCustomMouseCursor(arrow);

		//check if we can progress the spell
		var playingSpace = GetNode("../../PlayingSpace");
		int amountOfBurn = SearchForThing(playingSpace, "Burn");
		GD.Print($"number of burns found: {amountOfBurn}");
		if (amountOfBurn > 50)
		{
			EmitSignal("ProgressSpell", 2);
		}
		else
		{
			EmitSignal("ProgressSpell", -1);
		}
	}

	private int SearchForThing(Node theNode, string thing)
	{
		int valueToReturn = 0;
		if (theNode == null)
		{
			//GD.Print("node is null, returning");
		}
		else
		{
			//GD.Print($"Looking at {theNode.Name}");
			var children = theNode.GetChildren();
			if (theNode.Name.ToString().StartsWith(thing))
			{
				//GD.Print($"found {theNode.Name}, incrementing count.");
				valueToReturn = 1;
			}
			else
			{
				//GD.Print($"No {thing} found, moving on.");
			}

			foreach (var child in children)
			{
				valueToReturn += SearchForThing(child, thing);
			}
		}
		return valueToReturn;
	}
	
	private void DisablePainting()
	{
		canPaint = false;
		GD.Print("Stopping Painting");
	}
}
