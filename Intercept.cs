// https://github.com/0x2E757/InputInterceptor/ Example Application
using InputInterceptorNS;
using System;
using Context = System.IntPtr;
using Device = System.Int32;

namespace InterceptMouse
{
	/// <summary>
	/// Mouse Interception handling for console apps
	/// </summary>
	public class Intercept
	{
		static List<DeviceData>? devices;
		MouseHook Mousehook { get; } = new(MouseCallback);
	//  KeyboardHook keyboardHook = new KeyboardHook(KeyboardCallback);
		public delegate void WriteStatus(string s);
		static WriteStatus Writestring = Console.WriteLine;
		public int Count => (null != devices) ? devices.Count : 0;

		public Intercept()
		{
		}

		public bool Initialize(Intercept.WriteStatus writeString)
		{
			Writestring = writeString;

			if (InputInterceptor.Initialized)
			{
				if (!InputInterceptor.CheckDriverInstalled())
					return InstallDriver();
				else Writestring("Intercept Driver Initialized");
			}
			else
			{
				Console.WriteLine("Input interceptor not initialized;  valid dll probably not found", "Intercept");
				return false;
			}
			return true;
		}

		public void End()
		{
		//	keyboardHook?.Dispose();
			Mousehook?.Dispose();
		}

		// https://learn.microsoft.com/en-us/dotnet/framework/interop/how-to-implement-callback-functions
		private static bool MouseCallback(Context context, Device device, ref MouseStroke m)
		{
			try
			{
				if (null == devices)
					devices = InputInterceptor.GetDeviceList(context, InputInterceptor.IsMouse);

				string scroll = (0 == (0xC00 & (ushort)m.State)) ? "" : $" x:{XY(ref m, 11)}, y:{XY(ref m, 10)}";
				// Mouse XY coordinates are raw changes
				Writestring($"Device: {device}; MouseStroke: X:{m.X}, Y:{m.Y}; S: {m.State}" + scroll);
			}
			catch (Exception exception)
			{
				Console.WriteLine($"MouseStroke: {exception}");
			}

			//  m.X = -m.X;	 // Invert mouse X
			//  m.Y = -m.Y;	 // Invert mouse Y
			return true;
		}

		// decode scrolling
		private static short XY(ref MouseStroke m, short s) { return (short)((((UInt16)m.State >> s) & 1) * ((m.Rolling < 0) ? -1 : 1)); }

		private static bool KeyboardCallback(Context context, Device device, ref KeyStroke keyStroke)
		{
			try
			{
				Writestring($"Device: {device}; Keystroke: {keyStroke.Code} {keyStroke.State} {keyStroke.Information}");
			}
			catch (Exception exception)
			{
				Console.WriteLine($"KeyStroke: {exception}");
			}
		/*	Button swap
			keyStroke.Code = keyStroke.Code switch {
				KeyCode.A => KeyCode.B,
				KeyCode.B => KeyCode.A,
				_ => keyStroke.Code,
			};
		 */
			return true;
		}

		static bool InstallDriver()
		{
			Writestring("Input interception driver not installed.");
			if (InputInterceptor.CheckAdministratorRights())
			{
				Writestring("Installing...");
				if (InputInterceptor.InstallDriver())
					Writestring("Input interception driver installed! Restart your computer.");
				else Writestring("Something... gone... wrong... :(");
			}
			else Writestring("Run InputInterceptori\\Resources\\install-interception.exe to install the required driver.");
			return false;
		}
	}
}
