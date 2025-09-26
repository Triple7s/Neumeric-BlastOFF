from flask import Flask, request, jsonify
import os
import json

app = Flask(__name__)

# --- Configure upload folder relative to teacher_server.py ---
BASE_DIR = os.path.dirname(os.path.abspath(__file__))   # .../Assets/Scripts/TeacherServer
UPLOAD_FOLDER = os.path.join(BASE_DIR, "submissions")   # inside TeacherServer/submissions
UPLOAD_FOLDER = os.path.abspath(UPLOAD_FOLDER)

# --- Simulate Android-like persistentDataPath structure ---
ANDROID_BASE = os.path.join(BASE_DIR, "android_persistent")
ANDROID_BASE = os.path.abspath(ANDROID_BASE)

# Ensure both exist
os.makedirs(UPLOAD_FOLDER, exist_ok=True)
os.makedirs(ANDROID_BASE, exist_ok=True)

print("UPLOAD_FOLDER is set to:", UPLOAD_FOLDER)
print("ANDROID_BASE is set to:", ANDROID_BASE)


@app.route("/upload", methods=["POST"])
def upload():
    try:
        data = request.get_json(force=True)
        print("Received:", data)
    except Exception as e:
        return {"status": "error", "message": str(e)}, 400

    student_id = data.get("student_id", "unknown")

    # --- Save to submissions ---
    filepath = os.path.join(UPLOAD_FOLDER, f"{student_id}_answers.json")
    print(f"Saving JSON to: {filepath}")
    with open(filepath, "w", encoding="utf-8") as f:
        json.dump(data, f, indent=4)

    # --- Also save to android-like folder ---
    android_path = os.path.join(ANDROID_BASE, student_id, "files")
    os.makedirs(android_path, exist_ok=True)
    filepath_android = os.path.join(android_path, "answers.json")
    print(f"Saving JSON (Android-style) to: {filepath_android}")
    with open(filepath_android, "w", encoding="utf-8") as f:
        json.dump(data, f, indent=4)

    return jsonify({"status": "success", "message": f"Received answers from {student_id}"})


if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)