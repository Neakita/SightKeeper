[![Windows](https://img.shields.io/badge/Windows-10|11-0078D6?logo=windows)](https://www.microsoft.com/windows/)
[![Linux](https://img.shields.io/badge/Linux-X11%20Desktop-yellow?logo=linux)](https://www.linux.org/)
[![.NET](https://img.shields.io/badge/.NET-9-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)

> **Note:** This project is in active development.

> **Disclaimer:** This application is not intended for use in environments where it violates the respective Terms of Service. The developer is not responsible for any misuse.

## About

SightKeeper is a desktop application for Windows and Linux that handles the complete computer vision pipeline for gamified environments, powered by state-of-the-art models.

## How It Works

1.  **Capture:** Utilizes low-level OS APIs (`DirectX Desktop Duplication` on Windows, `MIT-SHM` on Linux X11) for high-performance screen capture.
2.  **Annotate:** Features a custom-built annotation tools for labeling captured images to create datasets for model training.
3.  **Train:** Orchestrates the training of object detection models via local Python/Conda environments.
4.  **Deploy:** Exports trained models to `ONNX` format for efficient inference using `ONNX Runtime`, supporting both CPU and GPU via CUDA.

## Showcase
> TBD
