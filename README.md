# FirstGodotGame
## 🕹️ 2D Platformer Game (Godot 4.x, C#)

A simple but polished 2D platformer made in **Godot 4** using **C#**, originally based on the [Brackeys YouTube tutorial](https://www.youtube.com/watch?v=LOhfqjmasi0), and extended with original features and improvements. Built as a personal learning project and portfolio piece.

![Screenshot](screenshots/gameplay.png)

---

## 🎯 Features

- 2D platformer character controller (movement & jump)
- Collectable coins with sound and UI counter
- Game win and game over states
- UI menus (HUD, pause, level complete)
- Parallax background
- Sound effects and background music
- Hazard collisions and level reset
- Modular scene structure using Godot's node system
- Entirely coded in C# (Godot .NET)

---

## 🛠 Tech Stack

- **Engine**: [Godot 4.x (C#)](https://godotengine.org/download/windows/)
- **Language**: C# (.NET 6+ compatible)
- **Tools**: Visual Studio / VS Code, Git
- **Target Platforms**: Desktop (Windows), Web (HTML5)

---

## 📁 Project Structure

<pre>
├── Scenes/
│   ├── Player.tscn
│   ├── Coin.tscn
│   ├── Level1.tscn
│   └── UI/
│       ├── HUD.tscn
│       └── MainMenu.tscn
│
├── Scripts/
│   ├── Player.cs
│   ├── Coin.cs
│   ├── GameManager.cs
│   └── UIManager.cs
│
├── assets/
│   ├── sprites/
│   ├── audio/
│   ├── tiles/
│   └── fonts/
│
├── screenshots/
│   └── gameplay.png
│
├── project.godot
└── README.md
</pre>

---

## 🚀 Getting Started

### Requirements

- [Godot 4.x (C# version)](https://godotengine.org/download/windows/)
- [.NET SDK 6.0 or newer](https://dotnet.microsoft.com/en-us/download)

### Run the Game

1. Clone or download this repo.
2. Open the project in **Godot 4 (C#)**.
3. Press **Play** to launch the main scene.

---

## 🎓 Roadmap

| Feature                       | Status        |
|-------------------------------|---------------|
| Core movement + jumping       | ✅ Done        |
| Coin collection + SFX         | ✅ Done        |
| Win + Game Over screen        | 🟡 In progress |
| Enemy and hazards             | ⬜ Planned     |
| Power-ups                     | ⬜ Planned     |
| Multiple levels + transitions | ⬜ Planned     |
| Soundtrack & audio settings   | ⬜ Planned     |
| Game export (HTML5, others)   | ⬜ Planned     |
| Upload to Itch.io             | ⬜ Planned     |

---

## 📸 Screenshots

> Todo : Add `.png` screenshots to the `/screenshots/` folder and link them here:
- Gameplay
- Menus
- Effects

---

## 📚 Learning Resources

- [Brackeys Tutorial (Godot 4 C#)](https://www.youtube.com/watch?v=LOhfqjmasi0)
- [Godot C# Docs](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/)
- [GDScript vs C# in Godot](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_differences.html)

---

## 📜 License

This project is released under the **MIT License**. See `LICENSE` file for details.  
**Note**: Some assets may have their own licenses. Check the `/assets` folder.
