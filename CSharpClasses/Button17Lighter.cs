using Godot;
using System;
using System.Threading.Tasks;

public partial class Button17Lighter : TextureButton
{
	[Signal]
	public delegate void ProgressSpellEventHandler(int spellStep);
	[Signal]
	public delegate void TriggerPaintingOffEventHandler();
	[Export]
	AudioStream soundToPlay;
	
	bool burning = false;
	int burnNumber = 0;
	int burnTick = 5;
	int currentTick = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (burning == true)
		{
			//don't burn too fast
			if (currentTick % burnTick == 0)
			{
				//burn randomly
				if (GD.Randi() % 3 == 0)
				{
					var positionOfMouse = GetViewport().GetMousePosition();
					//offset for where the flame image is
					positionOfMouse.X += 130;
					positionOfMouse.Y += 20;

					//make burn random size
					float randomScale = (float)GD.RandRange(0.6,1.2);
					string image = "burn";
					var parent = GetNode<CastSpell>("..");
					if (parent.currentSpellStep == 1 || burnNumber > 50)
					{
						image = "goldburn";
					}
					Sprite2D strokeSprite = new Sprite2D
					{
						Name = "Burn" + burnNumber++,
						Texture = (Texture2D)ResourceLoader.Load($"Images/{image}.png"),
						Position = positionOfMouse,
						Scale = new Vector2(randomScale,randomScale)
					};
					parent.AddChild(strokeSprite);
				}
			}
			currentTick++;
		}
	}
	
	private void DisableRocking()
	{
		burning = false;
		GD.Print("Setting cursor image back to normal");
		// Load the custom images for the mouse cursor.
		var arrow = ResourceLoader.Load("Images/arrow.png");

		// Changes only the arrow shape of the cursor.
		// This is similar to changing it in the project settings.
		Input.SetCustomMouseCursor(arrow);
		burnNumber = 0;
	}

	private void BurningOn()
	{
		burning = true;
	}

	
	private async void PlaySound()
	{
		EmitSignal("TriggerPaintingOff");

		GD.Print("commence vibing");
		BurningOn();
		
		var arrow = ResourceLoader.Load("Images/lighter.png");
		Input.SetCustomMouseCursor(arrow);

        AudioStreamPlayer2D newSound = new AudioStreamPlayer2D
        {
            Stream = soundToPlay
        };
		AddChild(newSound);
        newSound.Play();
		
		EmitSignal("ProgressSpell", 1);
	}
}
