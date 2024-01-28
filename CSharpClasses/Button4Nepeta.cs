using Godot;
using System;
using System.Threading.Tasks;

public partial class Button4Nepeta : TextureButton
{
	[Signal]
	public delegate void ProgressSpellEventHandler(int spellStep);

	[Export]
	AudioStream soundToPlay;
	int nepetaNumber = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private async void CreateNepeta()
	{
		//get viewport dimensions (broken, gets values way too big. gonna hardcode to get unstuck)
		var viewportOfProject = GetViewportRect().Size;
		var xValToUse = GD.Randi() % viewportOfProject.X;
		var yValToUse = GD.Randi() % viewportOfProject.Y;
		//GD.Print($"creating Nepeta at x: {xValToUse}, y: {yValToUse}");

        Sprite2D nepetaSprite = new Sprite2D
        {
			Name = "Nepeta" + nepetaNumber++,
            Texture = (Texture2D)ResourceLoader.Load("Images/nepeta.png"),
            Position = new Vector2(xValToUse, yValToUse),
            Scale = new Vector2(0.2f, 0.2f)
        };

		//I could try to figure out how to normalize the X and Y co-ordinates to the global scale,
		//or I could just create them under a parent that lives at (0,0) already. doing that
		var parent = GetNode("..");
		parent.AddChild(nepetaSprite);

		AudioStreamPlayer2D newSound = new AudioStreamPlayer2D
        {
            Stream = soundToPlay
        };
		AddChild(newSound);
        newSound.Play();
		while (newSound.Playing)
		{
			await Task.Delay(500);
		}
		newSound.QueueFree();
		
		//check if we can progress the spell
		var playingSpace = GetNode("../../PlayingSpace");
		int amountOfPaint = SearchForThing(playingSpace, "Stroke");
		GD.Print($"number of paints found: {amountOfPaint}");
		if (amountOfPaint > 300)
		{
			EmitSignal("ProgressSpell", 3);
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
}
