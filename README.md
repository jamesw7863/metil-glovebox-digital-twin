# METIL Glovebox Digital Twin Training Simulator

A Unity-based digital twin of a laboratory glovebox focused on pressure control training. The simulator models simplified pressure dynamics driven by inflow and outflow, injects a leak fault scenario, and evaluates whether the operator can hold pressure within a safe target band for a required duration.

## Demo Overview
- **Goal:** Hold glovebox pressure between **98–105 kPa** for **30 seconds** during a leak.
- **Pass feedback:** Status changes to **PASSED** and the glovebox turns **green**, then the simulation pauses and prompts restart.

## Controls
- **W / S:** Increase or decrease inflow valve
- **D / A:** Increase or decrease outflow valve
- **H:** Toggle help panel
- **SPACE:** Start scenario (from start screen)
- **R:** Restart / return to start screen (depending on current state)

## How It Works
- **Core simulation:** Pressure updates each frame based on inflow and outflow imbalance.
- **Fault injection:** A leak is injected after a short delay to disturb pressure.
- **Training logic:** Tracks time spent in range, triggers pass state after the required hold time.
- **UI:** HUD displays pressure, inflow, outflow, status, objective progress, and help text.
- **Visual feedback:** Glovebox color reflects system state and pass condition.

## Project Structure (Key Scripts)
- `Assets/Scripts/Core/GloveboxSystem.cs`  
  Core pressure simulation and state.
- `Assets/Scripts/Core/TrainingController.cs`  
  Scenario flow, objective tracking, pass condition, restart behavior.
- `Assets/Scripts/UI/HUDController.cs`  
  HUD updates and status display.
- `Assets/Scripts/UI/UIManager.cs`  
  Start screen and UI toggles.
- `Assets/Scripts/Core/GloveboxView.cs`  
  Visual state feedback (color changes).

## Requirements
- Unity **6.3 LTS** (6000.3.x)
- TextMeshPro

## Running
1. Open the project in Unity Hub.
2. Load `Assets/Scenes/Main.unity`.
3. Press **Play**.
4. Press **SPACE** or click **Start** to begin.

## Notes / Limitations
This is a simplified training simulator intended for clarity and responsiveness. It does not model full CFD behavior or real sensor integration.

## Author
James Williams
