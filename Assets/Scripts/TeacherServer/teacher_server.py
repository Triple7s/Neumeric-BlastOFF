import os
import json
from flask import Flask, request, jsonify

# -----------------------------------------------------------
# ALWAYS SAVE HERE:
#   C:\Users\<User>\AppData\LocalLow\Tripple 7 GAMES\
#       Neumeric-BlastOFF RE_Calculated\teacher_submissions
#
# This mirrors Unity's Application.persistentDataPath.
# -----------------------------------------------------------

LOCALLOW = os.path.join(
    os.path.expanduser("~"),
    "AppData",
    "LocalLow",
    "Tripple 7 GAMES",
    "Neumeric-BlastOFF RE_Calculated",
)

UPLOAD_FOLDER = os.path.join(LOCALLOW, "teacher_submissions")
os.makedirs(UPLOAD_FOLDER, exist_ok=True)

print("UPLOAD_FOLDER is set to:", UPLOAD_FOLDER, flush=True)

app = Flask(__name__)


@app.route("/upload", methods=["POST"])
def upload():
    try:
        data = request.get_json(force=True)
        print("Received JSON:", data, flush=True)
    except Exception as e:
        print("JSON parse error:", e, flush=True)
        return jsonify({"status": "error", "message": str(e)}), 400

    # Extract a safe student name
    student = data.get("student", {}).get("name", "unknown")
    safe_name = "".join(c for c in student if c.isalnum() or c in " _-").strip()

    filename = f"{safe_name}_answers.json"
    filepath = os.path.join(UPLOAD_FOLDER, filename)

    print("Saving JSON to:", filepath, flush=True)

    try:
        with open(filepath, "w", encoding="utf-8") as f:
            json.dump(data, f, indent=4)

        print(f"__UNITY_JSON_RECEIVED__:{safe_name}", flush=True)
        return jsonify({"status": "success", "file": filename})
    except Exception as e:
        print("File save error:", e, flush=True)
        return jsonify({"status": "error", "message": str(e)}), 500


if __name__ == "__main__":
    print("TeacherServer running on port 5000...", flush=True)
    app.run(host="0.0.0.0", port=5000)