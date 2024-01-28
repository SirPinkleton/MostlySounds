using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

public partial class Button3Pipe : TextureButton
{
	[Signal]
	public delegate void ProgressSpellEventHandler(int spellStep);
	[Signal]
	public delegate void StopMusicEventHandler(Node playingSpace);
	[Export]
	AudioStream soundToPlay;
	[Export]
	AudioStream boomSoundToPlay;
	[Export]
	AudioStream sparkleSoundToPlay;

	bool createSparkles = false;
	int numberOfSparkles = 0;
	int counterForSparkles = 0;

	List<Sprite2D> sparkleSprites = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sparkleSprites = new List<Sprite2D>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (createSparkles)
		{
			if (counterForSparkles < 150)
			{
				//GD.Print($"creating a sparkle: {counterForSparkles}");
				var viewportOfProject = GetViewportRect().Size;
				var xValToUse = GD.Randi() % viewportOfProject.X;
				var yValToUse = GD.Randi() % viewportOfProject.Y;

				Sprite2D sparkleSprite = new Sprite2D
				{
					Name = "Sparkle" + numberOfSparkles++,
					Texture = (Texture2D)ResourceLoader.Load("Images/sparkle.png"),
					Position = new Vector2(xValToUse, yValToUse),
					//Scale = new Vector2(0.8f, 0.8f)
				};
				//random rotation, 360 degrees is 2261 radians
				sparkleSprite.Rotation = GD.Randi() % 2261;
				var parent = GetNode("..");
				parent.AddChild(sparkleSprite);
				sparkleSprites.Add(sparkleSprite);
			}
			else if (counterForSparkles >= 150 && counterForSparkles < 250)
			{
				//GD.Print($"enoguh sparkles, holding {counterForSparkles}");
				//no-op. keep it bright
			}
			else if (counterForSparkles >= 250)
			{
				if (sparkleSprites.Count() == 0)
				{
					createSparkles = false;
				}
				
				//GD.Print($"removing sparkles, {counterForSparkles}");
				//remove sprite from display and local list in reverse order
				var sparkleToRemove = sparkleSprites?.Last();
				sparkleToRemove?.QueueFree();
				sparkleSprites?.RemoveAt(sparkleSprites.Count()-1);
			}

			counterForSparkles++;

			GD.Print(counterForSparkles);
			if (counterForSparkles == 300)
			{
				GD.Print("you won!");
				//congrats, you won!
				var mp3FileToPlay = Directory.GetFiles("Sounds").ToList().Where(filename => filename.Contains("congrats")).First();
				var soundToPlay = ResourceLoader.Load(mp3FileToPlay) as AudioStream;

				AudioStreamPlayer2D newSound = new AudioStreamPlayer2D
				{
					Stream = soundToPlay
				};
				AddChild(newSound);
				newSound.Play();
			}
		}
	}

	private void CreateFallingButton()
	{
		//int XVal = 42;
		//int YVal = 49;
		//float XVal = Position.X;
		//float YVal = Position.Y;
		int XVal = 310;
		int YVal = 67;
        RigidBody2D newButtonPhysics = new RigidBody2D
        {
			Name = "BrokenButton",
            PhysicsMaterialOverride = new PhysicsMaterial(),
			ContactMonitor = true,
			MaxContactsReported = 8,
			GravityScale = 0.5f
        };
        newButtonPhysics.PhysicsMaterialOverride.Bounce = 0.1f;
		newButtonPhysics.BodyEntered += OnBodyEntered;


        Sprite2D newButtonSprite = new Sprite2D
        {
            Texture = (Texture2D)ResourceLoader.Load("Images/pipe-button-3.png"),
            Position = new Vector2(XVal, YVal),
            Scale = new Vector2(0.148f, 0.148f)
        };

        CollisionShape2D newButtonShape = new CollisionShape2D()
		{
			Position = new Vector2(XVal, YVal)
		};
        var shapeDeets = new CircleShape2D
        {
            Radius = 45
        };
        newButtonShape.Shape = shapeDeets;

		//get parent, since we're about to hide this node
		var parent = GetNode("..");
		parent.AddChild(newButtonPhysics);
		newButtonPhysics.AddChild(newButtonSprite);
		newButtonPhysics.AddChild(newButtonShape);
		newButtonPhysics.LinearVelocity = new Vector2(50,-50);
		Hide();
	}

    private async void OnBodyEntered(Node body)
    {
		var parent = GetNode<CastSpell>("..");
		if (parent.currentSpellStep >= 4)
		{
			EmitSignal("StopMusic", parent);
			//create new sound effect of explosion
			AudioStreamPlayer2D boomSound = new AudioStreamPlayer2D
			{
				Stream = boomSoundToPlay,
			};
			AddChild(boomSound);
			boomSound.Play();
			AudioStreamPlayer2D sparkleSound = new AudioStreamPlayer2D
			{
				Stream = sparkleSoundToPlay,
			};
			AddChild(sparkleSound);
			sparkleSound.Play();
			//create a rapidly expanding sparkle until screen is white
			createSparkles = true;
			//after a delay, load a new scene? slowly fade away the white...
			//play the "congratulations, you won!" sound, place credits, and place a restart button
			GD.Print("spell successful!");
			parent.currentSpellStep = 0;
		}
		
		GD.Print("normal collision");
		AudioStreamPlayer2D newSound = new AudioStreamPlayer2D
		{
			Stream = soundToPlay,
		};
		AddChild(newSound);
		newSound.Play();
		if (parent.currentSpellStep == 0)
		{
			EmitSignal("ProgressSpell", 69);
		}
		else if (parent.currentSpellStep > 0)
		{
			EmitSignal("ProgressSpell", -1);
		}
		while (newSound.Playing)
		{
			await Task.Delay(500);
		}
		newSound.QueueFree();

    }
}
