# How to compile

## Install tools

You need to have `autoconf`, `automake`, and `libtool`, which can be installed via Brew:

```
brew install autoconf automake libtool
```

You also need to have the latest Xcode tools (`xcode-select --install`).

## Configuring the libopus version

You can set the version of libopus by opening the `compile.sh` file and finding the `OPUS_VERSION` variable. Modify it to your heart's content.

## Run the script

Simply run the script:

```
./compile.sh
```
