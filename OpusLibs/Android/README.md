# How to compile
## Install the NDK
Download and extract the android NDK at https://developer.android.com/ndk/downloads. Then set the folder inside of the extracted folder of the NDK as an environment PATH by searching for `Edit the system environment variables` in the windows start menu -> Click on `Environment variables...` -> Search for `Path` under the variable category and click on it so it's selected -> Click on `Edit` -> Add the full path of the ndk folder by clicking on `New` and filling in the empty space that pops up.
## Compile the code
Open a terminal/console **IN THIS DIRECTORY**, then run the follwing command `ndk-build NDK_PROJECT_PATH=. APP_BUILD_SCRIPT=Android.mk`

Code should be compiled, Ignore the 4 `The system cannot find the path specified.` errors.
