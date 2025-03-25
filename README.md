# 🏠 Airflow Visualization in Unity

This Unity project visualizes **airflow inside a model house** using a dataset of over **520,000 data points**. Each airflow vector is displayed as a 3D arrow, with **direction, length, and color** representing the flow's characteristics.

To ensure high performance, this script utilizes **mesh batching** to group arrows by material, significantly reducing draw calls in Unity.

---

## 📁 Project Overview

- **Engine:** Unity
- **Language:** C#  
- **Dataset:** CSV file with 520k+ vector data points  
- **Goal:** Efficiently visualize airflow with direction, speed, and temperature info  
- **Method:** Batched rendering using `CombineInstance`

---

## 🚀 Features

- ✅ Batches over 500,000 arrows using `CombineInstance`
- ✅ Arrow direction and length based on velocity vector
- ✅ Arrow color based on temperature
- ✅ Scales arrow size to prevent overlap
- ✅ Transparent materials for better 3D visualization

---

## 📦 Setup Instructions

### 1. Add the Script to Your Project

Place `ArrowBatchCombiner.cs` in your Unity project and attach it to an empty GameObject.

### 2. Assign Required Fields in Inspector

- **Shaft Mesh** (e.g., cylinder)
- **Arrowhead Mesh** (e.g., cone)
- **Materials** for different temperature ranges
- **CSV File** with simulation data (as a `TextAsset`)

### 3. Adjust Parameters (optional)

- `batchSize` – Number of arrows per mesh batch  
- `lengthMultiplier` – Controls arrow length  
- `thicknessMultiplier` – Controls shaft thickness  
- `arrowheadScale` – Controls arrowhead size  

---

## 🌡️ Temperature-Based Color Mapping

| Temperature Range (°C) | Material Used    | Color        |
|------------------------|------------------|--------------|
| < 21                   | `redMaterial`     | 🔴 Red        |
| 21 - 22                | `lightBlueMaterial` | 🔷 Light Blue |
| 22 - 23                | `greenMaterial`   | 🟢 Green      |
| 23 - 24                | `lightGreenMaterial` | 🟩 Light Green |
| 24 - 25                | `yellowMaterial`  | 🟡 Yellow     |
| 25 - 27                | `orangeMaterial`  | 🟠 Orange     |
| ≥ 27                   | `blueMaterial`    | 🔵 Blue       |

---

## 🧾 CSV File Format

The script expects a `.csv` file with at least **20 columns**, as follows:

| Index | Name            | Description                             |
|-------|------------------|-----------------------------------------|
| 0–2   | `wiU[X,Y,Z]`     | Measurement point (W'I)                 |
| 3–5   | `wiV[X,Y,Z]`     | Velocity at W'I                         |
| 6–8   | `wiW[X,Y,Z]`     | Direction vector                        |
| 9–11  | `Original[X,Y,Z]`| Original model coordinates              |
| 12–14 | `u, v, w`        | Velocity vector                         |
| 15–17 | `uEnergy, vEnergy, wEnergy` | Energy values             |
| 18    | `pressure`       | Scalar pressure                         |
| 19    | `temperature`    | Temperature at measurement point (°C)  |

> ℹ️ The first line of the CSV should be a header and is skipped during parsing.

---

## 🎯 Use Cases

- Indoor ventilation visualization  
- HVAC system analysis  
- Architecture design validation  
- Educational CFD (Computational Fluid Dynamics) demos

---

## 📸 Screenshots

> *(Add screenshots or animated GIFs here if available)*

---

## 🛠️ To-Do (Optional Improvements)

- [ ] Add a color legend to the scene  
- [ ] Toggle between temperature and pressure visualization  
- [ ] Enable camera fly-through or VR mode  
- [ ] Improve memory handling for larger datasets  

---

## 📃 License

This project is intended for **educational and research use only**.  
Commercial use is not permitted without permission.

---

## 👨‍💻 Author

Created with ❤️ using Unity and C#  
