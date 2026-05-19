# BURXA

A multi-stage Virtual Reality horror game built in Unity that challenges the player through four distinct, high-tension levels. Each stage is designed around a specific psychological fear and utilizes unique VR interaction mechanics—ranging from precise physical coordination to camera-based perspective puzzles and fast-paced motion shooting.

---

## Gameplay & Level Breakdown

The game is structured into four sequential scenes, each introducing a new mechanical twist and thematic horror element.

### Scene 1: Sarcasm (The Wire Loop Game)
![Scene 1](https://github.com/Meowmeran/CENG316_PROJECT/blob/main/Assets/Textures/Menu/Unity_sQrzd6IWGL.png)
*   **Thematic Core:** High-stakes tension and precision.
*   **The Objective:** The player must complete three increasingly difficult paths by holding a conductive, circular handle and moving it along a twisting wire path.
*   **The Catch:** If the loop touches the wire, the physical circuit completes, detonating an explosive charge that triggers an instant Game Over. Steady hands are mandatory.

### Scene 2: Scopophobia (Object Collection Game)
![Scene 2](https://github.com/Meowmeran/CENG316_PROJECT/blob/main/Assets/Textures/Menu/Unity_ojB5oLU57s.png)
*   **Thematic Core:** The fear of being watched.
*   **The Objective:** The player descends into a grotesque pit of blood and flesh to scavenge for 7 hidden cassette tapes.
*   **The Catch:** A colossal entity known as **The EYE** periodically looms over the pit to survey the area. The player must completely freeze when the eye is observing. Any movement detected during this phase results in a brutal jumpscare and failure.

### Scene 3: Analogy (The Climbing Game)
![Scene 3](https://github.com/Meowmeran/CENG316_PROJECT/blob/main/Assets/Textures/Menu/Unity_bjGHTD9K8b.png)
*   **Thematic Core:** Hidden dangers and low-visibility navigation.
*   **The Objective:** Trapped at the bottom of a dark vertical shaft, the player must physically climb the stone walls to escape. 
*   **The Catch:** The player’s only light source is a handheld camera interface. Certain handholds are infested with **hostile spiders** that are **only visible when viewed through the camera screen**. The player must look through the viewfinder to plan their path and avoid grabbing a possessed stone.

### Scene 4 (Final): Freedom (The Arena Shooter)
![Scene 4](https://github.com/Meowmeran/CENG316_PROJECT/blob/main/Assets/Textures/Menu/Unity_6vQ5eBOcTb.png)
*   **Thematic Core:** Overwhelming force and action payoff.
*   **The Objective:** Upon reaching the surface, the player acquires a high-powered **Fusion Rifle** and must survive an aggressive onslaught of randomized monster spawns.
*   **The Catch:** This final stage transitions into an intense VR arena shooter. The player must successfully eliminate 80 varied enemy types using manual aiming and shooting mechanics to secure their final freedom.

---

## Technical Stack & Frameworks

*   **Game Engine:** Unity (Recommended Version: 2022.3 LTS / 2023.x)
*   **Render Pipeline:** Universal Render Pipeline (URP) with custom post-processing for retro/analog visual filters.
*   **XR Framework:** OpenXR / Unity XR Interaction Toolkit
*   **Key Mechanics Implemented:**
    *   Physics-based VR climbing algorithms (Scene 3).
    *   Continuous collision detection for loop-wire interactions and projectile hits (Scene 1 & Scene 4).
    *   Camera-layer filtering / Render Textures for hidden entity visibility (Scene 3).
    *   Object pooling and random spawning wave systems (Scene 4).

---

## Supported Hardware

This project is built using the **OpenXR** standard and supports standard VR setups, including:
*   Meta Quest 2 / 3 / Pro (via Link/AirLink or standalone build)
*   Valve Index
*   HTC Vive / Cosmos

**Input Requirements:** Dual motion controllers with capacitive touch support or analog trigger configurations (for grabbing handles, climbing stones, and firing weapons).

---
