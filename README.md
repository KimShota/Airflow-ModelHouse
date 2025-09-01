# Airflow-ModelHouse  

This repository contains scripts for simulating and analyzing **airflow dynamics in architectural model houses**. It combines **C# (Unity)** for interactive 3D visualization and **Python** for preprocessing and analyzing airflow datasets.  

The project aims to provide a framework for studying **ventilation, energy efficiency, and airflow distribution** in scaled architectural models.  

---

## 📂 Repository Structure  

Airflow-ModelHouse/
├── ArrowBatchCombiner.cs # Unity C# script for rendering vector arrows (airflow visualization)
├── processdata.py # Python script for preprocessing airflow datasets (CSV → structured format)
└── README.md # Project documentation


---

## 🔬 Key Features  

- **Unity-Based Airflow Visualization**  
  - `ArrowBatchCombiner.cs` batches vector glyphs (arrows) into combined meshes.  
  - Reduces draw calls for real-time VR/3D rendering.  
  - Supports visualization of **50k+ airflow vectors at 90 FPS** in Unity.  

- **Data Preprocessing (Python)**  
  - `processdata.py` processes raw **fluid dynamics CSVs**.  
  - Performs **downsampling** to reduce file size by >90% while preserving vector magnitude & angle distributions.  
  - Outputs cleaned datasets ready for Unity visualization.  

- **Cross-Language Workflow**  
  - **Python** handles data cleaning and structuring.  
  - **Unity C#** handles rendering and real-time interaction.  

---

## 📊 Workflow  

1. **Prepare Data**  
   - Collect raw airflow simulation/export data (CSV format).  
   - Run preprocessing script:  
     ```bash
     python processdata.py
     ```  
   - This outputs optimized CSVs.  

2. **Visualize in Unity**  
   - Import processed CSV into Unity.  
   - Attach `ArrowBatchCombiner.cs` to your visualization object.  
   - Run the scene → view airflow dynamics as interactive 3D arrows.  

---

## 🛠 Tech Stack  

- **Unity (C#)** → real-time rendering of airflow dynamics.  
- **Python** → data preprocessing with pandas/NumPy.  
- **OpenCV/Matplotlib (optional)** → exploratory visualization before Unity import.  

---

## 🚀 Usage Example  

Preprocess raw data:  
```bash
python processdata.py --input airflow_raw.csv --output airflow_processed.csv


In Unity:

Import airflow_processed.csv.

Use ArrowBatchCombiner.cs to combine arrow meshes.

Press Play → view airflow patterns inside the model house.
