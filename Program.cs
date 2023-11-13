// See https://aka.ms/new-console-template for more information
// https://github.com/0x2E757/InputInterceptor/ Example Application
using InputInterceptorNS;
using Device = System.Int32;

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

bool MouseCallback(Device device, ref MouseStroke m)
{
	try {
    	Console.WriteLine($"MouseStroke: X:{m.X}, Y:{m.Y}; F:{m.Flags} S:{m.State} I:{m.Information}; Device: {device}"); // Mouse XY coordinates are raw
	}
    catch (Exception exception) {
        return true;				// try to let keystrokes pass thru
    }
//	m.X = -m.X;		// Invert mouse X
//	m.Y = -m.Y;		// Invert mouse Y
    return true;
}

bool KeyboardCallback(Device device, ref KeyStroke keyStroke)
{
	try {
    	Console.WriteLine($"{keyStroke.Code} {keyStroke.State} {keyStroke.Information}, Device: {device}");
	}
	catch (Exception exception) {
		return false;
	}
    // Button swap
    //keyStroke.Code = keyStroke.Code switch {
    //    KeyCode.A => KeyCode.B,
    //    KeyCode.B => KeyCode.A,
    //    _ => keyStroke.Code,
    //};
    return true;
}

Boolean InitializeDriver()
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

void InstallDriver()
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
