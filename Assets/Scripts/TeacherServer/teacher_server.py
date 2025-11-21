from flask import Flask, request, jsonify
import os
import json
import sys

# --- Determine save directory passed from Unity ---
if len(sys.argv) > 1:
    BASE_DIR = sys.argv[1]  # Unity passes Application.persistentDataPath/QTM_Submissions
else:
    # fallback for running manually
    BASE_DIR = os.path.join(os.path.dirname(os.path.abspath(__file__)), "submissions")

BASE_DIR = os.path.abspath(BASE_DIR)

# Create folder if it doesn't exist
os.makedirs(BASE_DIR, exist_ok=True)
print("Saving JSON files to:", BASE_DIR)

app = Flask(__name__)

@app.route("/upload", methods=["POST"])
def upload():
    try:
        data = request.get_json(force=True)
        print("Received:", data)
    except Exception as e:
        return {"status": "error", "message": str(e)}, 400

    # Extract student name safely
    student_name = data["student"]["name"]
    sanitized = "".join(c for c in student_name if c.isalnum() or c in (' ','_','-')).strip()

    file_path = os.path.join(BASE_DIR, f"{sanitized}_answers.json")

    print(f"Saving JSON to: {file_path}")

    with open(file_path, "w", encoding="utf-8") as f:
        json.dump(data, f, indent=4)

    return jsonify({"status": "success", "message": f"Received answers from {student_name}"})


if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)