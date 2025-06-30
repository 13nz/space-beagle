# 🌌 Space Beagle

**Space Beagle** is a Unity platformer where you control a beagle in space, exploring procedurally generated levels to collect treasure and avoid enemies.
<img width="1440" alt="236686992-2696bf9b-b4ab-472d-9df1-c03f785ad6e4" src="https://github.com/user-attachments/assets/4c015593-c82d-4c27-9483-f730c9835322" />

---

## 🎮 Gameplay

- Navigate through platformer levels made of interconnected rooms.
- Each level is procedurally generated using cellular automata and a pathfinding algorithm.
- Guaranteed path from the **spawn point** (top) to the **exit door** (bottom).
- Player abilities:
  - **Run**
  - **Double-jump**
  - **Dash**
- The player cannot manually move upward, but can reach higher platforms via double-jump and dash.

---

## Playable demo: https://lenzz.itch.io/koni

## 🧩 Level Generation

- Rooms are generated with adjustable **smoothness** and **fill percentage**.
- Each room has directional openings (top, bottom, left, right) to form valid paths.
- Levels grow in **width** and **height** with each new stage.
- Randomized elements:
  - **Backgrounds**
  - **Tilemaps**
  - **Decorative tiles** (non-colliding visuals)

---

## 🎵 Audio

- Random music is selected from a set of tracks for each level.
- Menus use a dedicated background track.
- 🎧 **Original soundtrack included!**

---

## 👾 Enemies

- Spawn randomly on valid surfaces based on surrounding tiles.
- Each enemy has a randomly assigned color.
- Player can defeat enemies by:
  - **Stomping** on them
  - **Dashing** into them
- Enemies can drop:
  - **Hearts** ❤️
  - **Collectables** 💎
- Defeating an enemy grants **+1 point**.

---

## 💎 Collectables

- Placed in reachable locations only.
- Spawning chance depends on:
  - **Rarity**
  - **Point value**
- Hearts cannot be collected if the player is at full health.

---

## 🧠 UI & Controls

- **HUD displays:**
  - 💖 Hearts
  - ⭐ Score
  - 📈 Current Level

- **Controls:**
  - `ESC` → Pause menu
  - `R` → Restart current level

- **Menus:**
  - Main Menu
  - Pause Screen
  - Game Over Screen

All menus contain **Play** and **Quit** buttons.

---

## 📈 Progression & Game Over

- Player health and score carry over between levels.
- Game Over occurs when all hearts are lost.
- Starting a new game resets score to **0**.

---

Enjoy exploring the cosmos with your trusty beagle companion!
