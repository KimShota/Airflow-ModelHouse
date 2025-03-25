# 🏠 Airflow Visualization in Unity

This Unity project visualizes **airflow inside a model house** using a dataset of over **520,000 data points**. Each airflow vector is displayed as a 3D arrow, with **direction, length, and color** representing the flow's characteristics.

To ensure high performance, this script utilizes **mesh batching** to group arrows by material, significantly reducing draw calls in Unity.

---

## 📁 Project Overview

- **Engine:** Unity
- **Language:** C# (Unity), Python (Data Preprocessing)  
- **Dataset:** CSV file with 520k+ vector data points  
- **Goal:** Efficiently visualize airflow with direction, speed, and temperature info  
- **Method:** Batched rendering using `CombineInstance` and data reduction via Python

---

## 🚀 Features

- ✅ Batches over 500,000 arrows using `CombineInstance`
- ✅ Arrow direction and length based on velocity vector
- ✅ Arrow color based on temperature
- ✅ Scales arrow size to prevent overlap
- ✅ Transparent materials for better 3D visualization
- ✅ Python script to downsample large CSV files for improved Unity performance

---

## 🐍 Python Preprocessing Script

To handle very large datasets efficiently, the included Python script (`process_csv.py`) reduces the number of rows in the input CSV by keeping every *n*th row. This is essential to prevent Unity from being overloaded with too many arrows.

### 📄 Key Functions

- Loads a CSV file while skipping the header row
- Retains every *n*th row to reduce data size
- Saves the result as a new, lightweight CSV for Unity

### ▶️ Example Usage

```bash
python process_csv.py
