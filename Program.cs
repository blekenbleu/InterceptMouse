// See https://aka.ms/new-console-template for more information
// https://github.com/0x2E757/InputInterceptor/ Example Application
using InputInterceptorNS;
using Context = System.IntPtr;
using Device = System.Int32;

namespace InterceptMouse
{
	internal static class Program
	{
		static void Main()
		{
			if (InitializeDriver())
			{
				MouseHook mouseHook = new(MouseCallback);

				//	KeyboardHook keyboardHook = new KeyboardHook(KeyboardCallback);
				Console.WriteLine("Mouse Hook enabled. Press any keyboard key to release.");
				Console.ReadKey();
				//	keyboardHook.Dispose();	
				mouseHook.Dispose();
			}
			else
			{
				InstallDriver();
			}

			Console.WriteLine("Hooks released. Press any key to exit.");
			Console.ReadKey();
		}

		// https://learn.microsoft.com/en-us/dotnet/framework/interop/how-to-implement-callback-functions
		private static bool MouseCallback(Context context, Device device, ref MouseStroke m)
		{
			try
			{													// Mouse XY coordinates are raw changes
				Console.WriteLine($"Device: {device}; MouseStroke: X:{m.X}, Y:{m.Y}; F:{m.Flags} S:{m.State} I:{m.Information}");
			}
			catch (Exception exception)
			{
				Console.WriteLine($"MouseStroke: {exception}");
			}

			//	m.X = -m.X;		// Invert mouse X
			//	m.Y = -m.Y;		// Invert mouse Y
			return true;
		}

		private static bool KeyboardCallback(Context context, Device device, ref KeyStroke keyStroke)
		{
			try
			{
				Console.WriteLine($"Device: {device}; Keystroke: {keyStroke.Code} {keyStroke.State} {keyStroke.Information}");
			}
			catch (Exception exception)
			{
				Console.WriteLine($"KeyStroke: {exception}");
			}
			// Button swap
			//keyStroke.Code = keyStroke.Code switch {
			//	KeyCode.A => KeyCode.B,
			//	KeyCode.B => KeyCode.A,
			//	_ => keyStroke.Code,
			//};
			return true;
		}

		static Boolean InitializeDriver()
		{
			if (InputInterceptor.CheckDriverInstalled())
			{
				Console.WriteLine("Input interceptor seems to be installed.");
				if (InputInterceptor.Initialize())
				{
					Console.WriteLine("Input interceptor successfully initialized.");
					return true;
				}
			}
			Console.WriteLine("Input interceptor initialization failed.");
			return false;
		}

		static void InstallDriver()
		{
			Console.WriteLine("Input interceptor not installed.");
			if (InputInterceptor.CheckAdministratorRights())
			{
				Console.WriteLine("Installing...");
				if (InputInterceptor.InstallDriver())
				{
					Console.WriteLine("Done! Restart your computer.");
				}
				else
				{
					Console.WriteLine("Something... gone... wrong... :(");
				}
			}
			else
			{
				Console.WriteLine("Restart program with administrator rights so it will be installed.");
			}
		}
	}
}
