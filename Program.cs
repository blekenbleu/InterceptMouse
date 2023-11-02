// See https://aka.ms/new-console-template for more information
// https://github.com/0x2E757/InputInterceptor/ Example Application
using InputInterceptorNS;

if (InitializeDriver())
{
    MouseHook mouseHook = new MouseHook(MouseCallback);
    Console.WriteLine("Mouse Hook enabled. Press any keyboard key to release.");
    Console.ReadKey();
    mouseHook.Dispose();
}
else
{
    InstallDriver();
}

Console.WriteLine("End of program. Press any key.");
Console.ReadKey();

bool MouseCallback(ref MouseStroke mouseStroke)
{
    Console.WriteLine($"MouseStroke: {mouseStroke.X} {mouseStroke.Y} {mouseStroke.Flags} {mouseStroke.State} {mouseStroke.Information}"); // Mouse XY coordinates are raw
    // Invert mouse X
    //mouseStroke.X = -mouseStroke.X;
    // Invert mouse Y
    //mouseStroke.Y = -mouseStroke.Y;
    return true;
}

bool KeyboardCallback(ref KeyStroke keyStroke)
{
    Console.WriteLine($"{keyStroke.Code} {keyStroke.State} {keyStroke.Information}");
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
