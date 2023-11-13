### Sample mouse interception code  
using [blekenbleu/InputInterceptor-PersonalFork](https://github.com/blekenbleu/InputInterceptor-PersonalFork)
 fork of [MP3Martin Library](https://github.com/MP3Martin/InputInterceptor-PersonalFork/)  
... which added bool return codes to [0x2E757](https://github.com/0x2E757) @ https://github.com/0x2E757/InputInterceptor/  
which wrapped C# around Francisco Lopes' [**Interception** driver](https://www.oblita.com/interception.html)
 and provided [Example Application](https://github.com/0x2E757/InputInterceptor/#example-application).

Console app invokes `new MouseHook(MouseCallback);`,  
where `MouseCallback` writes `MouseStroke` members to the console until any keystroke is received.  

public struct [MouseStroke](MouseStroke.md) {
```
        public MouseState State;
        public MouseFlags Flags;

        public Int16 Rolling;		// ButtonData

        public Int32 X;
        public Int32 Y;

        public UInt32 Information;
	}

```
`stroke.Mouse` is the instance of [`MouseStroke`](https://github.com/blekenbleu/InputInterceptor-PersonalFork/blob/master/InputInterceptor/Classes/MouseStroke.cs)
 in [`InputInterceptor`](https://github.com/blekenbleu/InputInterceptor-PersonalFork/blob/master/InputInterceptor/InputInterceptor.cs) class,
- which [delegates](https://learn.microsoft.com/en-US/dotnet/csharp/programming-guide/delegates/)
  to [`DllWrapper`](https://github.com/blekenbleu/InputInterceptor-PersonalFork/blob/master/InputInterceptor/DllWrapper.cs) class,  
  - which wraps  [`InterceptionMethods`](https://github.com/blekenbleu/InputInterceptor-PersonalFork/blob/master/InputInterceptor/InterceptionMethods.cs) class methods,  
    - which declare C# interfaces to [**Interception**](https://www.oblita.com/interception.html) driver [C library functions](https://github.com/oblitum/Interception/blob/master/library/interception.c)  

### Other interception variables
`Context`:  returned by `InputInterceptor.CreateContext();` during `Hook()`.  
`Predicate`: typically `interception_is_mouse(device)`, obtained from `predicate(device)` in `GetDeviceList()`;  
  &nbsp; &nbsp; &nbsp; which also assembles `DeviceData` list of `Device` Int32 for up to 10 keyboard + 10 mouse devices using `GetHardwareId()`  
`Device`: numbers from `DeviceData` list, as discovered by Windows;  `1-10` for keyboards, `11-20` for mice.    
  &nbsp; &nbsp; &nbsp; Unplugging and replugging a USB mouse gets it a new (higher) device number.  
`InterceptionPrecedence`:  obtained by `interception_get_precedence()`  
 &nbsp; &nbsp; &nbsp; from `DeviceIoControl(device.handle, IOCTL_GET_PRECEDENCE,..)`  

### [`MouseCallback()`](blob/master/program.cs#L24)
 eventually gets called as `this.Callback()` in [`CallbackWrapper()`](https://github.com/blekenbleu/InputInterceptor-PersonalFork/blob/master/InputInterceptor/MouseHook.cs#L29)
 from [`InterceptionMain()`](https://github.com/blekenbleu/InputInterceptor-PersonalFork/blob/master/InputInterceptor/Classes/Hook.cs#L57)  
- Modified [`CallbackWrapper()`](https://github.com/blekenbleu/InputInterceptor-PersonalFork/blob/master/InputInterceptor/MouseHook.cs#L28) delegate to also pass `this.Device` 

### [`GetHardwareId()`](https://github.com/blekenbleu/InputInterceptor-PersonalFork/blob/master/InputInterceptor/InterceptionMethods.cs#L47)
Perhaps more consistent over time than `Device`... but requires `Context` as well as `Device`

*12 Nov 2023*  
`InterceptMouse.exe` runs OK in Visual Studio debugger, but crashes when invoked from Explorer:  
![](exception.jpg)  
- `Hook.cs` line 68 is inside a try{} and should be caught,  
- `Program.cs` line 24 is a `Console.WriteLine()` using variables checked for non-Null in `Hook.cs`,  
   suggesting that something about `Console.WriteLine()` from callback is problematic...  
