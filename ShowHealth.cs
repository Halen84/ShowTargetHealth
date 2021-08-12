using RDR2;
using RDR2.Native;
using RDR2.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ShowTargetHealth
{
	public class ShowHealth : Script
	{
		private float TextSize = 0.3f;		// Lower value = Smaller text
		private float TextPosX = 970f;		// Lower value = The more LEFT the text is
		private float TextPosY = 530f;      // Lower value = The more UP the text is

		private Keys ToggleKey = Keys.F9;
		private bool Enabled { get; set; } = true;

		public ShowHealth()
		{
			Tick += OnTick;
			KeyDown += OnKeyDown;
			Interval = 1;
			ReadSettings();
		}

		#region Reading from ini file
		private void ReadSettings()
		{
			IniFile file = new IniFile("scripts\\ShowTargetHealth.ini");
			if (file.Exists())
			{
				if (file.KeyExists("TextSize"))
				{
					string content = file.Read("TextSize");
					int index = content.IndexOf("	");
					if (index >= 0)
					{
						TextSize = float.Parse(content.Substring(0, index));
					} else {
						TextSize = float.Parse(content);
					}
				}
				if (file.KeyExists("TextPosX"))
				{
					string content = file.Read("TextPosX");
					int index = content.IndexOf("	");
					if (index >= 0)
					{
						TextPosX = float.Parse(content.Substring(0, index));
					} else {
						TextPosX = float.Parse(content);
					}
				}
				if (file.KeyExists("TextPosY"))
				{
					string content = file.Read("TextPosY");
					int index = content.IndexOf("	");
					if (index >= 0)
					{
						TextPosY = float.Parse(content.Substring(0, index));
					} else {
						TextPosY = float.Parse(content);
					}
				}
				if (file.KeyExists("ToggleKey"))
				{
					ToggleKey = (Keys)Enum.Parse(typeof(Keys), file.Read("ToggleKey"), true);
				}
			}
		}
		#endregion

		private void OnTick(object sender, EventArgs e)
		{
			OutputArgument entityOutput = new OutputArgument();
			TextElement text;
			if (Enabled)
			{
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
				}
				else if (Function.Call<bool>(Hash.IS_PLAYER_TARGETTING_ANYTHING, Game.Player)) {
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

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == ToggleKey)
			{
				Enabled = !Enabled;
				RDR2.UI.Screen.ShowSubtitle(Enabled == true ? "Enabled" : "Disabled");
			}
		}
	}
}
