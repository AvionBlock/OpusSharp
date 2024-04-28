#!/bin/bash

# Exit on error
set -e


OPUS_VERSION="1.5.1"
# Directory where the libopus will be built
BUILD_DIR="libopus_build"
# libopus source directory
SOURCE_DIR="libopus-${OPUS_VERSION}"
# Output directory for the final universal library
OUTPUT_DIR="libopus_output"
# Architectures to build
ARCHS="arm64 armv7 armv7s i386 x86_64"
# Minimum iOS version
MIN_IOS_VERSION="9.0"

# Download and extract libopus if it doesn't exist
if [ ! -d "$SOURCE_DIR" ]; then
  echo "Downloading libopus ${OPUS_VERSION}..."
  curl -LO http://downloads.xiph.org/releases/opus/opus-${OPUS_VERSION}.tar.gz
  tar -xzf opus-${OPUS_VERSION}.tar.gz
  mv opus-${OPUS_VERSION} $SOURCE_DIR
  rm opus-${OPUS_VERSION}.tar.gz
fi

# Create build and output directories
mkdir -p $BUILD_DIR
mkdir -p $OUTPUT_DIR

# Build for each architecture
for ARCH in $ARCHS; do
  echo "Building libopus for $ARCH..."
  # Create architecture specific build directory
  ARCH_BUILD_DIR=$BUILD_DIR/$ARCH
  mkdir -p $ARCH_BUILD_DIR
  cd $ARCH_BUILD_DIR
  
  # Configure the build for the current architecture
  SDK=""
  if [[ "$ARCH" == "i386" || "$ARCH" == "x86_64" ]]; then
    SDK="iphonesimulator"
  else
    SDK="iphoneos"
  fi
  DEV_ROOT=`xcrun --sdk $SDK --show-sdk-path`
  HOST=""
  case $ARCH in
    arm64)
      HOST="aarch64-apple-darwin"
      ;;
    armv7)
      HOST="armv7-apple-darwin"
      ;;
    armv7s)
      HOST="armv7s-apple-darwin"
      ;;
    i386)
      HOST="i386-apple-darwin"
      ;;
    x86_64)
      HOST="x86_64-apple-darwin"
      ;;
  esac
  CFLAGS="-arch $ARCH -isysroot $DEV_ROOT -mios-version-min=$MIN_IOS_VERSION"
  LDFLAGS="-arch $ARCH -isysroot $DEV_ROOT"
  
  # Configure and make
  ../../$SOURCE_DIR/configure --disable-shared --enable-static --host=$HOST --prefix=`pwd`/installed CFLAGS="$CFLAGS" LDFLAGS="$LDFLAGS"
  make clean
  make
  make install
  cd ../../
done

echo "Copying binaries to output dir..."
for ARCH in $ARCHS; do
  cp $BUILD_DIR/$ARCH/installed/lib/libopus.a $OUTPUT_DIR/libopus_$ARCH.a
done

echo "libopus.a has been created for each arch at $OUTPUT_DIR"
