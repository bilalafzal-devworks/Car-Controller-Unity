# Unity Car Physics: WheelCollider Vehicle Controller

A hands-on vehicle dynamics project in **Unity (C#)** that implements a drivable car using **WheelColliders**, applying **motor torque**, **brake torque**, and **steering** to study how real-world car physics translate into game mechanics. It also includes a smooth third-person **follow camera**.

> 📸 *Add a screenshot or GIF of the car driving here:*
> `![Gameplay](docs/gameplay.gif)`

---

## 📖 Table of Contents

1. [Overview](#-overview)
2. [Features](#-features)
3. [Controls](#-controls)
4. [Scripts](#-scripts)
5. [Physics Concepts Explored](#-physics-concepts-explored)
6. [Scene Setup Guide](#-scene-setup-guide)
7. [Inspector Settings](#-inspector-settings)
8. [Getting Started](#-getting-started)
9. [Known Limitations](#-known-limitations)
10. [Future Improvements](#-future-improvements)
11. [Author](#-author)

---

##  Overview

This project was built to learn how a car is simulated in a game engine. Instead of moving the car by changing its position directly, the car is driven the way a real one is: **torque is applied to the wheels**, the wheels grip the ground through Unity's `WheelCollider` friction model, and the `Rigidbody` moves as a result.

Topics practiced in this project:

- Setting up `WheelCollider` components and syncing them with visible wheel meshes
- Applying **motor torque** and **brake torque**
- **Steering** the front wheels
- Lowering the **center of mass** for stability
- Building a **smooth follow camera**

---

##  Features

-  **Four-wheel drive:** motor torque is applied to all four wheels
-  **Front-wheel steering** with an adjustable maximum angle
-  **Brakes** applied to all four wheels
-  **Adjustable center of mass** to reduce rollover
-  **Wheel mesh syncing** using `WheelCollider.GetWorldPose()`, so the wheels spin and steer visually
-  **Smooth follow camera** using `Vector3.SmoothDamp` and `LookAt`
-  **Auto-setup:** finds wheel colliders and meshes by name and adds a `Rigidbody` if one is missing

---

## 🎮 Controls

| Key | Action |
|---|---|
| `W` / `↑` | Accelerate |
| `S` / `↓` | Reverse |
| `A` / `←` | Steer left |
| `D` / `→` | Steer right |
| `Space` | Brake |

A gamepad also works through Unity's default `Horizontal` and `Vertical` axes.

---

##  Scripts

| Script | Purpose |
|---|---|
| [`CarController.cs`](Assets/Scripts/CarController.cs) | Reads input and drives the car: motor torque, steering, braking, wheel mesh updates |
| [`CarFollow.cs`](Assets/Scripts/CarFollow.cs) | Camera script that follows the car and looks at it |

### `CarController.cs`

On `Awake()` the script:

1. Finds the four `WheelCollider`s under the child object `Wheels/Colliders`
2. Finds the four wheel meshes under `Wheels/Meshes`
3. Gets the `Rigidbody` (or adds one) and sets `centerOfMass`

Every `FixedUpdate()` it runs:

| Method | What it does |
|---|---|
| `HandleMotor()` | Sets `motorTorque = motorForce × verticalInput` on all four wheels |
| `HandleSteering()` | Sets `steerAngle = maxSteerAngle × horizontalInput` on the front wheels |
| `HandleBraking()` | Sets `brakeTorque` on all four wheels while `Space` is held |
| `UpdateWheels()` | Copies each collider's world pose to its wheel mesh |

### `CarFollow.cs`

Finds the car and its `CarPoint` child (the camera anchor). Each physics step the camera looks at the car and moves smoothly toward the anchor with `Vector3.SmoothDamp`.

---

## 🔬 Physics Concepts Explored

| Concept | Where it appears |
|---|---|
| **Motor torque** | `WheelCollider.motorTorque`, measured in N·m. The value is multiplied by throttle input, so `S` gives negative torque (reverse) |
| **Brake torque** | `WheelCollider.brakeTorque` opposes wheel rotation and stops the car |
| **Steering geometry** | Only the front wheels receive a `steerAngle` |
| **Center of mass** | Lowering it with `centerOfMassOffset` makes the car harder to flip |
| **Wheel friction** | Handled by each `WheelCollider`'s forward and sideways friction curves, which you can tune in the Inspector |
| **Fixed timestep physics** | All forces are applied in `FixedUpdate()` so behavior is frame-rate independent |

---

##  Scene Setup Guide

The scripts find objects **by name**, so the hierarchy must match exactly:

```
Free Racing Car Blue Variant        ← CarController.cs + Rigidbody
├── Wheels
│   ├── Colliders
│   │   ├── FrontLeftWheel          ← WheelCollider
│   │   ├── FrontRightWheel         ← WheelCollider
│   │   ├── RearLeftWheel           ← WheelCollider
│   │   └── RearRightWheel          ← WheelCollider
│   └── Meshes
│       ├── FrontLeftWheel          ← wheel mesh
│       ├── FrontRightWheel         ← wheel mesh
│       ├── RearLeftWheel           ← wheel mesh
│       └── RearRightWheel          ← wheel mesh
└── CarPoint                        ← empty object behind/above the car (camera anchor)

Main Camera                         ← CarFollow.cs
```

**Steps**

1. Place your car model in the scene and name the root object `Free Racing Car Blue Variant` (this is the name `CarFollow.cs` searches for).
2. Create the `Wheels/Colliders` and `Wheels/Meshes` children and name the wheels exactly as shown above.
3. Add a `WheelCollider` to each of the four objects under `Colliders`. Position them at the wheel centers.
4. Add `CarController` to the car root. A `Rigidbody` will be added automatically if missing; setting its mass (for example 1000 to 1500) is recommended.
5. Create an empty child called `CarPoint` behind and above the car.
6. Add `CarFollow` to the **Main Camera**.
7. Make sure the ground has a collider.

>  If your car object has a different name, change the string in `GameObject.Find("...")` inside `CarFollow.cs`.

---

## ⚙️ Inspector Settings

Public fields on `CarController`:

| Field | Default | Description |
|---|---|---|
| `Motor Force` | `2500` | Torque applied to each wheel (N·m). Higher means faster acceleration |
| `Brake Force` | `3000` | Brake torque applied to each wheel while `Space` is held |
| `Max Steer Angle` | `30` | Maximum front wheel angle in degrees |
| `Center Of Mass Offset` | `(0, -0.5, 0)` | Lower values make the car more stable |

**Tuning tips**

- Car flips in turns → lower the center of mass offset or reduce steer angle.
- Car feels slow → raise `Motor Force` or lower the `Rigidbody` mass.
- Car slides too much → adjust the `WheelCollider` sideways friction stiffness.

---

## 🚀 Getting Started

### Requirements

- **Unity** (add your version here, e.g. 2022.3 LTS)
- In **Project Settings → Player → Active Input Handling**, choose **Input Manager (Old)** or **Both**. The scripts use the classic `Input` class.

### Run the project

```bash
git clone https://github.com/bilalafzal-devworks/<your-repo-name>.git
```

1. Open **Unity Hub → Add → select the cloned folder**.
2. Open the scene from `Assets/Scenes/`.
3. Press **Play** and drive with `W A S D` and `Space`.

---

## ⚠️ Known Limitations

- The car and camera anchor are found **by name**, so renaming objects breaks the setup.
- Torque is sent equally to all four wheels, with no differential or gearbox.
- `CarController` logs the applied torque to the Console every physics step while accelerating, which can flood the Console.
- `CarFollow` moves the camera in `FixedUpdate()`, which can look slightly jittery. `LateUpdate()` is usually smoother for cameras.
- If the car object isn't found, `CarFollow` throws a `NullReferenceException` before its own null check runs.
- Steering angle is fixed, so it does not reduce at high speed.

---

## 🔮 Future Improvements

- Speed-sensitive steering
- Gearbox and engine torque curve
- Handbrake and drifting (rear sideways friction)
- Suspension tuning and anti-roll bars
- Speedometer UI
- Migrate to Unity's new Input System
- Engine and tire sound effects

---

## 👤 Author

**Muhammad Bilal**
GitHub: [bilalafzal-devworks](https://github.com/bilalafzal-devworks)

---

## 📜 License

Add a license of your choice (for example MIT) or state that the project is for learning purposes only.
