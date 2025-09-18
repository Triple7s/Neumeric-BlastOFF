from flask import Flask, request, jsonify
import os
import json

app = Flask(__name__)
UPLOAD_FOLDER = "submissions"
os.makedirs(UPLOAD_FOLDER, exist_ok = True)

@app.route("/upload", methods=["POST"])
def upload():
    data = request.get_json()
    student_id = data.get("student_id", "unknown")

    filepath = os.path.join(UPLOAD_FOLDER, f"{student_id}_answers.json")
    with open(filepath, "w") as f:
        json.dump(data, f, indent=4)

    return jsonify({"status": "success", "message": f"Received answers from {student_id}"})

if __name__ == "__main__":
    app.run(host="0.0.0.0", port = 5000)        // Teacher listens on local network

    