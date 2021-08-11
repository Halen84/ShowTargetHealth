using RDR2;
using RDR2.Native;
using RDR2.UI;
using System;
using System.Drawing;

namespace ShowTargetHealth
{
	public class ShowHealth : Script
	{
		private float TextSize = 0.3f;		// Lower value = Smaller text
		private float TextPosX = 970f;		// Lower value = the more LEFT the text is
		private float TextPosY = 530f;		// Lower value = the UP the text is

		public ShowHealth()
		{
			Tick += OnTick;
			Interval = 1;
		}

		private void OnTick(object sender, EventArgs e)
		{
			OutputArgument entityOutput = new OutputArgument();
			TextElement text;

			if (Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING))
			{
				if (Function.Call<bool>(Hash.GET_ENTITY_PLAYER_IS_FREE_AIMING_AT, Game.Player, entityOutput))
				{
					Entity target = entityOutput.GetResult<Entity>();
					if (target != null && target.Exists() && target.EntityType == EntityType.Ped)
					{
						// Explicit string interpolation is slightly faster
						text = new TextElement($"{target.Health.ToString()}", new PointF(TextPosX, TextPosY), TextSize);
						text.Draw();
					}
				}
			} else if (Function.Call<bool>(Hash.IS_PLAYER_TARGETTING_ANYTHING, Game.Player))
			{
				if (Function.Call<bool>(Hash.GET_PLAYER_TARGET_ENTITY, Game.Player, entityOutput))
				{
					Entity target = entityOutput.GetResult<Entity>();
					if (target != null && target.Exists() && target.EntityType == EntityType.Ped)
					{
						text = new TextElement($"{target.Health.ToString()}", new PointF(TextPosX, TextPosY), TextSize);
						text.Draw();
					}
				}
			}
		}
	}
}
