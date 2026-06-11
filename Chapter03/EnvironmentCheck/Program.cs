using System.Reflection;
using System.Runtime.InteropServices;

Console.WriteLine("=== MCP Development Environment Check ===");
Console.WriteLine();

// .NET runtime info
Console.WriteLine($"  .NET Version:    {RuntimeInformation.FrameworkDescription}");
Console.WriteLine($"  OS:              {RuntimeInformation.OSDescription}");
Console.WriteLine($"  Architecture:    {RuntimeInformation.OSArchitecture}");
Console.WriteLine();

// Try to load the MCP assembly
try
{
    var asm = Assembly.Load("ModelContextProtocol");
    Console.WriteLine($"  MCP SDK loaded:  {asm.GetName().Version}");
    Console.WriteLine("  Status:          All good!");
}
catch (Exception ex)
{
    Console.WriteLine($"  MCP SDK:         NOT FOUND ({ex.Message})");
    Console.WriteLine("  Status:          Please install the ModelContextProtocol NuGet package.");
}

Console.WriteLine();
Console.WriteLine("Environment check complete.");
