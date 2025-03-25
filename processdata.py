import pandas as pd

def load_csv(input_file, encoding="ISO-8859-1"):
    """
    Load a CSV file into a pandas DataFrame.

    Parameters:
        input_file (str): The path to the input CSV file.
        encoding (str): The encoding of the CSV file (default is "ISO-8859-1").

    Returns:
        pd.DataFrame: The loaded DataFrame.
    """
    try:
        data = pd.read_csv(input_file, encoding=encoding, skiprows=1)  # Skip the first row
        return data
    except Exception as e:
        raise ValueError(f"Error loading CSV file: {e}")

def filter_data_by_resolution(data, n):
    """
    Filter the DataFrame to keep every nth row.

    Parameters:
        data (pd.DataFrame): The input DataFrame.
        n (int): The interval to retain rows (default is 50).

    Returns:
        pd.DataFrame: The filtered DataFrame.
    """
    if n <= 0:
        raise ValueError("Interval 'n' must be greater than zero.")
    return data.iloc[::n, :]

def save_csv(data, output_file):
    """
    Save the DataFrame to a CSV file.

    Parameters:
        data (pd.DataFrame): The DataFrame to save.
        output_file (str): The path to the output CSV file.

    Returns:
        None
    """
    try:
        data.to_csv(output_file, index=False)
    except Exception as e:
        raise ValueError(f"Error saving CSV file: {e}")

def process_csv(input_file, output_file, n=50, encoding="ISO-8859-1"):
    """
    Process the input CSV to retain every nth data point and save the result.

    Parameters:
        input_file (str): The path to the input CSV file.
        output_file (str): The path to the output CSV file.
        n (int): The interval to retain rows (default is 50).
        encoding (str): The encoding of the CSV file (default is "ISO-8859-1").

    Returns:
        None
    """
    # Load the data
    data = load_csv(input_file, encoding=encoding)
    print(f"Loaded {len(data)} rows from {input_file}.")

    # Filter data by resolution
    filtered_data = filter_data_by_resolution(data, n)
    print(f"Filtered data to {len(filtered_data)} rows using interval {n}.")

    # Save the filtered data to a new CSV file
    save_csv(filtered_data, output_file)
    print(f"Filtered data saved to {output_file}.")

if __name__ == "__main__":
    # Example usage
    input_file = "FujiDataNew1.csv"
    output_file = "output.csv"
    interval = 60 # change interval to modify the number of arrows you want Unity to render 

    process_csv(input_file, output_file, n=interval)
