from flask import Flask, request, jsonify
import os
import json

app = Flask(__name__)

# --- Configure upload folder relative to teacher_server.py ---
BASE_DIR = os.path.dirname(os.path.abspath(__file__))   # .../Assets/Scripts/TeacherServer
UPLOAD_FOLDER = os.path.join(BASE_DIR, "submissions")   # inside TeacherServer/submissions
UPLOAD_FOLDER = os.path.abspath(UPLOAD_FOLDER)

os.makedirs(UPLOAD_FOLDER, exist_ok=True)
print("UPLOAD_FOLDER is set to:", UPLOAD_FOLDER)


@app.route("/upload", methods=["POST"])
def upload():
    try:
        data = request.get_json(force=True)
        print("Received JSON:", data)
    except Exception as e:
        return jsonify({"status": "error", "message": f"Invalid JSON: {e}"}), 400

    # 🔹 Extract values according to new JSON format
    student_name = data.get("student", "unknown")
    summary = data.get("summary", {})
    categories = data.get("categories", {})

    total_questions = summary.get("total_questions", 0)
    correct_answers = summary.get("correct_answers", 0)

    # 🔹 Print received info
    print(f"\n Received quiz submission from: {student_name}")
    print(f"   Total Questions: {total_questions}, Correct: {correct_answers}")
    print("   Categories:")
    for cat, stats in categories.items():
        print(f"     {cat.capitalize()}: {stats['correct']}/{stats['total']}")

    # 🔹 Save file under student's name (replace spaces)
    safe_name = student_name.replace(" ", "_")
    filepath = os.path.join(UPLOAD_FOLDER, f"{safe_name}_submission.json")

    with open(filepath, "w", encoding="utf-8") as f:
        json.dump(data, f, indent=4)

    print(f"Saved submission to: {filepath}\n")

    return jsonify({
        "status": "success",
        "message": f"Received quiz results from {student_name}"
    })


if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)