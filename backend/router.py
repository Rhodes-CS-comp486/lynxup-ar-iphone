from flask import Flask
import firebase_admin
from firebase import db_firestore

app = Flask(__name__)

@app.route("/")
def index():
    return "Index Page"

@app.route("/hello")
def hello_world():
    return "Hello, World!"

@app.route("/add_user/<username>/<name>")
def add_user(username, name):
    user = db_firestore.collection("users").document(f"{username}")
    user.set({"name": f"{name}"})
    return "Success"

@app.route("/get_user/<username>")
def get_user(username):
    user = db_firestore.collection("users").document(f"{username}")
    return user


