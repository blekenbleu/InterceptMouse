### Sample mouse interception code using [MP3Martin/InputInterceptor-PersonalFork](https://github.com/blekenbleu/InputInterceptor-PersonalFork) Library
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
`stroke.Mouse` is the instance of [`MouseStroke`](https://github.com/MP3Martin/InputInterceptor-PersonalFork/blob/master/InputInterceptor/Classes/MouseStroke.cs)
 in [`InputInterceptor`](https://github.com/MP3Martin/InputInterceptor-PersonalFork/blob/master/InputInterceptor/InputInterceptor.cs) class,
- which [delegates](https://learn.microsoft.com/en-US/dotnet/csharp/programming-guide/delegates/)
  to [`DllWrapper`](https://github.com/MP3Martin/InputInterceptor-PersonalFork/blob/master/InputInterceptor/DllWrapper.cs) class,  
  - which wraps  [`InterceptionMethods`](https://github.com/MP3Martin/InputInterceptor-PersonalFork/blob/master/InputInterceptor/InterceptionMethods.cs) class methods,  
    - which declare C# interfaces to [**Interception**](https://www.oblita.com/interception.html) driver [C library functions](https://github.com/oblitum/Interception/blob/master/library/interception.c)  

### Other important variables
`Context`:  returned by `InputInterceptor.CreateContext();` during `Hook()`.  
`Predicate`: typically `interception_is_mouse(device)`, obtained from `predicate(device)` in `GetDeviceList()`;  
which also assembles `DeviceData` list of `Device` Int32 for up to 10 keyboard + 10 mouse devices using `GetHardwareId()`  
`Device`: one of `DeviceData` list  
`InterceptionPrecedence`:  obtained by `interception_get_precedence()`  
 &nbsp; &nbsp; &nbsp; from `DeviceIoControl(device.handle, IOCTL_GET_PRECEDENCE,..)`  

### [`MouseCallback()`](program.cs)
 eventually gets called as `this.Callback()` in [`CallbackWrapper()`](https://github.com/MP3Martin/InputInterceptor-PersonalFork/blob/master/InputInterceptor/MouseHook.cs)
 from [`InterceptionMain()`](https://github.com/MP3Martin/InputInterceptor-PersonalFork/blob/master/InputInterceptor/Classes/Hook.cs)  
- With luck, perhaps `this.Device` is available to `MouseCallback()`..?  

### ['GetHardwareId()'](https://github.com/MP3Martin/InputInterceptor-PersonalFork/blob/master/InputInterceptor/InterceptionMethods.cs)
Perhaps more consistent over time than `Device`...
