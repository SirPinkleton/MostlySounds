using Godot;
using System;

public partial class VolumeSlider : HSlider
{
	int masterBus = AudioServer.GetBusIndex("Master");

	private void SliderChanged(float value)
	{
		AudioServer.SetBusVolumeDb(masterBus, value);

		if (value <= -20)
		{
			AudioServer.SetBusMute(masterBus, true);
		}
		else
		{
			AudioServer.SetBusMute(masterBus, false);
		}
	}
}
