# How to compile
## Install the NDK
Download and extract the android NDK at https://developer.android.com/ndk/downloads. Then set the folder inside of the extracted folder of the NDK as an environment PATH 
## Windows PATH Setup
Search for `Edit the system environment variables` in the windows start menu -> Click on `Environment variables...` -> Search for `Path` under the variable category and click on it so it's selected -> Click on `Edit` -> Add the full path of the ndk folder by clicking on `New` and filling in the empty space that pops up.
## Compile the code
Open a terminal/console **IN THIS DIRECTORY**, then run the follwing command `ndk-build NDK_PROJECT_PATH=. APP_BUILD_SCRIPT=Android.mk`

Code should be compiled, Ignore the 4 `The system cannot find the path specified.` errors.

## What you need
Include the following files into you're mobile/android native project.
- `obj/local/arm64-v8a/libopus.so`
- `obj/local/armeabi-v7a/libopus.so`
- `obj/local/x86/libopus.so`
- `obj/local/x86_64/libopus.so`

These files are static libraries that include the full compiled opus lib.

## Including into build
Copy each `.so` file into their respective folders in you're application like so...
- lib
    - arm64-v8a
        - libopus.so
    - armeabi-v7a
        - libopus.so
    - x86
        - libopus.so
    - x86_64
        - libopus.so

`For Visual Studio Users`: Make sure that when you add each `.so` file to set the build action to `AndroidNativeLibrary` by right clicking on the file, clicking `properties` then select from the dropdown next to `Build Action`.

# Credits
Makefile was yoinked and edited from https://github.com/theeasiestway/android-opus-codec/blob/develop/app/src/main/jni/Android.mk
