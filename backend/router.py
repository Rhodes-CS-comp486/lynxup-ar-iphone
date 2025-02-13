from flask import Flask, request, jsonify
import firebase_admin
from firebase import db_firestore

app = Flask(__name__)

@app.route("/")
def index():
    return "Index Page"

@app.route("/hello")
def hello_world():
    return "Hello, World!"

# TODO: make this more expansive; let's receive JSON from the frontend
# this will give us more flexibility
@app.route("/add_user", methods=['POST'])
def add_user(username):
    data = request.get_json()

    # Extract fields; what fields?
    # I think a (non-personal) location of origin would actually be really cool; let's add that!
    username = data.get("username")
    name = data.get("name")
    location = data.get("location")

    user = db_firestore.collection("users").document(f"{username}")
    user.set({"name": f"{name}",
              "location": f"{location}"})

    return jsonify({"message" :f"Received name: {name}, location: {location}"}), 200

@app.route("/get_user/<username>")
def get_user(username):
    user = db_firestore.collection("users").document(f"{username}").get()
    return user.to_dict()


