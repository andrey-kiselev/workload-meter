# Workload Meter
Workload Meter is a Unity3D asset to measure user's mental workload at runtime by using a secondary mathematical task and verbal feedback.
The mathematical tasks are based on the Montreal Imaging Stress Test algorithms. The tasks are presented to the user on the overlay in the bottom of the viewport. The tasks are generated in the way that the correct answer is always a single digit from zero to nine. The responses are collected using keyword spotting.

## Licensing

This software is provided under GNU GENERAL PUBLIC LICENSE Version 3 license conditions. See LICENSE file for details.

## Pre-requesites

This package was only tested on Windows x64 platform. Please, make sure you have Windows Speech enabled and configured.

## Installation

0. If starting a new project, make sure to enable VR in the settings (used to be in PlayerSettings - XR Settings).
1. Import contents into your assets and drop prefab into the scene. The prefab contains an extra camera and rendering canvas for UI elements.
2. Create a new layer called SecondaryTask (or whatever else you like).
3. In you main camera(s) settings uncheck SecondaryTask layer.
4. In the TaskCamera (inside SecondaryTaskManager) set Culling Mask to only SecondaryTask layer.
5. Set the layer of the SecondaryTaskManager to SecondaryTask, including children.
6. Enjoy.

## Usage

The main setting for the workload measurement is the task difficulty in combination with task duration. Currently, only difficulty levels 1 and 2 are implemented. You may need to adjust keywords and feedback texts if you speech recognition language is not US English. Saving log file is disabled by default, you must both, tick the Save Log box and specify log file path to save data.

## Referencing

Please, refer to the following paper if using this package for research:
TBD
