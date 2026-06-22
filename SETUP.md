# Mayab Top-Down Shooter - Team Setup

## 1. Clone the Repo

```
git clone <repo-url>
```

Open the project in **Unity 6** (URP).

## 2. Import Required Packages

Open **Window > Package Manager** and install:

- **Input System** (switch to "New Input System" when prompted, or set in Player Settings)
- **Cinemachine**
- **ProBuilder** (optional, for custom level geometry)
- **TextMeshPro** — import TMP Essentials when prompted

## 3. Import POLYGON Apocalypse Pack

This asset is **not included in the repo** (too large + license restrictions).

1. Open **Window > Package Manager**
2. Switch to **My Assets** (top-left dropdown)
3. Search for **POLYGON - Apocalypse Pack**
4. Click **Download**, then **Import**
5. Import into the default location (`Assets/PolygonApocalypse/`)

Unity will automatically reconnect all prefab, material, and scene references because the `.meta` file is tracked in the repo.

> **Do NOT rename or move the folder.** It must stay at `Assets/PolygonApocalypse/` to match existing references.

## 4. Layers Setup

Go to **Edit > Project Settings > Tags and Layers** and create:

| Layer # | Name   |
|---------|--------|
| 6       | Ground |
| 7       | Player |
| 8       | Enemy  |
| 9       | Bullet |
| 10      | Pickup |

## 5. Verify

- Open the game scene in `Assets/Scenes/`
- Hit Play — the player should move with WASD and aim with mouse
- If you see pink materials, the POLYGON pack wasn't imported correctly (repeat step 3)
