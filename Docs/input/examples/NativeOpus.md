<!---
Title: NativeOpus
Xref: nativeopus-examples
--->

Native opus examples.

Note: While directly calling native functions is typically not recommended, it may be beneficial where you may want to implement your own handlers or add missing wrapper features such as the DREDDecoder.

## Using a native function

```csharp
using OpusSharp.Core.SafeHandlers;
using OpusSharp.Core;

unsafe
{
	int error = 0;
	OpusEncoderSafeHandle safeHandle = NativeOpus.opus_encoder_create(48000, 2, (int)OpusPredefinedValues.OPUS_APPLICATION_AUDIO, &error);
	if(error < 0)
		throw new OpusException(((OpusErrorCodes)error).ToString());

	//Successfully created an OpusEncoder native object.

	//Free/Close Object
	safeHandle.Close();
}
```