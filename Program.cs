// See https://aka.ms/new-console-template for more information
using InputInterceptorNS;

namespace InterceptMouse
{
	internal static class Program
	{
		static void Main()
		{
			if (!InputInterceptor.Initialize()) {
				Console.WriteLine("Input interceptor not initialized;  probably invalid " + InputInterceptor.DPath);
				return;
			}
			Intercept Mouse = new();
			if(Mouse.Initialize(Console.WriteLine)) // Intercept instance will use Console.WriteLine()
			{
				Console.WriteLine("Mouse Hook enabled. Press any keyboard key to release.");
				Console.ReadKey();
				Mouse.End();

				Console.WriteLine($"..\n..");
				Console.WriteLine($"..\n..");
				Console.WriteLine($"Hooks released;  Mouse count = {Mouse.Count}\n");
			}
			else Console.WriteLine("Interception failed.");

			Console.WriteLine("Press any key to exit.");
			Console.ReadKey();
		}
	}
}
