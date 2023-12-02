﻿// See https://aka.ms/new-console-template for more information
// https://github.com/0x2E757/InputInterceptor/ Example Application
using InputInterceptorNS;
using Context = System.IntPtr;
using Device = System.Int32;

namespace InterceptMouse
{
	internal static class Program
	{
		static List<DeviceData>? devices;
		static bool once = false;

		static void Main()
		{
			devices = null;

			if (!InitializeDriver())
			{
				InstallDriver();
				return;
			}
			once = true;
			MouseHook mouseHook = new(MouseCallback);
		//	KeyboardHook keyboardHook = new KeyboardHook(KeyboardCallback);

			Console.WriteLine("Mouse Hook enabled. Press any keyboard key to release.");
			Console.ReadKey();

		//	keyboardHook.Dispose();	
			mouseHook.Dispose();

			Console.WriteLine(" \n");
			if (null == devices)
				Console.WriteLine("\nNo devices");
			else
				Console.WriteLine($"\nMouse count = {devices.Count}\n");
			Console.WriteLine("Hooks released. Press any key to exit.");
			Console.ReadKey();
		}

		// https://learn.microsoft.com/en-us/dotnet/framework/interop/how-to-implement-callback-functions
		private static bool MouseCallback(Context context, Device device, ref MouseStroke m)
		{
			try
			{
				if (true == once) {
					once = false;
					devices = InputInterceptor.GetDeviceList(context, InputInterceptor.IsMouse);
				}
				string scroll = (0 == (0xC00 & (UInt16)m.State)) ? "" : $" x:{XY(ref m, 11)}, y:{XY(ref m, 10)}";
				// Mouse XY coordinates are raw changes
				Report($"Device: {device}; MouseStroke: X:{m.X}, Y:{m.Y}; S: {m.State}" + scroll);
			}
			catch (Exception exception)
			{
				Console.WriteLine($"MouseStroke: {exception}");
			}

			//	m.X = -m.X;		// Invert mouse X
			//	m.Y = -m.Y;		// Invert mouse Y
			return true;
		}

		// decode scrolling
		private static short XY(ref MouseStroke m, short s) { return (short)((((UInt16)m.State >> s) & 1) * ((m.Rolling < 0) ? -1 : 1)); }

		private static void Report (string message) { Console.WriteLine(message); }

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
