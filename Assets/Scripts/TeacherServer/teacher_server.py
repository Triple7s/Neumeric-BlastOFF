from flask import Flask, request, jsonify
import os
import json
import sys

# Unity will pass the submissions folder path as first argument.
# Example:
# python teacher_server.py "C:/MyBuild/MyGame_Data/StreamingAssets/TeacherServer/submissions"

if len(sys.argv) > 1:
    UPLOAD_FOLDER = os.path.abspath(sys.argv[1])
else:
    # Fallback: local folder next to this script
    BASE_DIR = os.path.dirname(os.path.abspath(__file__))
    UPLOAD_FOLDER = os.path.join(BASE_DIR, "submissions")

# Ensure directory exists
os.makedirs(UPLOAD_FOLDER, exist_ok=True)
print("UPLOAD_FOLDER is set to:", UPLOAD_FOLDER)

app = Flask(__name__)

@app.route("/upload", methods=["POST"])
def upload():
    try:
        data = request.get_json(force=True)
        print("Received JSON from student:", data)
    except Exception as e:
        return {"status": "error", "message": str(e)}, 400

    # Extract student name
    student_name = data["student"]["name"]
    safe_name = "".join(c for c in student_name if c.isalnum() or c in " _-").strip()

    filepath = os.path.join(UPLOAD_FOLDER, f"{safe_name}_answers.json")

    print("Saving JSON to:", filepath)

    with open(filepath, "w", encoding="utf-8") as f:
        json.dump(data, f, indent=4)

    return jsonify({"status": "success", "message": f"Saved for {student_name}"})

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)