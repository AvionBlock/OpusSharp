byte[]? test = null;

unsafe
{
    fixed (byte* ptr = test.AsSpan())
    {
        Console.WriteLine(new IntPtr(ptr));
    }
}