using System;
// https://github.com/0x2E757/InputInterceptor/ Example Application
using InputInterceptorNS;
using Context = System.IntPtr;
using Device = System.Int32;

namespace InterceptMouse
{
	public class Intercept
	{
		private static List<DeviceData>? devices;
		MouseHook? mouseHook;			// may be null
		KeyboardHook? keyboardHook;
		public delegate void WriteString(string s);
		static WriteString Writestring = Console.WriteLine;

        public Intercept()
		{
		}

        public int Count => (null != devices) ? devices.Count : 0;
        public bool Initialize(Intercept.WriteString writeString)
		{
            Writestring = writeString;
            if (!InitializeDriver())
            {
                InstallDriver();
                return false;
            }
            Writestring("Insert Driver Initialized");

            mouseHook = new(MouseCallback);
        //  keyboardHook = new KeyboardHook(KeyboardCallback);
		
            return true;
		}

		public void End()
        {
			keyboardHook?.Dispose();
			mouseHook?.Dispose();
		}

		// https://learn.microsoft.com/en-us/dotnet/framework/interop/how-to-implement-callback-functions
		private static bool MouseCallback(Context context, Device device, ref MouseStroke m)
		{
			try
			{
            	if (null == devices)
					devices = InputInterceptor.GetDeviceList(context, InputInterceptor.IsMouse);

				string scroll = (0 == (0xC00 & (UInt16)m.State)) ? "" : $" x:{XY(ref m, 11)}, y:{XY(ref m, 10)}";
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

		static Boolean InitializeDriver()
		{
			if (InputInterceptor.CheckDriverInstalled())
			{
				Writestring("Input interceptor seems to be installed.");
				if (InputInterceptor.Initialize())
				{
					Writestring("Input interceptor successfully initialized.");
					return true;
				}
			}
			Writestring("Input interceptor initialization failed.");
			return false;
		}

		static void InstallDriver()
		{
			Writestring("Input interceptor not installed.");
			if (InputInterceptor.CheckAdministratorRights())
			{
				Writestring("Installing...");
				if (InputInterceptor.InstallDriver())
				{
					Writestring("Done! Restart your computer.");
				}
				else
				{
					Writestring("Something... gone... wrong... :(");
				}
			}
			else
			{
				Writestring("Restart program with administrator rights so it will be installed.");
			}
		} 
	}
}
