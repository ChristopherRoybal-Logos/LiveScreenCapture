# LiveScreenCapture

A Windows Presentation Foundation (WPF) sample application for experimenting with the WPFCap package for live screen and webcam capture with real-time rendering and frame rate control.

## Overview

LiveScreenCapture is a C# WPF sample application created to experiment with using the WPFCap package for live video capture from webcam devices with precise frame rate control. The application can render frames both in visible and hidden windows, making it suitable for screen capture scenarios where the source window doesn't need to be visible.

## Features

- **Webcam Capture**: Capture live video from connected webcam devices
- **Frame Rate Control**: Adjustable FPS (Frames Per Second) with real-time monitoring
- **Hidden Window Rendering**: Ability to capture and render frames from hidden/invisible windows
- **Dynamic FPS Adjustment**: Automatic FPS adjustment to match target frame rate
- **Multiple Background Options**: Toggle between different background colors (Transparent, Black, White, Silver, CadetBlue, DarkCyan)
- **Real-time Display**: Live preview of captured frames in the main window

## Technology Stack

- **Framework**: .NET Framework 4.7.2
- **UI Framework**: Windows Presentation Foundation (WPF)
- **Language**: C# 
- **External Dependencies**: WPFCap 1.2.0 (for webcam capture functionality)

## Project Structure

```
LiveScreenCapture/
├── App.xaml / App.xaml.cs          # Application entry point
├── MainWindow.xaml / .cs           # Main application window with controls
├── TestWindow.xaml / .cs           # Window for rendering captured frames
├── ViewModels/
│   └── TestWindowViewModel.cs      # ViewModel for managing webcam capture
├── Utilities/
│   └── FramesPerSecondUtility.cs   # FPS calculation utility
└── Properties/
    └── AssemblyInfo.cs             # Assembly metadata
```

## Key Components

### MainWindow
The main application window provides:
- Camera device selection dropdown
- FPS control with validation
- Real-time FPS statistics display
- Buttons to control camera and test window visibility
- Live preview of captured frames

### TestWindow
A specialized window that:
- Renders frames at a specified frame rate
- Can operate both visible and hidden
- Supports webcam video overlay
- Dynamically adjusts rendering speed to match target FPS

### TestWindowViewModel
Manages the webcam capture device using the WPFCap library's `CapPlayer` and `CapDevice` components.

### FramesPerSecondUtility
Utility class for converting FPS values to milliseconds for timer intervals.

## How It Works

1. **Initialization**: On startup, the application creates two `TestWindow` instances:
   - One hidden window for capturing frames
   - One visible window that can be toggled on/off

2. **Frame Generation**: The hidden test window generates bitmap frames at the specified FPS using a timer-based mechanism

3. **FPS Management**: 
   - A statistics timer monitors actual frame rate every second
   - The application automatically adjusts the internal FPS to match the target
   - Visual feedback shows both target and actual FPS

4. **Webcam Capture**: When a camera is started:
   - The selected device is initialized through the CapDevice
   - Video frames are rendered in the TestWindow
   - Frames are rotated 180 degrees for proper display

## Usage

1. **Select a Camera**: Choose a webcam device from the dropdown menu
2. **Set FPS**: Enter desired frame rate in the FPS textbox (validated in real-time)
3. **Start Camera**: Click "Start Camera" to begin capturing from the selected device
4. **Toggle Test Window**: Show/hide the visible test window
5. **Toggle Color**: Change the background color of test windows
6. **Monitor Performance**: Watch the "Actual FPS" to see real-time performance

## Building the Project

This is a Visual Studio solution (.sln) that requires:
- Visual Studio 2019 or later
- .NET Framework 4.7.2 or later
- NuGet package manager (for restoring WPFCap dependency)

To build:
```
1. Open LiveScreenCapture.sln in Visual Studio
2. Restore NuGet packages
3. Build the solution (Ctrl+Shift+B)
4. Run the application (F5)
```

## Performance Characteristics

- **FPS Range**: Supports 1-120 FPS with automatic adjustment
- **Resolution**: Test window renders at 1280x720
- **Adaptive Timing**: Automatically increases/decreases internal FPS to match target

## Dependencies

- **WPFCap 1.2.0**: Provides webcam capture capabilities via the `CatenaLogic.Windows.Presentation.WebcamPlayer` namespace

## License

Copyright © 2020

## Notes

- This is a sample application designed to experiment with the WPFCap package
- The application uses `RenderTargetBitmap` for frame generation, which is not very performant but is used here to pull frames for demonstration purposes
- Timer-based frame generation ensures consistent frame rates
- The hidden window rendering technique allows for screen capture without visible UI
